using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Order;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CRM.Handlers.OrderHandlers
{
    public class CreateOrderWithRelatedHandler : IRequestHandler<CreateOrderWithRelatedCommand, ResultBaseResponse>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<CreateOrderWithRelatedHandler> _logger;

        public CreateOrderWithRelatedHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser,
            ILogger<CreateOrderWithRelatedHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<ResultBaseResponse> Handle(CreateOrderWithRelatedCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                ValidateCompany(request.CompanyId);

                var order = await MapOrder(request);

                await AddOrderRelatedEntities(request, order);

                _context.Orders.Add(order);
                await SaveChangesAsync(cancellationToken);

                return new ResultBaseResponse { Success = true, Message = "NewOrder created successfully." };
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized access.");
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Entity not found.");
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update exception.");
                throw new SaveDatabaseException(typeof(Order), ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error.");
                throw;
            }
        }

        private void ValidateCompany(Guid requestCompanyId)
        {
            var companyId = _currentUser.GetCompanyId();
            if (companyId != requestCompanyId)
            {
                throw new UnauthorizedAccessException("User is not authorized to create this order.");
            }
        }

        private async Task<Order> MapOrder(CreateOrderWithRelatedCommand request)
        {
            var order = _mapper.Map<Order>(request);
            order.CountryFrom = await GetCountryById(request.CountryFromId);
            order.CountryTo = await GetCountryById(request.CountryToId);
            order.DateFrom = ConvertToUtc(request.DateFrom);
            order.DateTo = ConvertToUtc(request.DateTo);
            order.CreatedAt = DateTime.UtcNow;
            order.CreatedUserId = _currentUser.GetUserId();
            order.OrderStatus = request.LatestStatus;

            order.OrderStatusHistory.Add(new OrderStatusHistory
            {
                OrderId = order.Id,
                DateTime = DateTime.UtcNow,
                OrderStatus = request.LatestStatus,
                CreatedAt = DateTime.UtcNow,
                CreatedUserId = _currentUser.GetUserId()
            });

            return order;
        }

        private async Task<Country> GetCountryById(Guid countryId)
        {
            var country = await _context.Countries.FindAsync(countryId);
            if (country == null)
            {
                throw new KeyNotFoundException("Country not found.");
            }

            return country;
        }

        private async Task AddOrderRelatedEntities(CreateOrderWithRelatedCommand request, Order order)
        {
            foreach (var numberOfPeopleDto in request.NumberOfPeopleCreateDto)
            {
                var numberOfPeople = _mapper.Map<NumberOfPeople>(numberOfPeopleDto);
                numberOfPeople.OrderId = order.Id;
                order.NumberOfPeople.Add(numberOfPeople);
            }

            foreach (var stayDto in request.StaysCreateDto)
            {
                var stay = _mapper.Map<Stays>(stayDto);
                stay.OrderId = order.Id;
                stay.CheckInDate = ConvertToUtc(stayDto.CheckInDate);
                stay.CreatedAt = DateTime.UtcNow;
                stay.CreatedUserId = _currentUser.GetUserId();
                order.Stays.Add(stay);
            }

            foreach (var mealDto in request.MealsCreateDto)
            {
                var meal = _mapper.Map<Meals>(mealDto);
                _context.Meals.Add(meal);
            }

            foreach (var paymentDto in request.PaymentCreateDto)
            {
                var payment = _mapper.Map<Payment>(paymentDto);
                payment.OrderId = order.Id;
                payment.DateTime = ConvertToUtc(paymentDto.DateTime);
                payment.CreatedAt = DateTime.UtcNow;
                payment.CreatedUserId = _currentUser.GetUserId();
                order.Payments.Add(payment);
            }
        }

        private async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error saving changes to the database.");
                throw new SaveDatabaseException(typeof(Order), ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error saving changes to the database.");
                throw;
            }
        }

        private DateTime ConvertToUtc(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Local).ToUniversalTime();
            }

            return dateTime.ToUniversalTime();
        }
    }
}
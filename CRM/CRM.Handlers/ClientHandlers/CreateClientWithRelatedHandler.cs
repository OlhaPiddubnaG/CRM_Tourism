using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Commands.Client;
using CRM.Domain.Commands.PassportInfo;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CRM.Handlers.ClientHandlers
{
    public class CreateClientWithRelatedHandler : IRequestHandler<CreateClientWithRelatedCommand, ResultBaseResponse>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<CreateClientWithRelatedHandler> _logger;

        public CreateClientWithRelatedHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser, ILogger<CreateClientWithRelatedHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<ResultBaseResponse> Handle(CreateClientWithRelatedCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ValidateRequest(request);

                var client = _mapper.Map<Client>(request);

                ValidateUserAuthorization(client.CompanyId);
                await ValidateManagerIdsAsync(request.ManagerIds, cancellationToken);

                InitializeClient(client, request.ManagerIds);
                await AddClientStatusHistoryAsync(client, request.LatestStatus);
                await AddClientPrivateDataAsync(client, request.PassportsCreateDtos, cancellationToken);

                await SaveChangesAsync(cancellationToken);

                return new ResultBaseResponse { Success = true, Message = "Client created successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating client with related data.");
                return new ResultBaseResponse { Success = false, Message = "Error creating client. Please try again." };
            }
        }

        private void ValidateRequest(CreateClientWithRelatedCommand request)
        {
            request.Name = request.Name.ToUpper();
            request.Surname = request.Surname?.ToUpper();
            request.Patronymic = request.Patronymic?.ToUpper();
        }

        private void ValidateUserAuthorization(Guid companyId)
        {
            var currentUserCompanyId = _currentUser.GetCompanyId();
            if (currentUserCompanyId != companyId)
            {
                throw new UnauthorizedAccessException("User is not authorized to create this client.");
            }
        }

        private async Task ValidateManagerIdsAsync(IEnumerable<Guid> managerIds, CancellationToken cancellationToken)
        {
            if (managerIds.Any())
            {
                var currentUserCompanyId = _currentUser.GetCompanyId();
                var users = await _context.Users
                    .Where(u => managerIds.Contains(u.Id) && u.CompanyId == currentUserCompanyId)
                    .ToListAsync(cancellationToken);

                if (users.Count != managerIds.Count())
                {
                    throw new KeyNotFoundException("One or more user IDs are invalid.");
                }
            }
        }

        private void InitializeClient(Client client, IEnumerable<Guid> managerIds)
        {
            client.CurrentStatus = ClientStatus.Active;
            client.CreatedAt = DateTime.UtcNow;
            client.CreatedUserId = _currentUser.GetUserId();
            if (managerIds.Any())
            {
                var currentUserCompanyId = _currentUser.GetCompanyId();
                var users = _context.Users.Where(u => managerIds.Contains(u.Id) && u.CompanyId == currentUserCompanyId).ToList();
                client.Users.AddRange(users);
            }
            _context.Clients.Add(client);
        }

        private async Task AddClientStatusHistoryAsync(Client client, ClientStatus latestStatus)
        {
            var clientStatusHistory = new ClientStatusHistory
            {
                ClientId = client.Id,
                DateTime = DateTime.UtcNow,
                ClientStatus = latestStatus,
                CreatedAt = DateTime.UtcNow,
                CreatedUserId = _currentUser.GetUserId()
            };
            client.ClientStatusHistory.Add(clientStatusHistory);
            await SaveChangesAsync();
        }

        private async Task AddClientPrivateDataAsync(Client client, CreatePassportInfoCommand[] passportsCreateDtos, CancellationToken cancellationToken)
        {
            var clientPrivateDataCommand = new ClientPrivateData { ClientId = client.Id };
            var clientPrivateData = _mapper.Map<ClientPrivateData>(clientPrivateDataCommand);
            clientPrivateData.CreatedAt = DateTime.UtcNow;
            clientPrivateData.CreatedUserId = _currentUser.GetUserId();

            _context.ClientPrivateDatas.Add(clientPrivateData);
            await SaveChangesAsync(cancellationToken);

            foreach (var passport in passportsCreateDtos)
            {
                passport.ClientPrivateDataId = clientPrivateData.Id;
                var passportInfo = _mapper.Map<PassportInfo>(passport);

                passportInfo.CreatedAt = DateTime.UtcNow;
                passportInfo.CreatedUserId = _currentUser.GetUserId();
                _context.PassportInfo.Add(passportInfo);
            }
            await SaveChangesAsync(cancellationToken);
        }

        private async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving changes to the database.");
                throw;
            }
        }
    }
}
using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.NumberOfPeople;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.NumberOfPeopleHandlers;

public class CreateNumberOfPeopleHandler : IRequestHandler<CreateNumberOfPeopleCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public CreateNumberOfPeopleHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CreatedResponse> Handle(CreateNumberOfPeopleCommand request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        var order = await _context.Orders
            .FirstOrDefaultAsync(c => c.Id == request.OrderId &&
                                      c.CompanyId == companyId &&
                                      !c.IsDeleted, cancellationToken);

        if (order == null)
        {
            throw new UnauthorizedAccessException(
                "User is not authorized to create numberOfPeople for a order from a different company.");
        }

        var numberOfPeople = _mapper.Map<NumberOfPeople>(request);

        _context.NumberOfPeople.Add(numberOfPeople);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(NumberOfPeople), ex);
        }

        return new CreatedResponse(numberOfPeople.Id);
    }
}
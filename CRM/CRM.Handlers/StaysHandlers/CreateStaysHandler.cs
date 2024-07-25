using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Stays;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.StaysHandlers;

public class CreateStaysHandler : IRequestHandler<CreateStaysCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public CreateStaysHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CreatedResponse> Handle(CreateStaysCommand request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        var order = await _context.Orders
            .FirstOrDefaultAsync(c => c.Id == request.OrderId &&
                                      c.CompanyId == companyId &&
                                      !c.IsDeleted, cancellationToken);

        if (order == null)
        {
            throw new UnauthorizedAccessException(
                "User is not authorized to create stays for a order from a different company.");
        }

        var stays = _mapper.Map<Stays>(request);

        stays.CreatedAt = DateTime.UtcNow;
        stays.CreatedUserId = _currentUser.GetUserId();

        _context.Stays.Add(stays);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Stays), ex);
        }

        return new CreatedResponse(stays.Id);
    }
}
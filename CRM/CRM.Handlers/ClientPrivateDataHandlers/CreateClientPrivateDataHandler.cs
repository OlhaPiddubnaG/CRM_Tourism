using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.ClientPrivateData;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientPrivateDataHandlers;

public class CreateClientPrivateDataHandler : IRequestHandler<CreateClientPrivateDataCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public CreateClientPrivateDataHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CreatedResponse> Handle(CreateClientPrivateDataCommand request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        var client = await _context.Clients
            .FirstOrDefaultAsync(
                c => c.Id == request.ClientId &&
                     c.CompanyId == companyId &&
                     !c.IsDeleted, cancellationToken);

        if (client == null)
        {
            throw new UnauthorizedAccessException(
                "User is not authorized to create private data for a client from a different company.");
        }

        var clientPrivateData = _mapper.Map<ClientPrivateData>(request);

        clientPrivateData.CreatedAt = DateTime.UtcNow;
        clientPrivateData.CreatedUserId = _currentUser.GetUserId();
        _context.ClientPrivateDatas.Add(clientPrivateData);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(ClientPrivateData), ex);
        }

        return new CreatedResponse(clientPrivateData.Id);
    }
}
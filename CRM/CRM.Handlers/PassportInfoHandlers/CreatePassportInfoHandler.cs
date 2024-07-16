using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.PassportInfo;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.PassportInfoHandlers;

public class CreatePassportInfoHandler : IRequestHandler<CreatePassportInfoCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public CreatePassportInfoHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CreatedResponse> Handle(CreatePassportInfoCommand request, CancellationToken cancellationToken)
    {
        var currentUserCompanyId = _currentUser.GetCompanyId();

        var clientPrivateData = await _context.ClientPrivateDatas
            .Include(cpd => cpd.Client)
            .FirstOrDefaultAsync(cpd => cpd.Id == request.ClientPrivateDataId, cancellationToken);

        if (clientPrivateData == null || clientPrivateData.Client == null)
        {
            throw new NotFoundException(typeof(ClientPrivateData), request.ClientPrivateDataId);
        }

        if (clientPrivateData.Client.CompanyId != currentUserCompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to create passport info for this client.");
        }

        var passportInfo = _mapper.Map<PassportInfo>(request);
        
        passportInfo.CreatedAt = DateTime.UtcNow;
        passportInfo.CreatedUserId = _currentUser.GetUserId();
        _context.PassportInfo.Add(passportInfo);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(PassportInfo), ex);
        }

        return new CreatedResponse(passportInfo.Id);
    }
}
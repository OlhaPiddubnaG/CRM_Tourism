using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Touroperator;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.TouroperatorHandlers;

public class CreateTouroperatorHandler : IRequestHandler<CreateTouroperatorCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public CreateTouroperatorHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CreatedResponse> Handle(CreateTouroperatorCommand request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        if (companyId != request.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to create this touroperator.");
        }

        var touroperator = _mapper.Map<Touroperator>(request);
        
        touroperator.CreatedAt = DateTime.UtcNow;
        touroperator.CreatedUserId = _currentUser.GetUserId();
        _context.Touroperators.Add(touroperator);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Touroperator), ex);
        }

        return new CreatedResponse(touroperator.Id);
    }
}
using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Company;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CompanyHandlers;

public class GetCompanyByIdHandler : IRequestHandler<GetByIdRequest<CompanyResponse>, CompanyResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetCompanyByIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CompanyResponse> Handle(GetByIdRequest<CompanyResponse> request,
        CancellationToken cancellationToken)
    {
        var company = await GetCompanyByIdAsync(request.Id, cancellationToken);

        if (company == null)
        {
            throw new NotFoundException(typeof(Company), request.Id);
        }

        var companyResponse = _mapper.Map<CompanyResponse>(company);
        return companyResponse;
    }

    private async Task<Company> GetCompanyByIdAsync(Guid companyId, CancellationToken cancellationToken)
    {
        var roles = _currentUser.GetRoles();
        if (roles.Contains(RoleType.Admin))
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId, cancellationToken);
        }

        var currentCompanyId = _currentUser.GetCompanyId();
        if (companyId != currentCompanyId)
        {
            throw new ForbiddenException();
        }

        return await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId, cancellationToken);
    }
}

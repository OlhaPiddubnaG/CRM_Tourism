using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Constants;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Company;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CompanyHandlers;

public class GetAllCompaniesHandler : IRequestHandler<GetAllRequest<CompanyResponse>, List<CompanyResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllCompaniesHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<CompanyResponse>> Handle(GetAllRequest<CompanyResponse> request,
        CancellationToken cancellationToken)
    {
        var companies = await GetCompaniesByRoleAsync(cancellationToken);

        var companyResponses = _mapper.Map<List<CompanyResponse>>(companies);
        return companyResponses;
    }

    private async Task<List<Company>> GetCompaniesByRoleAsync(CancellationToken cancellationToken)
    {
        var roles = _currentUser.GetRoles();
        if (roles.Contains(RoleType.Admin))
        {
            return await _context.Companies
                .Where(c => !c.IsDeleted &&
                            c.Name != Constants.DefaultCompanyAdminName)
                .ToListAsync(cancellationToken);
        }

        var companyId = _currentUser.GetCompanyId();
        return await _context.Companies
            .Where(c => c.Id == companyId &&
                        !c.IsDeleted &&
                        c.Name != Constants.DefaultCompanyAdminName)
            .ToListAsync(cancellationToken);
    }
}
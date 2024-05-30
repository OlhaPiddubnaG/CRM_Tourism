using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Company;
using CRM.Handlers.Services;
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
        List<Company> companies;

        var role = _currentUser.GetRoleForCurrentUser();
        if (role == RoleType.Admin)
        {
            companies = await _context.Companies.ToListAsync(cancellationToken);
        }
        else
        {
            var companyId = _currentUser.GetCompanyIdForCurrentUser();
            companies = await _context.Companies.Where(u => u.Id == companyId).ToListAsync(cancellationToken);
        }

        var companyResponses = _mapper.Map<List<CompanyResponse>>(companies);

        return companyResponses;
    }
}
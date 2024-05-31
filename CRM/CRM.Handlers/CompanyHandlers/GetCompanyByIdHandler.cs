using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Company;
using CRM.Handlers.Services;
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
        Company company;

        var roles = _currentUser.GetRoles();
        if (roles == RoleType.Admin)
        {
            company = await _context.Companies
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        }
        else
        {
            var companyId = _currentUser.GetCompanyId();
            company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId && c.Id == request.Id,
                cancellationToken);
        }

        if (company == null)
        {
            throw new NotFoundException(typeof(Company), request.Id);
        }

        var companyResponse = _mapper.Map<CompanyResponse>(company);

        return companyResponse;
    }
}
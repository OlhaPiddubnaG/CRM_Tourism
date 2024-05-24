using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Company;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CompanyHandlers;

public class GetAllCompaniesHandler : IRequestHandler<GetAllRequest<CompanyResponse>, List<CompanyResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCompaniesHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CompanyResponse>> Handle(GetAllRequest<CompanyResponse> request,
        CancellationToken cancellationToken)
    {
        var companies = await _context.Companies.ToListAsync(cancellationToken);
        var companyResponses = _mapper.Map<List<CompanyResponse>>(companies);
        
        return companyResponses;
    }
}
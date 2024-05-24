using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Company;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CompanyHandlers;

public class GetCompanyByIdHandler : IRequestHandler<GetByIdRequest<CompanyResponse>, CompanyResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetCompanyByIdHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompanyResponse> Handle(GetByIdRequest<CompanyResponse> request,
        CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (company == null)
        {
            throw new NotFoundException(typeof(Company), request.Id);
        }

        var companyResponse = _mapper.Map<CompanyResponse>(company);
        
        return companyResponse;
    }
}
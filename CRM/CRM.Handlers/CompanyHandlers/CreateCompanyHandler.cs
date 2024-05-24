using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Company;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CompanyHandlers;

public class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CreateCompanyHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreatedResponse> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var existingCompany =
            await _context.Companies.FirstOrDefaultAsync(c => c.Name == request.Name, cancellationToken);

        if (existingCompany != null)
        {
            throw new ExistException();
        }

        var company = _mapper.Map<Company>(request);
        _context.Companies.Add(company);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Company), ex);
        }

        return new CreatedResponse(company.Id);
    }
}
using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.PassportInfo;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.PassportInfoHandlers;

public class CreatePassportInfoHandler : IRequestHandler<CreatePassportInfoCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CreatePassportInfoHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreatedResponse> Handle(CreatePassportInfoCommand request, CancellationToken cancellationToken)
    {
     
        var passportInfo = _mapper.Map<PassportInfo>(request);
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

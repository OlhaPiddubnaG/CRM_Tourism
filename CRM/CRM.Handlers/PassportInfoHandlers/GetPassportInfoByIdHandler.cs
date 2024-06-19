using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.PassportInfo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.PassportInfoHandlers;

public class GetPassportInfoByIdHandler : IRequestHandler<GetByIdRequest<PassportInfoResponse>, PassportInfoResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetPassportInfoByIdHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PassportInfoResponse> Handle(GetByIdRequest<PassportInfoResponse> request,
        CancellationToken cancellationToken)
    {
        var passportInfo =  await _context.PassportInfo.FirstOrDefaultAsync(c => c.Id == request.Id , cancellationToken);

        if (passportInfo == null)
        {
            throw new NotFoundException(typeof(PassportInfo), request.Id);
        }

        var passportInfoResponse = _mapper.Map<PassportInfoResponse>(passportInfo);
        return passportInfoResponse;
    }
}
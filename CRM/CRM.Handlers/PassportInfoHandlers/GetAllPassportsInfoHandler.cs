using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.PassportInfo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.PassportInfoHandlers;

public class GetAllPassportsInfoHandler : IRequestHandler<GetAllRequest<PassportInfoResponse>, List<PassportInfoResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetAllPassportsInfoHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PassportInfoResponse>> Handle(GetAllRequest<PassportInfoResponse> request,
        CancellationToken cancellationToken)
    {
        var passportInfo = await _context.PassportInfo.ToListAsync(cancellationToken);

        var passportInfoResponses = _mapper.Map<List<PassportInfoResponse>>(passportInfo);
        return passportInfoResponses;
    }
}
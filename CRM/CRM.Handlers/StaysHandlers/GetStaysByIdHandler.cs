using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Stays;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.StaysHandlers;

public class GetStaysByIdHandler : IRequestHandler<GetByIdRequest<StaysResponse>,
    StaysResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetStaysByIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<StaysResponse> Handle(GetByIdRequest<StaysResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var stays = await _context.Stays
            .FirstOrDefaultAsync(s => s.Id == request.Id &&
                                      s.Order.CompanyId == companyId &&
                                      !s.IsDeleted);

        if (stays == null)
        {
            throw new NotFoundException(typeof(Stays), request.Id);
        }

        var staysResponse = _mapper.Map<StaysResponse>(stays);

        return staysResponse;
    }
}
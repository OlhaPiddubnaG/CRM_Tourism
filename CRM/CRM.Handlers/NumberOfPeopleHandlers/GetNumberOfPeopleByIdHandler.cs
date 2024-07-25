using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.NumberOfPeople;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.NumberOfPeopleHandlers;

public class GetNumberOfPeopleByIdHandler : IRequestHandler<GetByIdRequest<NumberOfPeopleResponse>,
    NumberOfPeopleResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetNumberOfPeopleByIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<NumberOfPeopleResponse> Handle(GetByIdRequest<NumberOfPeopleResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var numberOfPeople = await _context.NumberOfPeople
            .FirstOrDefaultAsync(o => o.Id == request.Id &&
                                      o.Order.CompanyId == companyId &&
                                      !o.IsDeleted);

        if (numberOfPeople == null)
        {
            throw new NotFoundException(typeof(NumberOfPeople), request.Id);
        }

        var numberOfPeopleResponse = _mapper.Map<NumberOfPeopleResponse>(numberOfPeople);

        return numberOfPeopleResponse;
    }
}
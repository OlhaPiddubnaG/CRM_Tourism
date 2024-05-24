using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.UserHandlers;

public class GetAllUsersHandler : IRequestHandler<GetAllRequest<UserResponse>, List<UserResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetAllUsersHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<UserResponse>> Handle(GetAllRequest<UserResponse> request,
        CancellationToken cancellationToken)
    {
        var users = await _context.Users.ToListAsync(cancellationToken);
        var userResponses = _mapper.Map<List<UserResponse>>(users);
        
        return userResponses;
    }
}
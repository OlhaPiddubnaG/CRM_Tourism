using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.UserHandlers;

public class GetUserByIdHandler : IRequestHandler<GetByIdRequest<UserResponse>, UserResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetUserByIdHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserResponse> Handle(GetByIdRequest<UserResponse> request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(typeof(User), request.Id);
        }

        var userResponse = _mapper.Map<UserResponse>(user);
        return userResponse;
    }
}
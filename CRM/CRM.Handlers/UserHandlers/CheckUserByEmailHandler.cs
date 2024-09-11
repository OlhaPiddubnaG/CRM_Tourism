using AutoMapper;
using CRM.Admin.Data;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.UserHandlers;

public class CheckUserByEmailHandler: IRequestHandler<GetByEmailRequest<ResultModel>, ResultModel>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CheckUserByEmailHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ResultModel> Handle(GetByEmailRequest<ResultModel> request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Email == request.Email && !u.IsDeleted, cancellationToken);

        if (user == null)
        {
            return new ResultModel
            {
                Success = false,
                Message = $"User with email {request.Email} not found."
            };
        }

        var userResponse = _mapper.Map<UserResponse>(user);

        return new ResultModel
        {
            Success = true,
            Message = "User found"
        };
    }
}
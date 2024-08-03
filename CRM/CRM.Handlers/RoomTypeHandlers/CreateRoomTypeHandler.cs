using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.RoomType;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.RoomTypeHandlers;

public class CreateRoomTypeHandler : IRequestHandler<CreateRoomTypeCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public CreateRoomTypeHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CreatedResponse> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        if (companyId != request.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to create this roomType.");
        }

        var roomType = _mapper.Map<RoomType>(request);

        _context.RoomTypes.Add(roomType);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(RoomType), ex);
        }

        return new CreatedResponse(roomType.Id);
    }
}
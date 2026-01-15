using Hotel.Application.DTOs;
using Hotel.Domain.Entities;
using Hotel.Domain.Repositories;

namespace Hotel.Application.Services;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoomService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddRoomAsync(CreateRoomDto dto)
    {
        var room = new Room(dto.RoomNumber, dto.Type, dto.Price);

        await _unitOfWork.Rooms.AddAsync(room);
        await _unitOfWork.SaveChangesAsync(); 
    }

    public async Task DeleteRoomAsync(int id)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(id);

        if (room == null)
            throw new KeyNotFoundException($"Room with ID {id} not found");

        _unitOfWork.Rooms.Delete(room);
        await _unitOfWork.SaveChangesAsync(); 
    }
}
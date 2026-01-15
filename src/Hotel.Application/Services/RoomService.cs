using Hotel.Application.DTOs;
using Hotel.Domain.Entities;
using Hotel.Domain.Repositories;

namespace Hotel.Application.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _repository;

    public RoomService(IRoomRepository repository)
    {
        _repository = repository;
    }

    public async Task AddRoomAsync(CreateRoomDto dto)
    {
        var room = new Room(dto.RoomNumber, dto.Type, dto.Price);
        await _repository.AddAsync(room);
    }

    public async Task DeleteRoomAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
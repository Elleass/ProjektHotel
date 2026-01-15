using Hotel.Application.DTOs;

namespace Hotel.Application.Services;

public interface IRoomService
{
    Task AddRoomAsync(CreateRoomDto dto);
    Task DeleteRoomAsync(int id);
}
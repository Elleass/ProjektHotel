using Hotel.Application.DTOs;

namespace Hotel.Application.Services;

public interface IRoomService
{
    Task<RoomResponseDto> AddRoomAsync(CreateRoomDto dto);
    Task DeleteRoomAsync(int id);
    Task<RoomResponseDto?> GetRoomByIdAsync(int id);
    Task<List<RoomResponseDto>> GetAllRoomsAsync();
}
using AutoMapper;
using Hotel.Application.DTOs;
using Hotel.Domain.Entities;
using Hotel.Domain.Repositories;

namespace Hotel.Application.Services;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RoomResponseDto> AddRoomAsync(CreateRoomDto dto)
    {
        // AutoMapper:   CreateRoomDto -> Room
        var room = _mapper.Map<Room>(dto);

        await _unitOfWork.Rooms.AddAsync(room);
        await _unitOfWork.SaveChangesAsync();

        // AutoMapper:  Room -> RoomResponseDto
        return _mapper.Map<RoomResponseDto>(room);
    }

    public async Task DeleteRoomAsync(int id)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(id);

        if (room == null)
            throw new KeyNotFoundException($"Room with ID {id} not found");

        _unitOfWork.Rooms.Delete(room);
        await _unitOfWork.SaveChangesAsync(); // Soft delete
    }

    public async Task<RoomResponseDto?> GetRoomByIdAsync(int id)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(id);
        return room == null ? null : _mapper.Map<RoomResponseDto>(room);
    }

    public async Task<List<RoomResponseDto>> GetAllRoomsAsync()
    {
        var rooms = await _unitOfWork.Rooms.GetAllAsync();
        return _mapper.Map<List<RoomResponseDto>>(rooms);
    }
}
using Hotel.Application.DTOs;

namespace Hotel.Application.Services;

public interface IGuestService
{
    Task<GuestResponseDto> CreateGuestAsync(CreateGuestDto dto);
    Task<GuestResponseDto?> GetGuestByEmailAsync(string email);
}
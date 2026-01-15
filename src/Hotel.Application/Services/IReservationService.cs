using Hotel.Application.DTOs;

namespace Hotel.Application.Services;

public interface IReservationService
{
    Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto);
    Task<ReservationResponseDto?> GetReservationByIdAsync(int id);
    Task<List<ReservationResponseDto>> GetReservationsByGuestIdAsync(int guestId);
    Task CancelReservationAsync(int id);
}
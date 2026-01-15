using AutoMapper;
using Hotel.Application.DTOs;
using Hotel.Domain.Entities;
using Hotel.Domain.Repositories;
using Hotel.Domain.ValueObjects;

namespace Hotel.Application.Services;

public class ReservationService : IReservationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReservationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto)
    {
        // ========== 1. WALIDACJA DTO (FluentValidation robi to automatycznie) ==========
        // EndDate > StartDate - sprawdzone przez CreateReservationDtoValidator

        // ========== 2. SPRAWDZENIE CZY GOŚĆ ISTNIEJE ==========
        var guest = await _unitOfWork.Guests.GetByIdAsync(dto.GuestId);
        if (guest == null)
            throw new KeyNotFoundException($"Guest with ID {dto.GuestId} not found");

        // ========== 3. SPRAWDZENIE CZY POKÓJ ISTNIEJE ==========
        var room = await _unitOfWork.Rooms.GetByIdAsync(dto.RoomId);
        if (room == null)
            throw new KeyNotFoundException($"Room with ID {dto.RoomId} not found");

        // ========== 4. SPRAWDZENIE DOSTĘPNOŚCI POKOJU (LOGIKA BIZNESOWA!) ==========
        var dateRange = new DateRange(dto.StartDate, dto.EndDate);

        if (!dateRange.IsValid())
            throw new ArgumentException("Invalid date range");

        // Pobierz dostępne pokoje w danym zakresie dat
        var availableRooms = await _unitOfWork.Rooms.GetAvailableRoomsAsync(dateRange);

        if (!availableRooms.Any(r => r.Id == dto.RoomId))
            throw new InvalidOperationException(
                $"Room {room.RoomNumber} is not available for the selected dates");

        // ========== 5. MAPOWANIE DTO -> ENCJA ==========
        var reservation = _mapper.Map<Reservation>(dto);

        // ========== 6. DODANIE REZERWACJI (bez SaveChanges!) ==========
        await _unitOfWork.Reservations.AddAsync(reservation);

        // ========== 7. AKTUALIZACJA STATUSU POKOJU (opcjonalne) ==========
        // Możesz zdecydować, czy pokój z jedną rezerwacją jest "niedostępny"
        // lub zostawić IsAvailable = true i sprawdzać dostępność przez daty

        // Opcja A: Oznacz pokój jako niedostępny
        // room.IsAvailable = false;
        // _unitOfWork.Rooms.Update(room);

        // ========== 8. JEDNA ATOMOWA TRANSAKCJA!  ==========
        await _unitOfWork.SaveChangesAsync();

        // ========== 9. ZWRÓCENIE WYNIKU (DTO, nie encja!) ==========
        // Pobierz ponownie z relacjami dla pełnego DTO
        var createdReservation = await _unitOfWork.Reservations.GetByIdAsync(reservation.Id);

        if (createdReservation == null)
            throw new InvalidOperationException("Failed to create reservation");

        return _mapper.Map<ReservationResponseDto>(createdReservation);
    }

    public async Task<ReservationResponseDto?> GetReservationByIdAsync(int id)
    {
        var reservation = await _unitOfWork.Reservations.GetByIdAsync(id);
        return reservation == null ? null : _mapper.Map<ReservationResponseDto>(reservation);
    }

    public async Task<List<ReservationResponseDto>> GetReservationsByGuestIdAsync(int guestId)
    {
        var reservations = await _unitOfWork.Reservations.GetByGuestIdAsync(guestId);
        return _mapper.Map<List<ReservationResponseDto>>(reservations);
    }

    public async Task CancelReservationAsync(int id)
    {
        var reservation = await _unitOfWork.Reservations.GetByIdAsync(id);

        if (reservation == null)
            throw new KeyNotFoundException($"Reservation with ID {id} not found");

        // Opcjonalnie: Sprawdź czy rezerwacja nie jest w przeszłości
        if (reservation.StartDate < DateTime.UtcNow)
            throw new InvalidOperationException("Cannot cancel past reservations");

        // Usuń rezerwację
        _unitOfWork.Reservations.Delete(reservation);

        // Opcjonalnie: Przywróć dostępność pokoju
        // var room = await _unitOfWork. Rooms.GetByIdAsync(reservation.RoomId);
        // if (room != null)
        // {
        //     room.IsAvailable = true;
        //     _unitOfWork.Rooms.Update(room);
        // }

        await _unitOfWork.SaveChangesAsync();
    }
}
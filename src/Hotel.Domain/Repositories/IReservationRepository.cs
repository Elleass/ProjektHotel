using Hotel.Domain.Entities;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Repositories;

public interface IReservationRepository : IGenericRepository<Reservation>
{
    // Nadpisana metoda z eager loading
    new Task<Reservation?> GetByIdAsync(int id);

    Task<List<Reservation>> GetByRoomIdAsync(int roomId, DateRange dateRange);
    Task<List<Reservation>> GetByGuestIdAsync(int guestId);
}
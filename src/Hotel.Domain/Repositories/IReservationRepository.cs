using Hotel.Domain.Entities;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Repositories;

public interface IReservationRepository : IGenericRepository<Reservation>
{
    Task<List<Reservation>> GetByRoomIdAsync(int roomId, DateRange dateRange);
    Task<List<Reservation>> GetByGuestIdAsync(int guestId);
}
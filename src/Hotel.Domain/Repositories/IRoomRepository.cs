using Hotel.Domain.Entities;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Repositories;

public interface IRoomRepository : IGenericRepository<Room>
{
    // Metoda biznesowa - zgodnie z wytycznymi z Rozdzia³u 7
    Task<List<Room>> GetAvailableRoomsAsync(DateRange dateRange);
    Task<Room?> GetByIdWithReservationsAsync(int id);
}
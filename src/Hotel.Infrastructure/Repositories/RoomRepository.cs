using Hotel.Domain.Entities;
using Hotel.Domain.Repositories;
using Hotel.Domain.ValueObjects;
using Hotel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Infrastructure.Repositories;

public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    public RoomRepository(HotelDbContext context) : base(context)
    {
    }

    // Metoda biznesowa - zgodnie z wytycznymi 7.2
    public async Task<List<Room>> GetAvailableRoomsAsync(DateRange dateRange)
    {
        if (!dateRange.IsValid())
            throw new ArgumentException("Invalid date range");

        // Pobierz pokoje, które NIE maj¹ rezerwacji w podanym zakresie
        var occupiedRoomIds = await _context.Reservations
            .AsNoTracking()
            .Where(r => r.StartDate < dateRange.EndDate && r.EndDate > dateRange.StartDate)
            .Select(r => r.RoomId)
            .ToListAsync();

        // Zwróæ dostêpne pokoje (AsNoTracking + materialized list)
        return await _dbSet
            .AsNoTracking()
            .Where(r => r.IsAvailable && !occupiedRoomIds.Contains(r.Id))
            .ToListAsync();
    }

    public async Task<Room?> GetByIdWithReservationsAsync(int id)
    {
        // Include dla eager loading, ale wci¹¿ AsNoTracking
        return await _dbSet
            .AsNoTracking()
            .Include(r => r.HotelId) // Jeœli potrzebujesz relacji
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}
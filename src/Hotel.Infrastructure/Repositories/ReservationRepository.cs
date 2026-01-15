using Hotel.Domain.Entities;
using Hotel.Domain.Repositories;
using Hotel.Domain.ValueObjects;
using Hotel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Infrastructure.Repositories;

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(HotelDbContext context) : base(context)
    {
    }

    // ========== NADPISZ GetByIdAsync z eager loading ==========
    public new async Task<Reservation?> GetByIdAsync(int id)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(r => r.Guest)
            .Include(r => r.Room)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Reservation>> GetByRoomIdAsync(int roomId, DateRange dateRange)
    {
        if (!dateRange.IsValid())
            throw new ArgumentException("Invalid date range");

        return await _dbSet
            .AsNoTracking()
            .Include(r => r.Guest)
            .Include(r => r.Room)
            .Where(r => r.RoomId == roomId
                && r.StartDate < dateRange.EndDate
                && r.EndDate > dateRange.StartDate)
            .ToListAsync();
    }

    public async Task<List<Reservation>> GetByGuestIdAsync(int guestId)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(r => r.Room)
            .Where(r => r.GuestId == guestId)
            .OrderByDescending(r => r.StartDate)
            .ToListAsync();
    }
}
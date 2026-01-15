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

    public async Task<List<Reservation>> GetByRoomIdAsync(int roomId, DateRange dateRange)
    {
        if (!dateRange.IsValid())
            throw new ArgumentException("Invalid date range");

        // AsNoTracking + eager loading Guest i Room
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
        // AsNoTracking + eager loading
        return await _dbSet
            .AsNoTracking()
            .Include(r => r.Room)
            .Where(r => r.GuestId == guestId)
            .OrderByDescending(r => r.StartDate)
            .ToListAsync();
    }
}
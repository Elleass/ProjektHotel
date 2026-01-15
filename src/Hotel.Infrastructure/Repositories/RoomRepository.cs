using Hotel.Domain.Entities;
using Hotel.Domain.Repositories;
using Hotel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly HotelDbContext _context;

    public RoomRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Rooms.FindAsync(id);
        if (entity != null)
        {
            _context.Rooms.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
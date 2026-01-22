using Hotel.Domain.Entities;
using Hotel.Domain.Repositories;
using Hotel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore; // <--- Ważne dla metody FirstOrDefaultAsync

namespace Hotel.Infrastructure.Repositories;

public class GuestRepository : GenericRepository<Guest>, IGuestRepository
{
    public GuestRepository(HotelDbContext context) : base(context)
    {
    }

    // Implementacja metody wymaganej przez interfejs
    public async Task<Guest?> GetByEmailAsync(string email)
    {
        return await _context.Set<Guest>()
            .FirstOrDefaultAsync(g => g.Email == email);
    }
}
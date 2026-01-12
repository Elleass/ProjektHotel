using Microsoft.EntityFrameworkCore;

namespace ProjektHotel.Api.Data;

public class HotelDbContext : DbContext
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options)
    {
    }

    // Na razie pusto – encje dodamy póŸniej
}

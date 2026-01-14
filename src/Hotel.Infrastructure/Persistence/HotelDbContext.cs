using Hotel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Infrastructure.Persistence;

public class HotelDbContext : DbContext
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfiguracja precyzji dla cen (wymagane w EF Core dla typu decimal)
        modelBuilder.Entity<Room>()
            .Property(r => r.Price)
            .HasColumnType("decimal(18,2)");

        // Konfiguracja relacji (opcjonalna, EF domyśli się sam, ale warto być precyzyjnym)
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Guest)
            .WithMany()
            .HasForeignKey(r => r.GuestId)
            .OnDelete(DeleteBehavior.Restrict); // Nie chcemy kaskadowo usuwać gości przy usuwaniu rezerwacji

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Room)
            .WithMany()
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
            
        // Seedowanie danych (opcjonalnie na start, żeby baza nie była pusta)
        modelBuilder.Entity<Domain.Entities.Hotel>().HasData(
            new Domain.Entities.Hotel("Grand Hotel", "Warsaw, Poland") { Id = 1 }
        );
    }
}
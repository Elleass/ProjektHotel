using Hotel.Domain.Repositories;
using Hotel.Infrastructure.Persistence;

namespace Hotel.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly HotelDbContext _context;

    // Lazy initialization repozytoriów
    private IRoomRepository? _roomRepository;
    private IGuestRepository? _guestRepository;
    private IReservationRepository? _reservationRepository;

    public UnitOfWork(HotelDbContext context)
    {
        _context = context;
    }

    // Property z lazy initialization
    public IRoomRepository Rooms
    {
        get
        {
            _roomRepository ??= new RoomRepository(_context);
            return _roomRepository;
        }
    }

    public IGuestRepository Guests
    {
        get
        {
            _guestRepository ??= new GuestRepository(_context);
            return _guestRepository;
        }
    }

    public IReservationRepository Reservations
    {
        get
        {
            _reservationRepository ??= new ReservationRepository(_context);
            return _reservationRepository;
        }
    }

    // JEDYNA metoda, która wywołuje SaveChanges! 
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Tutaj dzieje się magia: 
        // - Soft delete (z HotelDbContext.SaveChangesAsync)
        // - Optimistic concurrency (z ConcurrencyTokenInterceptor)
        // - Wszystkie zmiany w JEDNEJ transakcji
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
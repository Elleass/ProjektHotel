namespace Hotel.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRoomRepository Rooms { get; }
    IGuestRepository Guests { get; }
    IReservationRepository Reservations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
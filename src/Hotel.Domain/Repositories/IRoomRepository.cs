using Hotel.Domain.Entities;

namespace Hotel.Domain.Repositories;

public interface IRoomRepository
{
    Task AddAsync(Room room);
    // Na razie tyle wystarczy, YAGNI (You Aren't Gonna Need It) - nie piszemy metod na zapas
}
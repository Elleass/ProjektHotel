using Hotel.Domain.Entities;

namespace Hotel.Domain.Repositories;

public interface IRoomRepository
{
    Task AddAsync(Room room);
    Task DeleteAsync(int id);
}
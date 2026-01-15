using Hotel.Domain.Entities;

namespace Hotel.Domain.Repositories;

public interface IGuestRepository : IGenericRepository<Guest>
{
    Task<Guest?> GetByEmailAsync(string email);
}
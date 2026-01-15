using Hotel.Domain.Entities;
using Hotel.Domain.Repositories;
using Hotel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly HotelDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(HotelDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        // AsNoTracking() - optymalizacja dla read-only (wytyczne 7.3)
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        // AsNoTracking() + zmaterializowana lista (NIE IQueryable!)
        return await _dbSet
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        // BRAK SaveChangesAsync() - to robi Unit of Work! 
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
        // BRAK SaveChangesAsync() - to robi Unit of Work!
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
        // Soft delete będzie obsłużony przez DbContext. SaveChangesAsync
        // BRAK SaveChangesAsync() - to robi Unit of Work!
    }
}
using System.Collections.Generic;
using BBS.Domain.Repositories;
using BBS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BBS.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly BbsContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(BbsContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id).AsTask();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(object id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}

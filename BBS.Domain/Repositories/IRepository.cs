using System.Collections.Generic;

namespace BBS.Domain.Repositories;

public interface IRepository<T, TKey> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(TKey id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(TKey id);
}


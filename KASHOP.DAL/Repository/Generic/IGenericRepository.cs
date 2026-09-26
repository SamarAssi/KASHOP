using System.Linq.Expressions;
using Microsoft.IdentityModel.Abstractions;

namespace KASHOP.DAL;

public interface IGenericRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string[]? includes = null);
    Task<T>? GetOneAsync(Expression<Func<T, bool>> filter, string[]? includes = null);
    Task<T> CreateAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
}

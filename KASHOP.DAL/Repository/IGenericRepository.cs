using System.Linq.Expressions;
using Microsoft.IdentityModel.Abstractions;

namespace KASHOP.DAL;

public interface IGenericRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(string[]? includes = null);
    Task<T> CreateAsync(T entity);
}

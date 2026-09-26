using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace KASHOP.DAL;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        string[]? includes = null
    )
    {
        IQueryable<T> query = AddIncludes(includes);

        if (filter is not null)
        {
            query = query.Where(filter);
        }
        
        return await query.ToListAsync();
    }

    public async Task<T>? GetOneAsync(
        Expression<Func<T, bool>> filter,
        string[]? includes = null
    )
    {
        IQueryable<T> query = AddIncludes(includes);

        return await query.FirstOrDefaultAsync(filter);
    }

    private IQueryable<T> AddIncludes(string[]? includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query;
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _context.AddAsync(entity);

        return entity;
    }

    public void Update(T entity)
    {
        _context.Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        _context.RemoveRange(entities);
    }
}

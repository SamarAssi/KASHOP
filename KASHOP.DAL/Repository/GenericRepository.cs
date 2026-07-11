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

    public async Task<List<T>> GetAllAsync(string[]? includes = null)
    {
        IQueryable<T> query = AddIncludes(includes);
        
        return await query.ToListAsync();
    }

    public async Task<T> GetOneAsync(
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
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        _context.Remove(entity);

        var affected = await _context.SaveChangesAsync();

        return affected > 0;
    }
}

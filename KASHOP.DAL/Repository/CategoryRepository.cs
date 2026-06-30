using Microsoft.EntityFrameworkCore;

namespace KASHOP.DAL;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        var categories = await _context.Categories
            .Include(category => category.Translations)
            .AsNoTracking()
            .ToListAsync();

        return categories;
    }

    public async Task<Category> CreateAsync(Category category)
    {
        await _context.AddAsync(category);
        await _context.SaveChangesAsync();

        return category;
    }
}

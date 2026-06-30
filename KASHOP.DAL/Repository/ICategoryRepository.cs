namespace KASHOP.DAL;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category> CreateAsync(Category category);
}

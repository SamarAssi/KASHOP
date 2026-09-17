
namespace KASHOP.DAL;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private ICategoryRepository? _categoryRepository;
    private IProductRepository? _productRepository;
    public ICategoryRepository CategoryRepository 
    { 
        get
        {
            if (_categoryRepository is null)
            {
                _categoryRepository = new CategoryRepository(_context);
            }

            return _categoryRepository;
        } 
    }
    public IProductRepository ProductRepository
    {
        get
        {
            if (_productRepository is null)
            {
                _productRepository = new ProductRepository(_context);
            }

            return _productRepository;
        }
    }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}

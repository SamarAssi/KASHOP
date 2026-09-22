namespace KASHOP.DAL;

public interface IUnitOfWork : IAsyncDisposable
{
    ICategoryRepository CategoryRepository { get; }
    IProductRepository ProductRepository { get; }
    ICartRepository CartRepository { get; }

    Task<int> CompleteAsync();
}

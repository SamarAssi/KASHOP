namespace KASHOP.DAL;

public interface IUnitOfWork : IAsyncDisposable
{
    ICategoryRepository CategoryRepository { get; }
    IProductRepository ProductRepository { get; }

    Task<int> CompleteAsync();
}

namespace KASHOP.DAL;

public class CartRepository : GenericRepository<CartItem>, ICartRepository
{
    public CartRepository(ApplicationDbContext context) : base(context)
    { 
    }
}

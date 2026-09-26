namespace KASHOP.DAL;

public class CartItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; }
    public int Count { get; set; }
}

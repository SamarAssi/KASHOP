namespace KASHOP.DAL;

public class CartItemResponse
{
    public int ProductId { get; set; }
    public int ProductName { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
    public string MainImage { get; set; } = null!;
    public decimal TotalPrice => Price * Count;
}

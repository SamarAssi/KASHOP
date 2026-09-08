using System.ComponentModel;

namespace KASHOP.DAL;

public class ProductResponse
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public int Quantity { get; set; }
    public double Rate { get; set; }
    public string MainImage { get; set; } = null!;
    public int CategoryId { get; set; } 
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}

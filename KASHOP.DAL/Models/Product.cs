namespace KASHOP.DAL;

public class Product : AuditLog
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public string MainImage { get; set; } = null!;
    public int Quantity { get; set; }
    public double Rate { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public List<ProductTranslation> Translations { get; set; }
}

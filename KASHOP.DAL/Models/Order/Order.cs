namespace KASHOP.DAL;

public class Order
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? ShippedDate { get; set; }

    public decimal? AmountPaid { get; set; }
    public decimal? TotalAmount { get; set; }

    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public PaymentStatus PayStatus { get; set; } = PaymentStatus.Pending;
    public PaymentMethod PayMethod { get; set; }

    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}

using Order.Domain.Enums;

namespace Order.Domain.Entities;

public class OrderHeader
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public DateTime CreatedDate { get; set; }
    public OrderStatus Status { get; set; }
    public string ErrorMessage { get; set; }
    public string Line { get; set; }
    public string Province { get; set; }
    public string District { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}

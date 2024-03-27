namespace Order.Shared;

public class OrderCreatedEvent
{
    public int OrderId { get; set; }
    public string BuyerId { get; set; }

    public PaymentMessage Payment { get; set; }
    public List<OrderItemMessage> OrderItems { get; set; } = new List<OrderItemMessage>();
}

public class OrderItemMessage
{
    public int ProductId { get; set; }
    public int Count { get; set; }
}
public class PaymentMessage
{
    public string CardName { get; set; }
    public string CardNumber { get; set; }
    public string Expiration { get; set; }
    public string CVV { get; set; }
    public decimal TotalPrice { get; set; }
}
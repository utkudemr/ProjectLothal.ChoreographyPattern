﻿namespace Order.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public int OrderId { get; set; }
    public OrderHeader Order { get; set; }
    public int Count { get; set; }
}



namespace Order.Application.Features.Order.Commands.Create
{
    public class OrderCreateDto
    {
        public string CustomerId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public PaymentDto OrderPayment { get; set; }
        public AddressDto OrderAddress { get; set; }
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }

    public class PaymentDto
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
    }

    public class AddressDto
    {
        public string Line { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
    }
}

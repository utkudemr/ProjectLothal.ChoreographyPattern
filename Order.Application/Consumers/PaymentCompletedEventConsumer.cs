
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order.Domain.Enums;
using Order.Persistance.Contexts;
using Order.Shared;

namespace Order.Application.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly OrderDbContext _orderDbContext;
        private readonly ILogger<PaymentCompletedEventConsumer> _logger;
        public PaymentCompletedEventConsumer(OrderDbContext orderDbContext, ILogger<PaymentCompletedEventConsumer> logger)
        {
            _orderDbContext = orderDbContext;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
           var order = await _orderDbContext.Orders.FirstOrDefaultAsync(a=>a.Id == context.Message.OrderId);
            if (order == null)
            {
                _logger.LogError("Order couldn't retrieved from db orderId: {id}", context.Message.OrderId);
            }
            else
            {
                order.Status = OrderStatus.Complete;
                await _orderDbContext.SaveChangesAsync();
                _logger.LogInformation("Order status changed  orderId:{id} status: {order.Status}",
                    order.Id,order.Status
                    );
            }
        }
    }
}

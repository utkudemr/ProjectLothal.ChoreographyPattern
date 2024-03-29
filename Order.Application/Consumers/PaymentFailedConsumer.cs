using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order.Domain.Enums;
using Order.Persistance.Contexts;
using Order.Shared;

namespace Order.Application.Consumers
{
    public class PaymentFailedConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly OrderDbContext _orderDbContext;
        private readonly ILogger<PaymentFailedConsumer> _logger;

        public PaymentFailedConsumer(OrderDbContext orderDbContext, ILogger<PaymentFailedConsumer> logger)
        {
            _orderDbContext = orderDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            try
            {
                var order = await _orderDbContext.Orders.FirstOrDefaultAsync(a => a.Id == context.Message.OrderId);
                if (order is null)
                {
                    _logger.LogError("Order couldn't retrieved from db id: {id}", context.Message.OrderId);
                }
                else
                {
                    order.Status = OrderStatus.Fail;
                    order.ErrorMessage = "Order Payment Failed";
                    await _orderDbContext.SaveChangesAsync();
                    _logger.LogInformation("Order couldn't retrieved from db id: {id}", context.Message.OrderId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured when retrieveing order from db id: {id}", context.Message.OrderId);
            }

        }
    }
}

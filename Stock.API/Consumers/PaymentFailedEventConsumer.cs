using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Shared;
using Stock.API.Contexts;

namespace Stock.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly StockDbContext _stockDbContext;
        private ILogger<PaymentFailedEventConsumer> _logger;
        public PaymentFailedEventConsumer(StockDbContext stockDbContext, ILogger<PaymentFailedEventConsumer> logger)
        {
            _stockDbContext = stockDbContext;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            try
            {
                foreach (var orderItem in context.Message.OrderItems)
                {
                    var item = await _stockDbContext.Stocks.FirstOrDefaultAsync(a => a.ProductId == orderItem.ProductId);
                    item.Count += orderItem.Count;
                    await _stockDbContext.SaveChangesAsync();
                }
                _logger.LogWarning("Payment Failed Stock re added  OrderId: {orderId}", context.Message.OrderId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex,"An error occured when Payment Failed Stock re added  OrderId: {orderId}", context.Message.OrderId);
            }
        }
    }
}

using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Shared;
using Stock.API.Contexts;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly StockDbContext _context;
        private ILogger<OrderCreatedEventConsumer> _logger;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedEventConsumer(
            StockDbContext context,
            ILogger<OrderCreatedEventConsumer> logger,
            ISendEndpointProvider sendEndpointProvider,
            IPublishEndpoint publishEndpoint
            )
        {
            _context = context;
            _logger = logger;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            bool hasStockError = false;
            try
            {
                foreach (var item in context.Message.OrderItems)
                {
                    var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId && x.Count > item.Count);

                    if (stock != null) stock.Count -= item.Count;
                    else hasStockError = true;
                }
                if (!hasStockError)
                {
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Stock was reserved for Buyer Id :{context.Message.BuyerId}");

                    await _publishEndpoint.Publish(new StockReservedEvent()
                    {
                        Payment = context.Message.Payment,
                        BuyerId = context.Message.BuyerId,
                        OrderId = context.Message.OrderId,
                        OrderItems = context.Message.OrderItems
                    });

                }
                else
                {
                    await _publishEndpoint.Publish(new StockNotReservedEvent()
                    {
                        OrderId = context.Message.OrderId,
                        Message = "Not enough stock"
                    });

                    _logger.LogInformation("Not enough stock for Buyer Id :{buyerId}", context.Message.BuyerId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occured when consuming OrderCreatedEvent :{ex}", ex.Message);
                throw;
            }
        }
    }
}

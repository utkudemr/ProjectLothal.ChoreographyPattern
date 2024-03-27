using MassTransit;
using Order.Shared;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly ILogger<StockReservedEventConsumer> _logger;
        private const decimal PaymentBalance = 5000;
        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(ILogger<StockReservedEventConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            if (PaymentBalance > context.Message.Payment.TotalPrice)
            {
                await _publishEndpoint.Publish(new PaymentCompletedEvent { BuyerId = context.Message.BuyerId, OrderId = context.Message.OrderId });
            }
            else
            {
                var failedEvent = new PaymentFailedEvent { 
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId, 
                    Message = "not enough balance",
                    OrderItems = context.Message.OrderItems
                };
                await _publishEndpoint.Publish(failedEvent);
            }
        }
    }
}

using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Domain.Enums;
using Order.Persistance.Contexts;
using Order.Shared;

namespace Order.Application.Consumers;

public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
{
    private readonly OrderDbContext _context;

    private readonly ILogger<StockNotReservedEventConsumer> _logger;

    public StockNotReservedEventConsumer(OrderDbContext context, ILogger<StockNotReservedEventConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
    {
        var order = await _context.Orders.FindAsync(context.Message.OrderId);

        if (order != null)
        {
            order.Status = OrderStatus.Fail;
            order.ErrorMessage = context.Message.Message;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order status changed : {status}  {id} ",
                order.Status, 
                context.Message.OrderId);
        }
        else
        {
            _logger.LogError("Order not found {id}", context.Message.OrderId);
        }
    }
}

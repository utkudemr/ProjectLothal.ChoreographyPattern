
namespace Order.Shared;

public class QueueConst
{
    public const string StockOrderCreatedEventQueueName = "stock-order-created-queue";
    public const string StockReservedEventQueueName = "stock-reserved-queue";

    public const string OrderStockNotReservedEventQueueName = "order-stock-not-reserved-queue";
}

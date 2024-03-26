
using MassTransit;
using MediatR;
using Order.Domain.Entities;
using Order.Domain.Enums;
using Order.Persistance.Contexts;
using Order.Shared;

namespace Order.Application.Features.Order.Commands.Create;

public class CreateOrderCommand : IRequest<bool>
{
    public OrderCreateDto Order { get; set; }
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly OrderDbContext _context;

        private readonly IPublishEndpoint _publishEndpoint;

        public CreateOrderCommandHandler(OrderDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }


        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newOrder = TransformCreateOrder(request);

                await _context.AddAsync(newOrder, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                var orderCreatedEvent = CreateOrderEvent(request, newOrder);

                await _publishEndpoint.Publish(orderCreatedEvent, cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static OrderCreatedEvent CreateOrderEvent(CreateOrderCommand request, OrderHeader newOrder)
        {
            var orderCreatedEvent = new OrderCreatedEvent()
            {
                BuyerId = request.Order.CustomerId,
                OrderId = newOrder.Id,
                Payment = new PaymentMessage
                {
                    CardName = request.Order.OrderPayment.CardName,
                    CardNumber = request.Order.OrderPayment.CardNumber,
                    TotalPrice = request.Order.OrderItems.Sum(x => x.Price * x.Count)
                },
            };

            request.Order.OrderItems.ForEach(item =>
            {
                orderCreatedEvent.orderItems.Add(new OrderItemMessage { Count = item.Count, ProductId = item.ProductId });
            });
            return orderCreatedEvent;
        }

        private static OrderHeader TransformCreateOrder(CreateOrderCommand request)
        {
            var newOrder = new OrderHeader
            {
                BuyerId = request.Order.CustomerId,
                Status = OrderStatus.Suspend,
                Line = request.Order.OrderAddress.Line,
                Province = request.Order.OrderAddress.Province,
                District = request.Order.OrderAddress.District,
                CreatedDate = DateTime.Now,
                ErrorMessage = string.Empty
            };

            newOrder.Items.ToList().ForEach(item =>
            {
                newOrder.Items.Add(new OrderItem { Price = item.Price, ProductId = item.ProductId, Count = item.Count });
            });
            return newOrder;
        }
    }
}

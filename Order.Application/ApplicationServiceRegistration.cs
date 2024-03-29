using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Order.Application.Consumers;
using Order.Shared;

namespace Order.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddMassTransit(x =>
            {
                x.AddConsumer<PaymentFailedConsumer>();
                x.AddConsumer<StockNotReservedEventConsumer>();
                x.AddConsumer<PaymentCompletedEventConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", c => { });

                    cfg.ReceiveEndpoint(QueueConst.OrderStockNotReservedEventQueueName, e =>
                    {
                        e.ConfigureConsumer<StockNotReservedEventConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(QueueConst.OrderPaymentFailedEventQueueName, e =>
                    {
                        e.ConfigureConsumer<PaymentFailedConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(QueueConst.OrderPaymentCompletedEventQueueName, e =>
                    {
                        e.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
                    });

                });

            });

            return services;
        }
    }
}

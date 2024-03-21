
using Order.Application.Services;
using Order.Persistance.Contexts;

namespace Order.Persistance.Repositories
{
    public class OrderRepository: IOrderRepository
    {
        public OrderRepository(OrderDbContext context)
        {
        }
    }
}

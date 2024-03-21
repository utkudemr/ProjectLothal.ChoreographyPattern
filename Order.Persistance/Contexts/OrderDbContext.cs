using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;

namespace Order.Persistance.Contexts;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<OrderHeader> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}

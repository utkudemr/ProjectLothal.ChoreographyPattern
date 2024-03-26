using Microsoft.EntityFrameworkCore;
using Stock.API.Models.Entities;
namespace Stock.API.Contexts
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {
        }

        public DbSet<OrderStock> Stocks { get; set; }
    }
}

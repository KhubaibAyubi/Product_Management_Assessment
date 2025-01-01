using Microsoft.EntityFrameworkCore;
using Product_Management_Assessment.DTO;

namespace Product_Management_Assessment
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
    }
}

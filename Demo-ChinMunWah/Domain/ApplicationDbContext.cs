using Microsoft.EntityFrameworkCore;

namespace Demo_ChinMunWah.Domain
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }    

        public DbSet<Customer> Customers { get; set; }
    }
}

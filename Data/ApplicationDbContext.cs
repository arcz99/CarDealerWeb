using CarDealerWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealerWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Car> Cars { get; set; }
    }
}

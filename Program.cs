using CarDealerWeb.Data;
using CarDealerWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealerWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (!context.Cars.Any())
                {
                    context.Cars.AddRange(
                            new Car { Brand = "Toyota", Model = "Corolla", Year = 2015, Price = 32000, Description = "Ekonomiczna i niezawodna" },
                            new Car { Brand = "BMW", Model = "3 Series", Year = 2018, Price = 75000, Description = "Sportowa limuzyna" },
                            new Car { Brand = "Ford", Model = "Focus", Year = 2016, Price = 28000, Description = "Dobre auto miejskie" }
                        );
                    context.SaveChanges();
                }
            }

            app.Run();
        }
    }
}

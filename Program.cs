using CarDealerWeb.Data;
using CarDealerWeb.Models;
using Microsoft.AspNetCore.Identity;
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

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login"; // œcie¿ka do strony logowania
            });
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

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

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

                Task.Run(async () =>
                {
                    var user = await userManager.FindByEmailAsync("admin@admin.com");
                    if (user == null)
                    {
                        var newUser = new IdentityUser
                        {
                            UserName = "admin@admin.com",
                            Email = "admin@admin.com",
                            EmailConfirmed = true
                        };
                        await userManager.CreateAsync(newUser, "Admin123!");
                    }
                }).GetAwaiter().GetResult();
            }


            app.Run();
        }
    }
}

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using CarDealerWeb.Data;
using CarDealerWeb.Models;

namespace CarDealerWeb.Pages.Admin
{
    [Authorize]
    public class CarsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CarsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Car> Cars { get; set; } = new();

        public void OnGet()
        {
            Cars = _context.Cars.ToList();
        }
    }
}
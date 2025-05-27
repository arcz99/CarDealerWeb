using CarDealerWeb.Data;
using CarDealerWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CarDealerWeb.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Car Car { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Car = await _context.Cars
                .Include(c => c.Images)
                .FirstOrDefaultAsync(c => c.id == id);

            if (Car == null)
                return NotFound();

            // RÊCZNE SORTOWANIE ZDJÊÆ PO ImageOrder
            Car.Images = Car.Images.OrderBy(i => i.ImageOrder).ToList();

            return Page();
        }
    }
}

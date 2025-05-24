using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarDealerWeb.Data;
using CarDealerWeb.Models;

namespace CarDealerWeb.Pages.Admin
{
    public class DeleteCarModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteCarModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Car Car { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Car = await _context.Cars.FindAsync(id);
            if (Car == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var car = await _context.Cars.FindAsync(Car.id);
            if (car == null)
                return NotFound();

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToPage("Cars");
        }
    }
}
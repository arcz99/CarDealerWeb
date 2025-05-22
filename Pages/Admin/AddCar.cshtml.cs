using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarDealerWeb.Data;
using CarDealerWeb.Models;
using System.Threading.Tasks;

namespace CarDealerWeb.Pages.Admin
{
    public class AddCarModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddCarModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Car Car { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Cars.Add(Car);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Index");
        }
    }
}
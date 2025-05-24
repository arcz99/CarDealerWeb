using CarDealerWeb.Data;
using CarDealerWeb.Migrations;
using CarDealerWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;


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

            Car = await _context.Cars.FindAsync(id);

            if (Car == null)
                return NotFound();
            Car = _context.Cars.Include(c => c.Images).FirstOrDefault(c => c.id == id);
            return Page();
        }
    }
}

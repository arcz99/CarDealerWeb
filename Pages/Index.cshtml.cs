using CarDealerWeb.Data;
using CarDealerWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarDealerWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Car> Cars { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Cars.Include(c => c.Images).AsQueryable();

            // WYSZUKIWARKA
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(c =>
                    c.Brand.Contains(SearchTerm) ||
                    c.Model.Contains(SearchTerm) ||
                    c.Description.Contains(SearchTerm));
            }

            // SORTOWANIE
            switch (SortOrder)
            {
                case "price_asc":
                    query = query.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(c => c.Price);
                    break;
                case "year_asc":
                    query = query.OrderBy(c => c.Year);
                    break;
                case "year_desc":
                    query = query.OrderByDescending(c => c.Year);
                    break;
                default:
                    query = query.OrderByDescending(c => c.id);
                    break;
            }

            Cars = await query.ToListAsync();
        }
    }
}
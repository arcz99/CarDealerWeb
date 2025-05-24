using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarDealerWeb.Data;
using CarDealerWeb.Models;

namespace CarDealerWeb.Pages.Admin
{
    public class EditCarModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditCarModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Car Car { get; set; } = default!;

        [BindProperty]
        public List<IFormFile>? Uploads { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Car = await _context.Cars.FindAsync(id);
            if (Car == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var carToUpdate = await _context.Cars.FindAsync(Car.id);
            if (carToUpdate == null)
                return NotFound();

            // Aktualizacja pól
            carToUpdate.Brand = Car.Brand;
            carToUpdate.Model = Car.Model;
            carToUpdate.Year = Car.Year;
            carToUpdate.Price = Car.Price;
            carToUpdate.Description = Car.Description;

            // Obs³uga zdjêcia
            if (Uploads != null && Uploads.Count > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                foreach (var file in Uploads.Take(10))
                {
                    var fileName = Path.GetFileName(Guid.NewGuid() + "_" + file.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var carImage = new CarImage
                    {
                        FileName = fileName,
                        IsMain = false,
                        Car = Car
                    };

                    _context.CarImages.Add(carImage);
                }

                // Mo¿esz wybraæ pierwsze jako domyœlne g³ówne
                var firstImage = Car.Images.FirstOrDefault();
                if (firstImage != null)
                    firstImage.IsMain = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Cars");
        }
    }
}

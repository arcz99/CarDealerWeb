using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarDealerWeb.Data;
using CarDealerWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CarDealerWeb.Pages.Admin
{
    [Authorize]
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
        [BindProperty]
        public List<IFormFile>? Uploads { get; set; }

        [BindProperty]
        public int MainImageIndex { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Najpierw dodaj auto, ¿eby mia³o ID
            _context.Cars.Add(Car);
            await _context.SaveChangesAsync();

            if (Uploads != null && Uploads.Any())
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                int imageIndex = 0;
                foreach (var file in Uploads.Take(10))
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var image = new CarImage
                    {
                        FileName = fileName,
                        IsMain = (imageIndex == MainImageIndex),
                        CarId = Car.id
                    };

                    _context.CarImages.Add(image);
                    imageIndex++;
                }

                await _context.SaveChangesAsync(); // dopiero teraz zapis zdjêæ
            }

            return RedirectToPage("/Admin/Cars");
        }
    }
}
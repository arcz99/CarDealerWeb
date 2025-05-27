using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        [BindProperty]
        public string ImageOrder { get; set; } = string.Empty;

        [BindProperty]
        public int MainImageId { get; set; }

        [BindProperty]
        public List<int> DeleteImageIds { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Car = await _context.Cars
                .Include(c => c.Images.OrderBy(i => i.ImageOrder))
                .FirstOrDefaultAsync(c => c.id == id);

            if (Car == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("ImageOrder");
            if (!ModelState.IsValid)
                return Page();

            var carToUpdate = await _context.Cars
                .Include(c => c.Images)
                .FirstOrDefaultAsync(c => c.id == Car.id);

            if (carToUpdate == null)
                return NotFound();

            // Aktualizacja podstawowych pól
            carToUpdate.Brand = Car.Brand;
            carToUpdate.Model = Car.Model;
            carToUpdate.Year = Car.Year;
            carToUpdate.Price = Car.Price;
            carToUpdate.Description = Car.Description;

            // Usuñ zaznaczone zdjêcia
            if (DeleteImageIds.Any())
            {
                var imagesToDelete = carToUpdate.Images.Where(i => DeleteImageIds.Contains(i.Id)).ToList();
                foreach (var img in imagesToDelete)
                {
                    var fullPath = Path.Combine("wwwroot/uploads", img.FileName);
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);

                    _context.CarImages.Remove(img);
                }
            }

            // Dodaj nowe zdjêcia
            if (Uploads != null && Uploads.Count > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                int nextOrder = carToUpdate.Images.Any() ? carToUpdate.Images.Max(i => i.ImageOrder) + 1 : 0;

                foreach (var file in Uploads)
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
                        IsMain = false,
                        Car = carToUpdate, // POWI¥ZANIE przez obiekt
                        ImageOrder = nextOrder++
                    };

                    _context.CarImages.Add(image);
                }

                // Najpierw zapisujemy zmiany, ¿eby zdjêcia mia³y przydzielone ID
                await _context.SaveChangesAsync();

                // Odœwie¿ listê zdjêæ samochodu
                await _context.Entry(carToUpdate).Collection(c => c.Images).LoadAsync();
            }

            // Aktualizuj kolejnoœæ zdjêæ (ImageOrder)
            if (!string.IsNullOrEmpty(ImageOrder))
            {
                var ids = ImageOrder.Split(',').Select(int.Parse).ToList();
                for (int i = 0; i < ids.Count; i++)
                {
                    var img = carToUpdate.Images.FirstOrDefault(x => x.Id == ids[i]);
                    if (img != null)
                        img.ImageOrder = i;
                }
            }

            // Ustawienie g³ównego zdjêcia
            foreach (var img in carToUpdate.Images)
                img.IsMain = (img.Id == MainImageId);

            // Jeœli ¿adne zdjêcie nie jest g³ówne, ustaw pierwsze jako g³ówne
            if (!carToUpdate.Images.Any(i => i.IsMain) && carToUpdate.Images.Any())
            {
                var first = carToUpdate.Images.OrderBy(i => i.ImageOrder).FirstOrDefault();
                if (first != null)
                    first.IsMain = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Cars");
        }
    }
}
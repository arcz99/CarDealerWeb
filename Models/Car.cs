using System.ComponentModel.DataAnnotations;

namespace CarDealerWeb.Models
{
    public class Car
    {
        public int id { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        public int Year { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string? ImageFileName { get; set; }
        public List<CarImage> Images { get; set; } = new();

    }
}

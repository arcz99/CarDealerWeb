using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealerWeb.Models
{
    public class CarImage
    {
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; } = default!;

        public bool IsMain { get; set; } = false;

        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public Car Car { get; set; } = default!;
    }
}
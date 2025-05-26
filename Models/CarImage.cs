using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealerWeb.Models
{
    public class CarImage
    {
        [Key]
        public int Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public bool IsMain { get; set; }

        public int ImageOrder { get; set; } = 0;

        [ForeignKey("Car")]
        public int CarId { get; set; }

        public Car Car { get; set; } = default!;
    }
}
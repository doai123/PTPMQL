using System.ComponentModel.DataAnnotations;

namespace mvc.Models
{
    public class Diem
    {
        [Required]
        [Range(0, 10, ErrorMessage = "DiemA phải từ 0 đến 10")]
        public double DiemA { get; set; }

        [Required]
        [Range(0, 10, ErrorMessage = "DiemB phải từ 0 đến 10")]
        public double DiemB { get; set; }

        [Required]
        [Range(0, 10, ErrorMessage = "DiemC phải từ 0 đến 10")]
        public double DiemC { get; set; }

        public double SumDiem => (DiemA * 0.6) + (DiemB * 0.3) + (DiemC * 0.1);
    }
}

using System.ComponentModel.DataAnnotations;

namespace mvc.Models
{
    public class HoaDon
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }

        [Required]
        [Range(1000, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn 1000")]
        public double DonGia { get; set; }

        public double ThanhTien => SoLuong * DonGia;
    }
}

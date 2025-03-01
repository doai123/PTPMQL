using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    [Table("Persons")] 
    public class Person
    {
        [Key] // Đánh dấu khóa chính
        public int PersonId { get; set; }      

        [Required] // Đảm bảo không null
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;
    }
}

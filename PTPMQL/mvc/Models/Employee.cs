using System.ComponentModel.DataAnnotations;
namespace mvc.Models
{
    public class Employee : Person
    {
        public int EmpId { get; set; }
        public string Department { get; set; } = default!;
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
    }
}
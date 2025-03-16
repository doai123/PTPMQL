using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public class Student : Person
    {
        public string StudentCode { get; set; }
    }
}
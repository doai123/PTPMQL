using System.ComponentModel.DataAnnotations;

namespace mvc.Models.Entities
{
    public class MemberUnit
    {
        [Key]
        public int MemberUnitId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string WebsiteUrl { get; set; }
    }
}

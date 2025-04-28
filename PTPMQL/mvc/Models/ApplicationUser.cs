using Microsoft.AspNetCore.Identity;

namespace mvc.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string? FullName { get; set; }
    }
}
using Microsoft.AspNetCore.Identity;

namespace PI4Daan.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdmin { get; set; } = false; // Standaard is een gebruiker geen admin
    }
}

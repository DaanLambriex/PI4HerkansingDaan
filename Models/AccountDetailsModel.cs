namespace PI4Daan.Models
{
    public class AccountDetailsModel
    {
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool IsAdmin { get; set; }
        public string? ProfilePhotoPath { get; set; } // Corrected to use ProfilePhoto
    }
}

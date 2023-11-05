using System.ComponentModel.DataAnnotations;

namespace Ads.Web.Mvc.Models
{

    public class UserViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        [Required(ErrorMessage = "New email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string NewEmail { get; set; }

        [Required(ErrorMessage = "Verify New email is required.")]
        [Compare("NewEmail", ErrorMessage = "The Verify New email must match the New email.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string VerifyNewEmail { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string? UserImagePath { get; set; }
    }

}

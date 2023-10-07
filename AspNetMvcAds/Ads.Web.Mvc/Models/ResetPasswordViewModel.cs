using System.ComponentModel.DataAnnotations;

namespace Ads.Web.Mvc.Models
{
    public class ResetPasswordViewModel
    {
      
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "New Password area is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }=string.Empty;

        [Required(ErrorMessage = "Confirm New Password area is required.")]
        [Compare("NewPassword", ErrorMessage = "Passwords does not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}

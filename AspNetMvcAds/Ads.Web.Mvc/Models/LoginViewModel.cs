using System.ComponentModel.DataAnnotations;

namespace Ads.Web.Mvc.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required."), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required."), MinLength(2)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}

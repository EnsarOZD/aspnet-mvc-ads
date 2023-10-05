using System.ComponentModel.DataAnnotations;

namespace Ads.Web.Mvc.Models
{
    public class LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(2)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}

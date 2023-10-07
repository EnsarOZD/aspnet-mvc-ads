using System.ComponentModel.DataAnnotations;

namespace Ads.Web.Mvc.Models
{
    public class ForgotPasswordViewModel
    {
        [Required,EmailAddress]
        public string? Email { get; set; } = string.Empty;
    }
}

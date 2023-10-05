using System.ComponentModel.DataAnnotations;

namespace Ads.Web.Mvc.Models
{
    public class RegisterViewModel
    {

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(4), DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string PasswordVerify { get; set; } = string.Empty;



    }
}

using System.ComponentModel.DataAnnotations;

namespace Ads.Web.Mvc.Models
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Geçerli bir e-posta adresi giriniz."), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required."), MinLength(4), DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage ="Required."), DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords does not match!")]
        public string PasswordVerify { get; set; } = string.Empty;

    }
}

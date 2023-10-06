using System.ComponentModel.DataAnnotations;

namespace Ads.Web.Mvc.Models
{
    public class ResetPasswordViewModel
    {
        //[Required, EmailAddress]
        //public string Email { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yeni şifre alanı zorunludur.")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }=string.Empty;

        [Required(ErrorMessage = "Yeni şifreyi doğrulama alanı zorunludur.")]
        [Compare("NewPassword", ErrorMessage = "Yeni şifre ile doğrulama şifresi uyuşmuyor.")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre Doğrulama")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}

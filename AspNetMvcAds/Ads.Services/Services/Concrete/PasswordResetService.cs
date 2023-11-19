namespace Ads.Services.Services.Concrete
{
    public class PasswordResetService
    {
        private readonly EmailService _emailService;

        public PasswordResetService(EmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendPasswordResetEmail(string userEmail, string resetToken)
        {
            string subject = "PasswordReset";
            //string message = $"Şifrenizi sıfırlamak için  bağlantıya tıklayın: <a href='https://localhost:44331/Auth/ResetPassword/{resetToken}'>Şifre Sıfırlama Bağlantısı</a>";
            //string message = $"Click this link for reset your password safely!: https://localhost:44331/Auth/ResetPassword/{resetToken}";
            string resetLink = $"https://siliconmarket.com.tr/Auth/ResetPassword/{resetToken}";
            string message = $"To reset your password securely, please click the following link: <a href='{resetLink}'>Reset Password</a>";


            await _emailService.SendEmailAsync(userEmail, subject, message);
        }

    }
}

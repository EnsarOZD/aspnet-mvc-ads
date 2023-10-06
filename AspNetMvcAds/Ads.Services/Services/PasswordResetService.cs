namespace Ads.Services.Services
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
            string subject = "Şifre Sıfırlama";
            //string message = $"Şifrenizi sıfırlamak için  bağlantıya tıklayın: <a href='https://localhost:44331/Auth/ResetPassword/{resetToken}'>Şifre Sıfırlama Bağlantısı</a>";
            string message = $"Şifrenizi sıfırlamak için  bağlantıya tıklayın: https://localhost:44331/Auth/ResetPassword/{resetToken}";

            await _emailService.SendEmailAsync(userEmail, subject, message);
        }

    }
}

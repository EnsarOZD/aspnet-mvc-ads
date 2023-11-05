using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Services.Services.Concrete
{
    public class EmailService
    {
        const string senderEmail = "siliconmademarket@gmail.com";
        const string senderName = "SiliconMadeMarket";
        const string senderPassword = "gvdoxkduxywlvawq";
        //gvdo xkdu xywl vawq

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            MailMessage mailMessage = new()
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            SmtpClient smtpClient = new("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true
            };

            await smtpClient.SendMailAsync(mailMessage);

        }
    }

}

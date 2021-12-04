using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<EmailSettings> _appSettings;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<User> _userManager;

        public EmailService(IOptions<EmailSettings> appSettings
            , IWebHostEnvironment hostEnvironment
            , UserManager<User> userManager)
        {
            _appSettings = appSettings;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

        public async Task SendEmail(string message, string recipient, string subject)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                // email contents
                mailMessage.From = new MailAddress(_appSettings.Value.Email);
                mailMessage.To.Add(new MailAddress(recipient));
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = message;

                // smtp credentials
                smtp.Port = _appSettings.Value.Port;
                smtp.Host = _appSettings.Value.Host;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_appSettings.Value.Email, _appSettings.Value.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                // send email
                smtp.Send(mailMessage);

            }
            catch (Exception) { }

        }

        public async Task SendConfirmationEmail(string recipient, string confirmationLink)
        {
            try
            {
                string filePath = string.Format("{0}\\{1}", _hostEnvironment.ContentRootPath, "Templates\\EmailConfirmation.html");

                StreamReader str = new StreamReader(filePath);
                string message = str.ReadToEnd();
                str.Close();

                message = message.Replace("[confirmationlink]", confirmationLink);

                await SendEmail(message, recipient, "Account Confirmation");

            }
            catch (Exception) { }

        }

        public async Task SendResetLinkEmail(string recipient, string resetLink)
        {
            try
            {
                string filePath = string.Format("{0}\\{1}", _hostEnvironment.ContentRootPath, "Templates\\ResetLink.html");

                StreamReader str = new StreamReader(filePath);
                string message = str.ReadToEnd();
                str.Close();

                message = message.Replace("[resetlink]", resetLink);

                await SendEmail(message, recipient, "Password Reset Link");

            }
            catch (Exception) { }

        }

        public async Task AccountVerified(string userId)
        { 
            var _userRepo = _userManager.Users.Where(x => x.Id == userId).FirstOrDefault();
            _userRepo.EmailConfirmed = true;

            var result = await _userManager.UpdateAsync(_userRepo);
        }
    }
}

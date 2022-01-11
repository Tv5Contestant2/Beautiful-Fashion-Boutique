using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        public async Task SendEmail(string message, string recipient, string subject, string sender = "")
        {
            await Task.Delay(0);
            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                // email contents
                mailMessage.From = new MailAddress(sender == "" ? _appSettings.Value.Email : sender);
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

        public async Task SendMessage(MessageViewModel viewModel)
        {
            try
            {
                string filePath = string.Format("{0}\\{1}", _hostEnvironment.ContentRootPath, "Templates\\Inquiry.html");

                StreamReader str = new StreamReader(filePath);
                string message = str.ReadToEnd();
                str.Close();

                message = message.Replace("[contactnumber]", viewModel.ContactNumber);
                message = message.Replace("[emailaddress]", viewModel.EmailAddress);
                message = message.Replace("[sendername]", viewModel.SenderName);
                message = message.Replace("[message]", viewModel.MessageBody);

                await SendEmail(message, _appSettings.Value.Email, "Inquiry", viewModel.EmailAddress);
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

        public async Task SendReceipt(string recipient, Orders orders)
        {
            try
            {
                string filePath = string.Format("{0}\\{1}", _hostEnvironment.ContentRootPath, "Templates\\Receipt.html");

                StreamReader str = new StreamReader(filePath);
                string message = str.ReadToEnd();
                str.Close();

                message = message.Replace("[reference]", orders.TransactionId.ToString());

                var subject = "[" + orders.TransactionId + "] receipt for your order on " + orders.OrderDate.ToShortDateString();
                await SendEmail(message, recipient, subject);
            }
            catch (Exception) { }
        }

        public async Task SendOrderShipped(string recipient, OrderViewModel orders)
        {
            try
            {
                string filePath = string.Format("{0}\\{1}", _hostEnvironment.ContentRootPath, "Templates\\OrderShipped.html");

                StreamReader str = new StreamReader(filePath);
                string message = str.ReadToEnd();
                str.Close();

                message = message.Replace("[reference]", orders.TransactionId.ToString());
                message = message.Replace("[expecteddate]", orders.ExpectedDeliveryDate.ToShortDateString());

                var subject = "[" + orders.TransactionId + "] has been shipped";
                await SendEmail(message, recipient, subject);
            }
            catch (Exception) { }
        }

        public async Task SendOrderReceived(string recipient, OrderViewModel orders)
        {
            try
            {
                string filePath = string.Format("{0}\\{1}", _hostEnvironment.ContentRootPath, "Templates\\OrderReceived.html");

                StreamReader str = new StreamReader(filePath);
                string message = str.ReadToEnd();
                str.Close();

                message = message.Replace("[reference]", orders.TransactionId.ToString());

                var subject = "[" + orders.TransactionId + "] completed";
                await SendEmail(message, recipient, subject);
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

        public async Task SendBlockEmail(string recipient)
        {
            try
            {
                string filePath = string.Format("{0}\\{1}", _hostEnvironment.ContentRootPath, "Templates\\BlockEmail.html");

                StreamReader str = new StreamReader(filePath);
                string message = str.ReadToEnd();
                str.Close();

                await SendEmail(message, recipient, "Your account has been blocked");
            }
            catch (Exception) { }
        }

        public async Task SendTemporaryDeleteEmail(string recipient)
        {
            try
            {
                string filePath = string.Format("{0}\\{1}", _hostEnvironment.ContentRootPath, "Templates\\TemporaryDeleteEmail.html");

                StreamReader str = new StreamReader(filePath);
                string message = str.ReadToEnd();
                str.Close();

                await SendEmail(message, recipient, "Your account has been tagged for deletion");
            }
            catch (Exception) { }
        }

        public async Task SendDeleteEmail(string recipient)
        {
            try
            {
                string filePath = string.Format("{0}\\{1}", _hostEnvironment.ContentRootPath, "Templates\\DeleteEmail.html");

                StreamReader str = new StreamReader(filePath);
                string message = str.ReadToEnd();
                str.Close();

                await SendEmail(message, recipient, "Your account has been deleted");
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
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IEmailService
    {
        public Task SendConfirmationEmail(string recipient, string confirmationLink);
        public Task SendResetLinkEmail(string recipient, string resetLink);
        public Task AccountVerified(string userId);
    }
}

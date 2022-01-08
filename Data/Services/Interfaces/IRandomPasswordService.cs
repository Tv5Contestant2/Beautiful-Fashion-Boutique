using Microsoft.AspNetCore.Identity;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IRandomPasswordService
    {
        public string GenerateRandomPassword(PasswordOptions opts = null);
    }
}
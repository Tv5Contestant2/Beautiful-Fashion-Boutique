using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public UserService(UserManager<User> userManager
            , IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task UpdateLastLoggedIn(string userId)
        {
            var _userRepo = await _userManager.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            _userRepo.LastLoggedIn = DateTime.Now;
            _userRepo.IsArchived = false;

            await _userManager.UpdateAsync(_userRepo);
        }

        public async Task ArchiveUsers()
        {
            var _userRepo = await _userManager.Users.Where(x => x.LastLoggedIn != null).ToListAsync();
            var _forArchive = _userRepo.Where(x => (DateTime.Today - x.LastLoggedIn).Days >= 90).ToList();

            if (_forArchive.Count > 0)
            {
                foreach (var user in _forArchive) { 
                    user.IsArchived = true;
                    await _userManager.UpdateAsync(user);
                }
            }
        }

        public async Task DeleteCustomers()
        {
            var _userRepo = await _userManager.Users.Where(x => x.DeletedOn != null && !x.IsDeleted.Value).ToListAsync();

            // All deleted accounts with deleted on > 8 days means they also receive an email that their account has been deleted
            var _forDeletion = _userRepo.Where(x => (x.DeletedOn - DateTime.Today).Value.Days == 8).ToList();

            if (_forDeletion.Count > 0)
            {
                foreach (var user in _forDeletion)
                {
                    user.IsDeleted = true;
                    await _userManager.UpdateAsync(user);

                    await _emailService.SendDeleteEmail(user.Email);
                }
            }
        }
    }
}

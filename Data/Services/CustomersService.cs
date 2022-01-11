using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly AppDBContext _context;
        private readonly ICommonServices _commonServices;
        private readonly UserManager<User> _userManager;

        public CustomersService(AppDBContext context
            , ICommonServices commonServices
            , UserManager<User> userManager)
        {
            _context = context;
            _commonServices = commonServices;
            _userManager = userManager;
        }

        public CustomerViewModel InitializeCustomer()
        {
            var _result = new CustomerViewModel()
            {
                Genders = _context.Genders.ToList(),
                Birthday = DateTime.Now
            };

            return _result;
        }

        public void InitializeCustomer(CustomerViewModel model)
        {
            model.Genders = _context.Genders.ToList();
        }

        public async Task<IEnumerable<Gender>> GetGenders() => await _context.Genders.ToListAsync();

        public async Task<(bool, IEnumerable<IdentityError>)> CreateCustomer(CustomerViewModel model)
        {
            // Copy data from RegisterViewModel to IdentityUser
            var user = new User
            {
                AddressBaranggay = model.AddressBaranggay,
                AddressBlock = model.AddressBlock,
                AddressCity = model.AddressCity,
                AddressLot = model.AddressLot,
                Birthday = model.Birthday,
                ContactNumber = model.ContactNumber,
                DateCreated = model.DateCreated,
                Email = model.Email,
                FirstName = model.FirstName,
                GenderId = model.GenderId,
                Image = model.Image,
                IsCustomer = true,
                LastName = model.LastName,
                UserName = model.Email,
            };

            // Store user data in AspNetUsers database table
            var result = await _userManager.CreateAsync(user, model.Password);

            return (result.Succeeded, result.Errors);
        }

        public async Task UpdateCustomer(CustomerViewModel model)
        {
            var _userRepo = await _userManager.Users.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            // Copy data from RegisterViewModel to IdentityUser
            _userRepo.AddressBaranggay = model.AddressBaranggay;
            _userRepo.AddressBlock = model.AddressBlock;
            _userRepo.AddressCity = model.AddressCity;
            _userRepo.AddressLot = model.AddressLot;
            _userRepo.Birthday = model.Birthday;
            _userRepo.ContactNumber = model.ContactNumber;
            _userRepo.DateCreated = model.DateCreated;
            //_userRepo.Email = model.Email;
            _userRepo.FirstName = model.FirstName;
            _userRepo.GenderId = model.GenderId;
            _userRepo.Image = model.Image;
            _userRepo.LastName = model.LastName;
            //_userRepo.UserName = model.Email.ToLower().Trim();

            if (!string.IsNullOrEmpty(model.Password))
            {
                _userRepo.PasswordHash = _userManager.PasswordHasher.HashPassword(_userRepo, model.Password);
            }

            // Update user data in AspNetUsers database table
            var result = await _userManager.UpdateAsync(_userRepo);
        }

        public async Task UpdateCustomer(User model)
        {
            // Update user data in AspNetUsers database table
            var result = await _userManager.UpdateAsync(model);
        }

        public async Task<User> GetCustomerById(string id)
        {
            var _customer = (await _userManager.Users.Where(x => x.Id == id).ToListAsync()).FirstOrDefault();
            return _customer;
        }

        public async Task DeleteCustomer(string id)
        {
            var _customer = await GetCustomerById(id);
            _customer.DeletedOn = DateTime.Now;

            await _userManager.UpdateAsync(_customer);
        }

        public async Task<IEnumerable<User>> GetAllCustomers()
        {
            var _customers = await _userManager.Users.Where(x => x.IsCustomer == true 
                && (x.IsBlock == false || x.IsBlock == null)
                && (x.IsArchived == false || x.IsArchived == null)
                && (x.DeletedOn == null)).ToListAsync();
            return _customers;
        }

        public async Task<IEnumerable<User>> GetAllArchivedCustomers()
        {
            var _customers = (await _userManager.Users.Where(x => x.IsCustomer == true && x.IsArchived == true).ToListAsync());
            return _customers;
        }

        public async Task<IEnumerable<User>> GetAllBlockCustomers()
        {
            var _customers = (await _userManager.Users.Where(x => x.IsCustomer == true && x.IsBlock == true).ToListAsync());
            return _customers;
        }

        public async Task<IEnumerable<User>> GetAllDeletedCustomers()
        {
            var _customers = (await _userManager.Users.Where(x => x.IsCustomer == true && x.DeletedOn != null).ToListAsync());
            return _customers;
        }

        public async Task<IEnumerable<User>> GetAllCustomersByGender(int genderId)
        {
            var _customers = (await _userManager.Users.Where(x => x.IsCustomer == true && x.IsBlock == false && x.GenderId == genderId).ToListAsync());
            return _customers;
        }
    }
}
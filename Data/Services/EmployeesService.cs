using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;

        public EmployeesService(AppDBContext context
            , UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<(bool, IEnumerable<IdentityError>)> CreateEmployee(EmployeeViewModel model)
        {
            // Copy data from RegisterViewModel to IdentityUser
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                IsEmployee = true,
                Birthday = model.Birthday,
                AddressBlock = model.AddressBlock,
                AddressLot = model.AddressLot,
                AddressCity = model.AddressCity,
                AddressBaranggay = model.AddressBaranggay,
                ContactNumber = model.ContactNumber,
                DateCreated = model.DateCreated,
                Image = model.Image,
            };

            // Store user data in AspNetUsers database table
            var result = await _userManager.CreateAsync(user, model.Password);

            return (result.Succeeded, result.Errors);
        }

        public async Task UpdateEmployee(EmployeeViewModel model)
        {
            var _userRepo = await _userManager.Users.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            // Copy data from RegisterViewModel to IdentityUser
            _userRepo.FirstName = model.FirstName;
            _userRepo.LastName = model.LastName;
            _userRepo.Birthday = model.Birthday;
            _userRepo.AddressBlock = model.AddressBlock;
            _userRepo.AddressLot = model.AddressLot;
            _userRepo.AddressCity = model.AddressCity;
            _userRepo.AddressBaranggay = model.AddressBaranggay;
            _userRepo.ContactNumber = model.ContactNumber;
            _userRepo.DateCreated = model.DateCreated;
            _userRepo.Image = model.Image;
            _userRepo.Email = model.Email;
            _userRepo.UserName = model.Email.ToLower().Trim();

            if (!string.IsNullOrEmpty(model.Password))
            {
                _userRepo.PasswordHash = _userManager.PasswordHasher.HashPassword(_userRepo, model.Password);
            }

            // Update user data in AspNetUsers database table
            var result = await _userManager.UpdateAsync(_userRepo);
        }

        public async Task<Employees> UpdateEmployee(long id, Employees employee)
        {
            _context.Update(id);
            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task DeleteEmployee(string id)
        {
            var _employee = await GetEmployeeById(id);

            var _result = await _userManager.DeleteAsync(_employee);
        }

        public async Task<User> GetEmployeeById(string id)
        {
            var _employee = (await _userManager.Users.Where(x => x.Id == id).ToListAsync()).FirstOrDefault();
            return _employee;
        }

        public async Task<IEnumerable<User>> GetAllEmployees()
        {
            var _employees = await _userManager.Users.Where(x => x.IsEmployee == true).ToListAsync();
            return _employees;
        }

        //public async Task<Employees> GetEmployeeById(long id)
        //{
        //    var result = await _context.Employees.ToListAsync();
        //    return result.Find(x => x.EmployeeID == id);
        //}
    }
}
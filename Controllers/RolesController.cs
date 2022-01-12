using ECommerce1.Data.Services.Interfaces;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    public class RolesController : Controller
    {
        private readonly IEmployeesService _employeesService;

        public RolesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var data = await _employeesService.GetAllEmployees();

            var viewModel = new HomeUserViewModel
            {
                ItemPerPage = 10,
                Users = data,
                CurrentPage = page
            };

            return View(viewModel);
        }
    }
}
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly IAdministratorService _service;
        public AdministratorController(IAdministratorService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UnderConstruction()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            var result = _service.GetAboutUs();
            return View(result);
        }

        public IActionResult ContactUs()
        {
            var result = _service.GetContactUs();
            return View(result);
        }

        public IActionResult CreateAboutUs(About about)
        {
            _service.CreateAboutUs(about);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreateContactUs(SocMed socMed)
        {
            _service.CreateContactUs(socMed);
            return RedirectToAction(nameof(Index));
        }
    }
}

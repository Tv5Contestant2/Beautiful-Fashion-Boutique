using ECommerce1.Data.Enums;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ICartService _cartService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public HomeController(ICartService cartService
            , UserManager<User> userManager
            , SignInManager<User> signInManager)
        {
            _cartService = cartService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index", "StoreFront");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> SignIn()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cart = await _cartService.GetCartItems(userId);
            ViewBag.Cart = cart;

            var _model = new HomeUserViewModel();
            return View(_model);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([Bind("SignInEmail,SignInPassword")] HomeUserViewModel model)
        {
            model.isLogInError = false;
            model.isSignUpError = false;
            var _removeValidation = "FirstName,LastName,Password,Email".Split(",");
            var userId = _userManager.GetUserId(HttpContext.User);
            foreach (var _item in _removeValidation)
            {
                ModelState[_item].Errors.Clear();
                ModelState.Remove(_item);
            }

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.SignInEmail, model.SignInPassword, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "StoreFront");
                }
                else
                {
                    var cart = await _cartService.GetCartItems(userId);
                    ViewBag.Cart = cart;
                }

                model.isLogInError = true;
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            else
            {
                model.isLogInError = true;
                var cart = await _cartService.GetCartItems(userId);
                ViewBag.Cart = cart;
            }

            return View(model);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "StoreFront");
        }

        public async Task<IActionResult> SignUp()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cart = await _cartService.GetCartItems(userId);
            ViewBag.Cart = cart;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([Bind("FirstName,LastName,Email,Password,ConfirmPassword")] HomeUserViewModel model)
        {
            model.isLogInError = false;
            model.isSignUpError = false;
            var _removeValidation = "SignInEmail,SignInPassword".Split(",");
            var userId = _userManager.GetUserId(HttpContext.User);
            foreach (var _item in _removeValidation)
            {
                ModelState[_item].Errors.Clear();
                ModelState.Remove(_item);
            }

            if (ModelState.IsValid)
            {
                // Copy data from RegisterViewModel to IdentityUser
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email,
                    IsCustomer = true,
                    GenderId = (int)GenderEnum.Men,
                    LastLoggedIn = DateTime.Now,
                    Birthday = DateTime.Parse("01/01/2000")
                };

                // Store user data in AspNetUsers database table
                var result = await _userManager.CreateAsync(user, model.Password);

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                if (!ModelState.IsValid)
                {
                    var cart = await _cartService.GetCartItems(userId);
                    ViewBag.Cart = cart;
                    model.isSignUpError = true;
                    return View("SignIn", model);
                }
            }
            else
            {
                var cart = await _cartService.GetCartItems(userId);
                ViewBag.Cart = cart;
                model.isSignUpError = true;
            }

            return View("SignIn", model);
        }

        public async Task<IActionResult> UnderConstruction()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cart = await _cartService.GetCartItems(userId);
            ViewBag.Cart = cart;

            return View();
        }
    }
}
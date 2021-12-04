using ECommerce1.Data.Enums;
using ECommerce1.Data.Services;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [Produces("application/json")]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IEmailService _emailService;
        private readonly IOptions<EmailSettings> _emailSettings;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public HomeController(ICartService cartService
            , IEmailService emailService
            , IOptions<EmailSettings> emailSettings
            , UserManager<User> userManager
            , SignInManager<User> signInManager)
        {
            _cartService = cartService;
            _emailService = emailService;
            _emailSettings = emailSettings;
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

        public async Task<IActionResult> RegistrationSuccess()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cart = await _cartService.GetCartItems(userId);
            ViewBag.Cart = cart;

            var _model = new HomeUserViewModel();
            return View(_model);
        }
        
        public async Task<IActionResult> ResetLinkSent()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cart = await _cartService.GetCartItems(userId);
            ViewBag.Cart = cart;

            var _model = new HomeUserViewModel();
            return View(_model);
        }

        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            var cart = await _cartService.GetCartItems("");
            ViewBag.Cart = cart;
            ViewBag.Email = email;
            ViewBag.Token = token;

            var _model = new ResetPasswordViewModel();
            return View(_model);
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
                var user = await _userManager.FindByEmailAsync(model.SignInEmail);

                if (user != null && !user.EmailConfirmed &&
                            (await _userManager.CheckPasswordAsync(user, model.SignInPassword)))
                {
                    var cart = await _cartService.GetCartItems(userId);
                    ViewBag.Cart = cart;

                    model.isLogInError = true;
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View(model);
                }

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
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Home",
    new { userId = user.Id, token = token }, Request.Scheme);

                    // send email confirmation
                    await _emailService.SendConfirmationEmail(model.Email, confirmationLink);
                    return RedirectToAction("RegistrationSuccess", new HomeUserViewModel());
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

        public async Task<IActionResult> ForgotPassword()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cart = await _cartService.GetCartItems(userId);
            ViewBag.Cart = cart;

            var _model = new ForgotPasswordViewModel();
            return View(_model);
        }

        public async Task<IActionResult> SubmitForgotPassword(ForgotPasswordViewModel model)
        {
            var cart = await _cartService.GetCartItems("");
            ViewBag.Cart = cart;

            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);
                // If the user is found AND Email is confirmed
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    // Generate the reset password token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Build the password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Home",
                            new { email = model.Email, token = token }, Request.Scheme);

                    // send email
                    await _emailService.SendResetLinkEmail(model.Email, passwordResetLink);
                    return RedirectToAction("ResetLinkSent", new HomeUserViewModel());
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
                model.isLogInError = true;
                ModelState.AddModelError(string.Empty, "Email does not exist.");
                return View("ForgotPassword", model);
            }

            model.isLogInError = true;
            ModelState.AddModelError(string.Empty, "Email does not exist.");
            return View("ForgotPassword", model);
        }

        public async Task<IActionResult> SubmitResetPassword(ResetPasswordViewModel model)
        {
            var cart = await _cartService.GetCartItems("");
            ViewBag.Cart = cart;

            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // reset the user password
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordSuccess", model);
                    }
                    // Display validation errors. For example, password reset token already
                    // used to change the password or password complexity rules not met
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist
                return View("ResetPasswordSuccess", model);
            }
            // Display validation errors if model state is not valid
            return View("ResetPasswordSuccess", model);
        }
        public async Task<IActionResult> ConfirmEmail(string userId)
        {
            await _emailService.AccountVerified(userId);

            var cart = await _cartService.GetCartItems(userId);
            ViewBag.Cart = cart;

            var model = new HomeUserViewModel();
            return View(model);
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
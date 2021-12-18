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
        private readonly ICommonServices _commonServices;

        public HomeController(ICartService cartService
            , IEmailService emailService
            , IOptions<EmailSettings> emailSettings
            , UserManager<User> userManager
            , SignInManager<User> signInManager
            , ICommonServices commonServices
            )
        {
            _cartService = cartService;
            _emailService = emailService;
            _emailSettings = emailSettings;
            _userManager = userManager;
            _signInManager = signInManager;
            _commonServices = commonServices;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            return RedirectToAction("Home", "StoreFront");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult RegistrationSuccess()
        {
            ViewBag.CartCount = 0;
            ViewBag.WishlistCount = 0;

            return View(new HomeUserViewModel());
        }

        public IActionResult ResetLinkSent()
        {
            ViewBag.CartCount = 0;

            return View(new HomeUserViewModel());
        }

        public IActionResult ResetPassword(string email, string token)
        {
            ViewBag.CartCount = 0;
            ViewBag.WishlistCount = 0;
            ViewBag.Email = email;
            ViewBag.Token = token;

            return View(new ResetPasswordViewModel());
        }

        public IActionResult SignIn()
        {
            ViewBag.CartCount = 0;
            ViewBag.WishlistCount = 0;

            return View(new HomeUserViewModel());
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
                    ViewBag.CartCount = 0;
                    ViewBag.WishlistCount = 0;

                    model.isLogInError = true;
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(
                    model.SignInEmail, model.SignInPassword, model.RememberMe, false);

                if (result.Succeeded)
                {
                    ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
                    ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
                    return RedirectToAction("Home", "StoreFront");
                }
            }
            else
            {
                ViewBag.CartCount = 0;
                ViewBag.WishlistCount = 0;
                model.isLogInError = true;
            }

            return View(model);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Home", "StoreFront");
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
                    Image = _commonServices.NoImage
                };

                // Store user data in AspNetUsers database table
                var result = await _userManager.CreateAsync(user, model.Password);

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Home", new { userId = user.Id, token = token }, Request.Scheme);

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
                    ViewBag.CartCount = 0;
                    ViewBag.WishlistCount = 0;

                    model.isSignUpError = true;
                    return View("SignIn", model);
                }
            }
            else
            {
                ViewBag.CartCount = 0;
                ViewBag.WishlistCount = 0;
                model.isSignUpError = true;
            }

            return View("SignIn", model);
        }

        public IActionResult ForgotPassword()
        {
            ViewBag.CartCount = 0;
            ViewBag.WishlistCount = 0;

            return View(new ForgotPasswordViewModel());
        }

        public async Task<IActionResult> SubmitForgotPassword(ForgotPasswordViewModel model)
        {
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
                ViewBag.CartCount = 0;
                ViewBag.WishlistCount = 0;

                model.isLogInError = true;
                ModelState.AddModelError(string.Empty, "Email does not exist.");
                return View("ForgotPassword", model);
            }

            ViewBag.CartCount = 0;
            ViewBag.WishlistCount = 0;

            model.isLogInError = true;
            ModelState.AddModelError(string.Empty, "Email does not exist.");
            return View("ForgotPassword", model);
        }

        public async Task<IActionResult> SubmitResetPassword(ResetPasswordViewModel model)
        {
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

            ViewBag.CartCount = 0;

            return View(new HomeUserViewModel());
        }

        public async Task<IActionResult> UnderConstruction()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);

            return View();
        }
    }
}
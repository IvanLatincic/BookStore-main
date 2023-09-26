using BookStore.Data.Repositories;
using BookStore.Data.Static;
using BookStore.Data.ViewModels;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace BookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginVM loginVM = new LoginVM
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM, string returnUrl)
        {
            loginVM.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
                if (user != null /*&& !user.EmailConfirmed*/ && (await _userManager.CheckPasswordAsync(user, loginVM.Password)))
                {
                    //ModelState.AddModelError(string.Empty, "Potvrda vaše e-mail adrese nije još izvršena.");

                    var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                    if (passwordCheck)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                        if (result.Succeeded)
                        {
                            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }

                            else
                            {
                                return RedirectToAction("Index", "Product");
                            }
                        }
                    }
                    TempData["Error"] = "Pogrešan unos. Molimo pokušajte ponovno.";
                    return View(loginVM);
                }
                TempData["Error"] = "Pogrešan unos. Molimo pokušajte ponovno.";
                return View(loginVM);

            }
          
            return View(loginVM);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
                      new { ReturnUrl = returnUrl });
            var properties = _signInManager
                .ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginVM loginVM = new LoginVM
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty,
                    $"Javila se pogreška prilikom dohvaćanja eksternih podataka : {remoteError}.");

                return View("Login", loginVM);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty,
                    "Javila se pogreška prilikom dohvaćanja eksternih podataka.");

                return View("Login", loginVM);
            }

            //var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            //ApplicationUser user = null;

            //if (email != null)
            //{
            //    user = await _userManager.FindByEmailAsync(email);

            //    if (user != null && !user.EmailConfirmed)
            //    {
            //        ModelState.AddModelError(string.Empty, "Potvrda vaše e-mail adrese nije još izvršena.");
            //        return View("Login", loginVM);
            //    }
            //}

            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                                        info.LoginProvider, info.ProviderKey,
                                        isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await _userManager.CreateAsync(user);

                        //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        //var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        //                new { userId = user.Id, token = token }, Request.Scheme);

                        //_logger.Log(LogLevel.Warning, confirmationLink);

                        //ViewBag.ErrorTitle = "Registracija uspješna";
                        //ViewBag.ErrorMessage = "Prije nego što prijava bude moguća, molimo vas da povrdite svoju " +
                        //    "e-mail adresu, putem linka koji je poslan na istu.";
                        //return View("Error");
                    }

                    await _userManager.AddLoginAsync(user, info);
                    await _userManager.AddToRoleAsync(user, UserRoles.User);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                ViewBag.ErrorTitle = $"Zahtjev e-mail adresom nije primljen od strane : {info.LoginProvider}.";

                return View("Error");
            }
        }

        public IActionResult Register()
        {
            var result = new RegisterVM();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);
            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "Već postoji korisnik sa unesenom e-mail adresom.";
                return View(registerVM);
            }

            var newUser = new ApplicationUser() { Email = registerVM.EmailAddress, UserName = registerVM.EmailAddress };

            var result = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (result.Succeeded)
            {
                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                //var confirmationLink = Url.Action("ConfirmEmail", "Account",
                //                        new { userId = newUser.Id, token = token }, Request.Scheme);

                //_logger.Log(LogLevel.Warning, confirmationLink);
                //TempData["Error"] = "Već postoji korisnik sa unesenom e-mail adresom.";
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                return View("CompleteRegister");
            }
            else
            {
                string errors = "";
                foreach (var error in result.Errors)
                {
                    errors += error.Description + ". ";
                }
                TempData["Error"] = errors;
                return View(registerVM);
            }

        }

        public IActionResult RegisterAsVendor()
        {
            var result = new RegisterVM();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsVendor(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);
            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "Već postoji korisnik sa unesenom e-mail adresom.";
                return View(registerVM);
            }

            var newUser = new ApplicationUser() { Email = registerVM.EmailAddress, UserName = registerVM.EmailAddress };

            var result = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (result.Succeeded)
            {
                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                //var confirmationLink = Url.Action("ConfirmEmail", "Account",
                //                        new { userId = newUser.Id, token = token }, Request.Scheme);

                //_logger.Log(LogLevel.Warning, confirmationLink);
                //TempData["Error"] = "Već postoji korisnik sa unesenom e-mail adresom.";
                await _userManager.AddToRoleAsync(newUser, UserRoles.Vendor);
                return View("CompleteRegister");
            }
            else
            {
                string errors = "";
                foreach (var error in result.Errors)
                {
                    errors += error.Description + ". ";
                }
                TempData["Error"] = errors;
                return View(registerVM);
            }

        }

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> ConfirmEmail(string userId, string token)
        //{
        //    if (userId == null || token == null)
        //    {
        //        return RedirectToAction("Index", "Product");
        //    }

        //    var user = await _userManager.FindByIdAsync(userId);

        //    if (user == null)
        //    {
        //        ViewBag.ErrorMessage = $"Korisničko ime {userId} nije dostupno.";
        //        return View("NotFound");
        //    }

        //    var result = await _userManager.ConfirmEmailAsync(user, token);

        //    if (result.Succeeded)
        //    {
        //        return View();
        //    }

        //    ViewBag.ErrorTitle = "Potvrda navedene e-mail adrese ne može biti obavljena.";
        //    return View("Error");
        //}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordVM.EmailAddress);

                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = forgotPasswordVM.EmailAddress, token = token }, Request.Scheme);

                    _logger.Log(LogLevel.Warning, passwordResetLink);

                    return View("ForgotPasswordConfirmation");
                }

                return View("ForgotPasswordConfirmation");
            }

            return View(forgotPasswordVM);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordVM.EmailAddress);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.Password);
                    if (result.Succeeded)
                    {
                        if (await _userManager.IsLockedOutAsync(user))
                        {
                            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }

                        return View("ResetPasswordConfirmation");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(resetPasswordVM);
                }

                return View("ResetPasswordConfirmation");
            }

            return View(resetPasswordVM);
        }


        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);

            var userHasPassword = await _userManager.HasPasswordAsync(user);

            if (!userHasPassword)
            {
                return RedirectToAction("AddPassword");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                var result = await _userManager.ChangePasswordAsync(user,
                    changePasswordVM.CurrentPassword, changePasswordVM.NewPassword);


                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }


                await _signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }

            return View(changePasswordVM);
        }

        [HttpGet]
        public async Task<IActionResult> AddPassword()
        {
            var user = await _userManager.GetUserAsync(User);

            var userHasPassword = await _userManager.HasPasswordAsync(user);

            if (userHasPassword)
            {
                return RedirectToAction("ChangePassword");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPassword(AddPasswordVM addPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var result = await _userManager.AddPasswordAsync(user, addPasswordVM.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                await _signInManager.RefreshSignInAsync(user);

                return View("AddPasswordConfirmation");
            }

            return View(addPasswordVM);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Users()
        {
            var users = _unitOfWork.ApplicationUsers.GetAll();
            return View(users);
        }

        public IActionResult UserProfile()
        {
            ViewBag.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var users = _unitOfWork.ApplicationUsers.GetAll();
            return View(users);
        }

    }
}

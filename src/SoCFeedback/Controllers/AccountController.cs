using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SoCFeedback.Enums;
using SoCFeedback.Models;
using SoCFeedback.Models.AccountViewModels;
using SoCFeedback.Services;

namespace SoCFeedback.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILoggerFactory loggerFactory,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _roleManager = roleManager;
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // Require the user to have a confirmed email before they can log on.
                //var user = await _userManager.FindByNameAsync(model.Email);
                //if (user != null)
                //{
                //    if (!await _userManager.IsEmailConfirmedAsync(user))
                //    {
                //        ModelState.AddModelError(string.Empty, "You must have a confirmed email to log in.");
                //        return View(model);
                //    }
                //}

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                    false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Register
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Surname = model.Surname,
                    Forename = model.Forename
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(model.Role.ToString()).Result)
                    {
                        var role = new IdentityRole
                        {
                            Name = model.Role.ToString()
                        };
                        var roleResult = _roleManager.CreateAsync(role).Result;
                        if (!roleResult.Succeeded)
                        {
                            await _userManager.DeleteAsync(user);
                            AddErrors(roleResult);
                        }
                    }
                    await _userManager.AddToRoleAsync(user, model.Role.ToString());
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = System.Net.WebUtility.UrlEncode(code);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    await _emailSender.SendEmailAsync(model.Email, "SoC Feedback Website Account",
                        $"An account on http://feedback.computing.dundee.ac.uk has been created for you.{Environment.NewLine}" +
                        $"Your login details are:{Environment.NewLine}" +
                        $"Id/Email:{model.Email}.{Environment.NewLine}" +
                        $"Password:{model.Password}");
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "Admin created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: Supervisors
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.Select(user => new AccountViewModel
            {
                Forename = user.Forename,
                Surname = user.Surname,
                Email = user.Email
            });

            var list = await users.ToListAsync();
            foreach (var user in list)
            {
                var dbUser = await _userManager.FindByNameAsync(user.Email);
                var role = await _userManager.GetRolesAsync(dbUser);
                user.Role = (Roles) Enum.Parse(typeof(Roles), role.FirstOrDefault());
            }

            return View(list);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string email)
        {
            if (email == null)
            {
                return NotFound();
            }
            var dbUser = await _userManager.FindByNameAsync(email);
            if (dbUser==null)
                return NotFound();

            return View(await GetAccountViewModel(dbUser));
        }

        [Authorize(Roles = "Admin")]
        private async Task<AccountViewModel> GetAccountViewModel(ApplicationUser dbUser)
        {
            var role = await _userManager.GetRolesAsync(dbUser);
            var model = new AccountViewModel
            {
                Email = dbUser.Email,
                Forename = dbUser.Forename,
                Surname = dbUser.Surname,
                Role = (Roles)Enum.Parse(typeof(Roles), role.FirstOrDefault())
            };
            return model;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string email, [Bind("Surname,Forename,Email,Role")] AccountViewModel model)
        {
            if (!email.Equals(model.Email))
                return NotFound();
            if (ModelState.IsValid)
            {
                var dbUser = await _userManager.FindByNameAsync(email);
                if (dbUser == null)
                    return NotFound();

                var roles = await _userManager.GetRolesAsync(dbUser);
                var role = (Roles) Enum.Parse(typeof(Roles), roles.FirstOrDefault());
                if (model.Role != role)
                {
                    var roleResult = IdentityResult.Success;
                    if (!_roleManager.RoleExistsAsync(model.Role.ToString()).Result)
                    {
                        var newRole = new IdentityRole
                        {
                            Name = model.Role.ToString()
                        };
                        roleResult = _roleManager.CreateAsync(newRole).Result;
                        if (!roleResult.Succeeded)
                            AddErrors(roleResult);
                    }
                    if (roleResult.Succeeded)
                    {
                        await _userManager.RemoveFromRoleAsync(dbUser, role.ToString());
                        await _userManager.AddToRoleAsync(dbUser, model.Role.ToString());
                    }
                }
                dbUser.Forename = model.Forename;
                dbUser.Surname = model.Surname;

                await _userManager.UpdateAsync(dbUser);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Levels/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string email)
        {
            if (email == null)
            {
                return NotFound();
            }
            var dbUser = await _userManager.FindByNameAsync(email);
            if (dbUser == null)
                return NotFound();

            return View(await GetAccountViewModel(dbUser));
        }

        // POST: Levels/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string email)
        {
            if (email == null)
            {
                return NotFound();
            }
            var cUser = await GetCurrentUserAsync();
            var dbUser = await _userManager.FindByNameAsync(email);
            if (dbUser == null)
                return NotFound();
            if (cUser == dbUser)
                return View("Error");

            await _userManager.DeleteAsync(dbUser);

            return RedirectToAction("Index");
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return View("Error");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return View("Error");
            code = WebUtility.UrlDecode(code);
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                    return View("ForgotPasswordConfirmation");

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new {userId = user.Id, code},
                    HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                    $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/AccessDenied
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        #endregion
    }
}
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
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
                //Require the user to have a confirmed email before they can log on.
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user != null)
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "You must have a confirmed email to log in.");
                        return View(model);
                    }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                    true);
                if (result.Succeeded)
                    return RedirectToLocal(returnUrl);
                if (result.IsLockedOut)
                    return View("Lockout");
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

                // create user
                var result = await _userManager.CreateAsync(user);
                //add user to a role
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

                    #pragma warning disable 4014
                    SendConfirmation(user);
                    #pragma warning restore 4014

                    return RedirectToAction(nameof(Index), new {Message = AccountMessageId.AccountCreated});
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        private async void SendConfirmation(ApplicationUser user)
        {
            // Send an email with this link
            var code = WebUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));
            var passCode = WebUtility.UrlEncode(await _userManager.GeneratePasswordResetTokenAsync(user));
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new {userId = user.Id, code, passCode},
                HttpContext.Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "Computing Feedback - Account Created",
                $"An account on <a href='http://feedback.computing.dundee.ac.uk'>http://feedback.computing.dundee.ac.uk</a> has been created for <strong>{user.Email}</strong>.<br/>" +
                $"You must set a new password before you can use your account by clicking <a href='{callbackUrl}'>here</a>.<br/><br/>" +
                $"This is an automated email, please do not reply.<br/>");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            if (email == null)
                return RedirectToAction(nameof(Index), new {Message = AccountMessageId.Error});
            var dbUser = await _userManager.FindByNameAsync(email);
            if (dbUser == null)
                return RedirectToAction(nameof(Index), new {Message = AccountMessageId.Error});
            #pragma warning disable 4014
            SendConfirmation(dbUser);
            #pragma warning restore 4014
            return RedirectToAction(nameof(Index), new {Message = AccountMessageId.EmailResent});
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index(AccountMessageId? message = null)
        {
            // possible statuses
            ViewData["StatusMessage"] =
                message == AccountMessageId.AcountUpdated
                    ? "Account has been updated."
                    : message == AccountMessageId.AccountCreated
                        ? "Account has been created and email sent."
                        : message == AccountMessageId.Error
                            ? "An error has occurred."
                            : message == AccountMessageId.EmailResent
                                ? "Email confirmation has been resent."
                                : message == AccountMessageId.AccountDeleted
                                    ? "Account has been deleted."
                                    : "";

            // get all users
            var users = _userManager.Users.Select(user => new AccountViewModel
            {
                Forename = user.Forename,
                Surname = user.Surname,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed
            });

            var list = await users.ToListAsync();
            // get user role
            foreach (var user in list)
            {
                var dbUser = await _userManager.FindByNameAsync(user.Email);
                var role = await _userManager.GetRolesAsync(dbUser);
                user.Role = (Roles) Enum.Parse(typeof(Roles), role.FirstOrDefault());
            }

            return View(list);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string email)
        {
            if (email == null)
                return NotFound();
            var dbUser = await _userManager.FindByNameAsync(email);
            if (dbUser == null)
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
                Role = (Roles) Enum.Parse(typeof(Roles), role.FirstOrDefault())
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

                // update user role if it has changed
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
                return RedirectToAction(nameof(Index), new {Message = AccountMessageId.AcountUpdated});
            }
            return View(model);
        }

        // GET: Levels/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string email)
        {
            if (email == null)
                return NotFound();
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
                return NotFound();
            var cUser = await GetCurrentUserAsync();
            var dbUser = await _userManager.FindByNameAsync(email);
            if (dbUser == null)
                return NotFound();
            if (cUser == dbUser)
                return RedirectToAction(nameof(HomeController.Error), "Home");

            await _userManager.DeleteAsync(dbUser);

            return RedirectToAction(nameof(Index), new {Message = AccountMessageId.AccountDeleted});
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //
        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string code = null, string passCode = null, string userId = null)
        {
            if (code == null || passCode == null || userId == null)
                return RedirectToAction(nameof(HomeController.Error), "Home");
            var user = await _userManager.FindByIdAsync(userId);
            if (user.EmailConfirmed)
                return RedirectToAction(nameof(HomeController.Error), "Home", new {error = 1});
            ViewData["userEmail"] = user.Email;
            return View();
        }

        // GET: /Account/ConfirmEmail
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return RedirectToAction(nameof(HomeController.Error), "Home");
            if (user.EmailConfirmed)
                return RedirectToAction(nameof(HomeController.Error), "Home", new {error = 1});

            var result = await _userManager.ConfirmEmailAsync(user, WebUtility.UrlDecode(model.Code));
            var passResult = await _userManager.ResetPasswordAsync(user, WebUtility.UrlDecode(model.PassCode),
                model.Password);
            // if successfully confirmed email, login
            if (result.Succeeded && passResult.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("ConfirmEmailConfirmation");
            }
            // if confirmation didn't succeed 
            user.EmailConfirmed = false;
            await _userManager.UpdateAsync(user);
            AddErrors(result);
            AddErrors(passResult);
            ViewData["userEmail"] = user.Email;
            return View();
        }

        //
        // GET: /Account/ConfirmEmailConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmEmailConfirmation()
        {
            return View();
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

                var code = WebUtility.UrlEncode(await _userManager.GeneratePasswordResetTokenAsync(user));
                var callbackUrl = Url.Action("ResetPassword", "Account", new {code},
                    HttpContext.Request.Scheme);
                // Send an email with this link
                #pragma warning disable 4014
                _emailSender.SendEmailAsync(model.Email, "Computing Feedback - Password Reset",
                    $"Please reset your password by clicking <a href='{callbackUrl}'>here</a>.<br/><br/>" +
                    $"This is an automated email, please do not reply.<br/> ");
                #pragma warning restore 4014
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
            return code == null ? (IActionResult) RedirectToAction(nameof(HomeController.Error), "Home") : View();
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
                return RedirectToAction(nameof(HomeController.Error), "Home");
            var result = await _userManager.ResetPasswordAsync(user, WebUtility.UrlDecode(model.Code), model.Password);
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

        public enum AccountMessageId
        {
            AccountCreated,
            Error,
            AcountUpdated,
            EmailResent,
            AccountDeleted
        }

        #endregion
    }
}
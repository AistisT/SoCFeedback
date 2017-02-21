using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoCFeedback.Models;
using SoCFeedback.Models.ManageViewModels;

namespace SoCFeedback.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //
        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess
                    ? "Your password has been changed."
                            : message == ManageMessageId.Error
                                ? "An error has occurred."
                                        : message==ManageMessageId.ChangeNameSuccess
                                        ? "Your name has been changed."
                                        : "";

            var user = await GetCurrentUserAsync();
            if (user == null)
                return RedirectToAction(nameof(HomeController.Error), "Home");
            var model = new IndexViewModel
            {
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user)
            };
            return View(model);
        }

        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction(nameof(Index), new {Message = ManageMessageId.ChangePasswordSuccess});
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new {Message = ManageMessageId.Error});
        }

        //
        // GET: /Manage/ChangeName
        [HttpGet]
        public async Task<IActionResult> ChangeName()
        {
            var user = await GetCurrentUserAsync();
            var model = new ChangeNameViewModel
            {
                Surname = user.Surname,
                Forename = user.Forename
            };
            return View(model);
        }

        //
        // POST: /Manage/ChangeName
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeName(ChangeNameViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                user.Forename = model.Forename;
                user.Surname = model.Surname;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangeNameSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            Error,
            ChangeNameSuccess
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
        #endregion
    }
}
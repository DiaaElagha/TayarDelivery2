﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TayarDelivery.Entity.Domins;
using Microsoft.EntityFrameworkCore;
using TayarDelivery.Data.StaticModel;

namespace TayarDelivery.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            if (User.Identity.IsAuthenticated)
            {
                return LocalRedirect(Url.Content("~/"));
            }
            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = await _userManager.Users.Include(x => x.UserType).SingleOrDefaultAsync(x => x.UserName.Equals(Input.Email));
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "محاولة تسجيل الدخول غير صحيحة.");
                    return Page();
                }
                if (user.IsActive)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        string usertype = user.UserType.TitlePrograming;
                        if (usertype.Equals(UserTypeValues.ADMINISTRATOR))
                        {
                            return LocalRedirect(Url.Content("/Admin/Orders/Index"));
                        }
                        else if (usertype.Equals(UserTypeValues.TRADER))
                        {
                            return LocalRedirect(Url.Content("/Admin/TraderOrders/Index"));
                        }
                        else if (usertype.Equals(UserTypeValues.DRIVER))
                        {
                            return LocalRedirect(Url.Content("/Admin/DriverOrders/Index"));
                        }
                        else if (usertype.Equals(UserTypeValues.ACCOUNTANT))
                        {
                            return LocalRedirect(Url.Content("/Admin/Orders/Index"));
                        }
                        else if (usertype.Equals(UserTypeValues.MANAGER))
                        {
                            return LocalRedirect(Url.Content("/Admin/Orders/Index"));
                        }
                        return LocalRedirect(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "محاولة تسجيل الدخول غير صحيحة.");
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Account is UnActive.");
                    return Page();
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

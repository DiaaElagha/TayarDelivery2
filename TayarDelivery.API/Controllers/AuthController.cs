using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TayarDelivery.API.Dto.Auth;
using TayarDelivery.API.Dto.Base;
using TayarDelivery.API.Dto.General.Auth;
using TayarDelivery.API.Helpers;
using TayarDelivery.API.Service.Interface;
using TayarDelivery.Data.Data;
using TayarDelivery.Data.StaticModel;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Entity.Domins.User;
using TayarDelivery.Repository.Interfaces;

namespace TayarDelivery.API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthRepository _authRepsotory;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IBaseRepository<UserProfile> _userProfileRepository;

        public AuthController(
            IAuthRepository authRepsotory,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IBaseRepository<UserProfile> userProfileRepository) : base(configuration, userManager, mapper)
        {
            _authRepsotory = authRepsotory;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userProfileRepository = userProfileRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _authRepsotory.Login(model);
                ApplicationUser LoginUser = null;
                if (result.Succeeded)
                {
                    LoginUser = _userManager.Users.Include(x => x.UserType)
                        .Include(x => x.Area).Include(x => x.PriceType).SingleOrDefault(x => x.UserName.Equals(model.Username));

                    if (LoginUser.IsActive)
                    {
                        string token = await _authRepsotory.CreateAccess(LoginUser);

                        LoginUser.FcmToken = model.FcmToken;
                        LoginUser.AccessToken = token;

                        await _userManager.UpdateAsync(LoginUser);
                        UserProfileDto userProfile = _mapper.Map<UserProfileDto>(LoginUser);

                        LoginResponseDto loginResponseDto = new LoginResponseDto
                        {
                            AccessToken = token,
                            ProfileData = userProfile
                        };
                        return GetResponse("تم تسجيل الدخول بنجاح", true, loginResponseDto);
                    }
                    else
                    {
                        return GetResponse("عذرا الحساب غير نشط", false, null);
                    }

                }
                else
                {
                    return GetResponse("خطأ في اسم المستخدم أو كلمة مرور", false, null);
                }
            }
            return GetResponse("بعض الحقول مطلوبة", false, General.GetValidationErrores(ModelState));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterTrader([FromBody] RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var newEntity = _mapper.Map<ApplicationUser>(model);

                IdentityResult result = await _authRepsotory.SignUp(newEntity, model.Password, UserTypeValues.TRADER);
                if (result.Succeeded)
                {
                    ApplicationUser RegisterUser = _userManager.Users.Include(x => x.Area).Include(x => x.PriceType)
                        .SingleOrDefault(x => x.UserName.Equals(model.UserName));
                    UserProfile profileSettings = await _userProfileRepository.Get(RegisterUser.Id);
                    UserProfileSettingsDto profileSettingsDTO = _mapper.Map<UserProfileSettingsDto>(profileSettings);

                    UserProfileDto userProfile = _mapper.Map<UserProfileDto>(RegisterUser);
                    LoginResponseDto loginResponseDto = new LoginResponseDto
                    {
                        AccessToken = null,
                        ProfileData = userProfile,
                        ProfileSettings = profileSettingsDTO
                    };
                    return GetResponse("تم التسجيل بنجاح", true, loginResponseDto);
                }
                else
                {
                    List<string> errors = new List<string>();
                    foreach (var x in result.Errors)
                    {
                        String errorcode = x.Code.ToString();
                        if (errorcode.Equals("DuplicateUserName"))
                        {
                            errors.Add("DuplicateUserName");
                        }
                    }
                    return GetResponse("عذرا فشل التسجيل", false, errors[0]);
                }
            }
            return GetResponse("بعض الحقول مطلوبة", false, General.GetValidationErrores(ModelState));
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await GetCurrentUser();
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user.UserName, model.OldPassword, true, lockoutOnFailure: true);
                    if (result.Succeeded)
                    {
                        var newPassword = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
                        user.PasswordHash = newPassword;
                        var identityResult = await _userManager.UpdateAsync(user);
                        if (identityResult.Succeeded)
                        {
                            return GetResponse("تم تغيير كلمة المرور", true, null);
                        }
                        else
                        {
                            return GetResponse("كلمة مرور خاطئة", false, null);
                        }
                    }
                    else
                    {
                        return GetResponse("كلمة مرور خاطئة", false, null);
                    }
                }
                else
                {
                    return GetResponse("اسم المستخدم خاطئ", false, null);
                }
            }
            return GetResponse("بعض الحقول مطلوبة", false, General.GetValidationErrores(ModelState));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            ApplicationUser user = await GetCurrentUser();
            if (user != null)
            {
                user.FcmToken = "";
                user.AccessToken = "";
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return GetResponse("Signed out successfully", true, null);
                }
            }
            return GetResponse("Something want wrong", true, null);
        }

        [HttpPost]
        public async Task<IActionResult> RefreshFcmToken([FromBody] RefreshFcmTokenDto model)
        {
            ApplicationUser user = await GetCurrentUser();
            if (user != null)
            {
                user.FcmToken = model.FcmToken;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return GetResponse("تم تحديث البيانات بنجاح", true, null);
                }
            }
            return GetResponse("بعض الحقول مطلوبة", false, null);
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            ApplicationUser user = await GetCurrentUser();
            if (user != null)
            {
                UserProfileDto userProfile = _mapper.Map<UserProfileDto>(user);
                return GetResponse("Data retrieved successfully", true, userProfile);
            }
            else
            {
                return GetResponse("Username is not valid", false, null);
            }
        }

    }
}

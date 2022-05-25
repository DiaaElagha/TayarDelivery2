using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TayarDelivery.API.Dto.Auth;
using TayarDelivery.API.Service.Interface;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Entity.Domins.User;
using TayarDelivery.Repository.Interfaces;

namespace TayarDelivery.API.Service.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<UserType> _userTypeRepository;
        private readonly IBaseRepository<UserProfile> _userProfileRepository;

        public AuthRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IMapper mapper,
            IBaseRepository<UserType> userTypeRepository,
            IBaseRepository<UserProfile> userProfileRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
            _userTypeRepository = userTypeRepository;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<string> CreateAccess(ApplicationUser user)
        {
            IList<String> role = await _userManager.GetRolesAsync(user);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim("suserid", user.Id),
                new Claim(ClaimTypes.Role, role[0]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
               };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddDays(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<SignInResult> Login(LoginDto login)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, true, lockoutOnFailure: true);
            return result;
        }

        public async Task<IdentityResult> SignUp(ApplicationUser user, string password, string userTypeName)
        {
            var userTypeItem = await _userTypeRepository.FindSingle(c => c.TitlePrograming.Equals(userTypeName));
            user.UserTypeID = userTypeItem.Id;
            IdentityResult result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var result2 = await _userManager.AddToRoleAsync(user, userTypeName);
                await _userProfileRepository.AddAsync(new UserProfile {
                    UserId = user.Id
                });
            }
            return result;
        }
    }
}

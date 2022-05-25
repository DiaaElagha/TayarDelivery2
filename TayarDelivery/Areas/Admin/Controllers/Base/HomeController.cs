using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TayarDelivery.Data.Data;
using TayarDelivery.Entity.Domins;

namespace TayarDelivery.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper) : base(userManager, roleManager, mapper)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetUsersByType(int id)
        {
            var listData = await _userManager.Users.Where(x => x.UserTypeID == id).ToListAsync();
            return Json(listData);
        }

    }
}

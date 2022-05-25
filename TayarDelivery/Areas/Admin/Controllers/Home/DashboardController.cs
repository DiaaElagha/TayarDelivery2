using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TayarDelivery.Entity.Domins;

namespace TayarDelivery.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper) : base(userManager, roleManager, mapper)
        {
        }

        public IActionResult Index()
        {
            return View();
        }


    }
}

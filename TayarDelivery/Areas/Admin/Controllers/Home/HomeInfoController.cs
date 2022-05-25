using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TayarDelivery.Areas.Admin.Controllers;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Entity.Domins.Home;
using TayarDelivery.Extensions;
using TayarDelivery.Repository.Interfaces;

namespace PlacesGuide.Web.Areas.Admin.Controllers.Setting
{
    public class HomeInfoController : BaseController
    {
        protected readonly IBaseRepository<HomeInfo> _homeInfoRepository;
        private readonly IHostingEnvironment _IHostingEnvironment;

        public HomeInfoController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IHostingEnvironment IHostingEnvironment,
            IBaseRepository<HomeInfo> homeInfoRepository) : base(userManager, roleManager, mapper)
        {
            this._homeInfoRepository = homeInfoRepository;
            this._IHostingEnvironment = IHostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsExists())
            {
                HomeInfo information = new HomeInfo();
                await _homeInfoRepository.AddAsync(information);
            }
            var info = await _homeInfoRepository.Get().FirstOrDefaultAsync();
            return View(info);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(HomeInfo model , IFormFile BackgroundImage)
        {
            if (model == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (BackgroundImage != null)
                    {
                        model.BackgroundImage = await ImageHelper.UploadImage(BackgroundImage, _IHostingEnvironment, "files/filesSystem");
                    }

                    var result = await _homeInfoRepository.UpdateAsync(model);
                    TempData["EditStatus"] = "تمت عملية التعديل بنجاح";
                    return View(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["EditStatus"] = "فشلت عملية التعديل";
                }
            }
            return View(model);
        }

        private bool IsExists()
        {
            return _homeInfoRepository.Get().Any();
        }

    }
}

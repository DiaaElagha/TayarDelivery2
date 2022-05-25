using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Helper;
using System.Collections.Generic;
using AutoMapper;
using TayarDelivery.Repository.Interfaces;
using TayarDelivery.Entity.Domins.LookUp;
using TayarDelivery.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace TayarDelivery.Areas.Admin.Controllers
{
    [Authorize(Roles = "administrator")]
    public class CompanyInformationController : BaseController
    {
        protected readonly IBaseRepository<CompanyInformation> _companyInfoRepository;
        public CompanyInformationController(
            IBaseRepository<CompanyInformation> companyInfoRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper) : base(userManager, roleManager, mapper)
        {
            this._companyInfoRepository = companyInfoRepository;
        }

        // GET: Admin/CompanyInformation/Create
        public async Task<IActionResult> Index()
        {
            if (!IsExists())
            {
                CompanyInformation information = new CompanyInformation();
                await _companyInfoRepository.AddAsync(information);
            }
            var info = await _companyInfoRepository.Get().FirstOrDefaultAsync();
            return View(info);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CompanyInformation model)
        {
            if (model == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _companyInfoRepository.UpdateAsync(model);
                    ViewData["EditStatus"] = "تمت عملية التعديل بنجاح";
                    return View(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewData["EditStatus"] = "فشلت عملية التعديل";
                }
            }
            return View(model);
        }

        private bool IsExists()
        {
            return _companyInfoRepository.Get().Any();
        }

    }
}

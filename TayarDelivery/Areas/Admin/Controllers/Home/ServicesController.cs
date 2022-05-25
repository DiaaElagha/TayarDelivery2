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
using TayarDelivery.Models.ViewModel.Auth;
using TayarDelivery.Entity.Domins.Home;
using TayarDelivery.Models.ViewModel.Home;
using Microsoft.AspNetCore.Http;
using TayarDelivery.Extensions;
using Microsoft.AspNetCore.Hosting;

namespace TayarDelivery.Areas.Admin.Controllers.Auth
{
    [Authorize(Roles = "administrator")]
    public class ServicesController : BaseController
    {
        private readonly IBaseRepository<Services> _servicesRepository;
        private readonly IHostingEnvironment _IHostingEnvironment;
        public ServicesController(
            IBaseRepository<Services> servicesRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IHostingEnvironment IHostingEnvironment,
            IMapper mapper) : base(userManager, roleManager, mapper)
        {
            this._servicesRepository = servicesRepository;
            this._IHostingEnvironment = IHostingEnvironment;
        }

        [HttpPost]
        public JsonResult AjaxData([FromBody] dynamic data)
        {
            DataTableHelper d = new DataTableHelper(data);

            var query = _servicesRepository.Filter(
                filter: x => (d.SearchKey == null || x.Name.Contains(d.SearchKey) || x.Description.Contains(d.SearchKey)),
                orderBy: x => x.OrderBy(x => x.CreateAt)).ToList();

            int totalCount = query.Count();

            var items = query.Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.IconName,
                createAt = x.CreateAt.Value.ToString("MM/dd/yyyy"),
            }).Skip(d.Start).Take(d.Length).ToList();
            var result =
               new
               {
                   draw = d.Draw,
                   recordsTotal = totalCount,
                   recordsFiltered = totalCount,
                   data = items
               };
            return Json(result);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceVM model)
        {
            if (ModelState.IsValid)
            {
                var newEntity = _mapper.Map<Services>(model);
                var result = await _servicesRepository.AddAsync(newEntity);
                if (result != null)
                    return Content(ShowMessage.AddSuccessResult(), "application/json");
                return Content(ShowMessage.FailedResult(), "application/json");
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await _servicesRepository.Get(id);
            if (model == null)
            {
                return NotFound();
            }
            var newEntity = _mapper.Map<ServiceVM>(model);
            return View(newEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var baseEntoty = await _servicesRepository.Get(id);
                    PropertyCopy.Copy(model, baseEntoty);
                    await _servicesRepository.UpdateAsync(baseEntoty);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ServiceExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Content(ShowMessage.EditSuccessResult(), "application/json");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await _servicesRepository.Get(id);
            if (model == null)
            {
                return NotFound();
            }
            var result = await _servicesRepository.DeleteAsync(model);
            if (result != null)
                return Content(ShowMessage.DeleteSuccessResult(), "application/json");
            return Content(ShowMessage.FailedResult(), "application/json");
        }

        private async Task<bool> ServiceExists(int id)
        {
            return await _servicesRepository.Any(e => e.Id == id);
        }
    }
}

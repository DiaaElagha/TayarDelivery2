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
    public class AreasController : BaseController
    {
        protected readonly IBaseRepository<Area> _areaRepository;
        public AreasController(
            IBaseRepository<Area> areaRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper) : base(userManager, roleManager, mapper)
        {
            this._areaRepository = areaRepository;
        }

        [HttpPost]
        public JsonResult AjaxData([FromBody] dynamic data)
        {
            DataTableHelper d = new DataTableHelper(data);

            var query = _areaRepository.Filter(
                filter: x => (d.SearchKey == null || x.Name.Contains(d.SearchKey)),
                orderBy: x => x.OrderBy(x => x.CreateAt));

            int totalCount = query.Count();

            var items = query.Select(x => new
            {
                x.Id,
                x.Name,
                x.Latitude,
                x.Longitude,
                createAt = x.CreateAt.Value.ToString("MM/dd/yyyy"),
            }).Skip(d.Start).Take(d.Length);
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

        // GET: Admin/Area/Index
        public IActionResult Index()
        {
            return View();
        }


        // GET: Admin/Area/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Area/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AreaVM model)
        {
            if (ModelState.IsValid)
            {
                var newEntity = _mapper.Map<Area>(model);
                var result = await _areaRepository.AddAsync(newEntity);
                if (result != null)
                    return Content(ShowMessage.AddSuccessResult(), "application/json");
                return Content(ShowMessage.FailedResult(), "application/json");
            }
            return View(model);
        }

        // GET: Admin/Area/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await _areaRepository.Get(id);
            if (model == null)
            {
                return NotFound();
            }
            var newEntity = _mapper.Map<AreaVM>(model);
            return View(newEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AreaVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var baseEntoty = await _areaRepository.Get(id);
                    PropertyCopy.Copy(model, baseEntoty);
                    await _areaRepository.UpdateAsync(baseEntoty);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await AreaExists(id))
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

        // GET: Admin/Area/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await _areaRepository.Get(id);
            if (model == null)
            {
                return NotFound();
            }
            var result = await _areaRepository.DeleteAsync(model);
            if (result != null)
                return Content(ShowMessage.DeleteSuccessResult(), "application/json");
            return Content(ShowMessage.FailedResult(), "application/json");
        }

        private async Task<bool> AreaExists(int id)
        {
            return await _areaRepository.Any(e => e.Id == id);
        }
    }
}

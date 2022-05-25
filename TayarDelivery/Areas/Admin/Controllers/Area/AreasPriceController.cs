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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;

namespace TayarDelivery.Areas.Admin.Controllers
{
    [Authorize(Roles = "administrator")]
    public class AreasPriceController : BaseController
    {
        protected readonly IBaseRepository<AreasPrice> _areasPriceRepository;
        protected readonly IBaseRepository<Area> _areaRepository;
        public AreasPriceController(
            IBaseRepository<AreasPrice> areasPriceRepository,
            IBaseRepository<Area> areaRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper) : base(userManager, roleManager, mapper)
        {
            this._areasPriceRepository = areasPriceRepository;
            this._areaRepository = areaRepository;
        }

        [HttpPost]
        public JsonResult AjaxData([FromBody] dynamic data)
        {
            DataTableHelper d = new DataTableHelper(data);

            var query = _areasPriceRepository.Filter(
                filter: x => (d.SearchKey == null
                    || x.DealerArea.Name.Contains(d.SearchKey)
                    || x.ReceverArea.Name.Contains(d.SearchKey)),
                orderBy: x => x.OrderBy(x => x.CreateAt),
                include: x => x.Include(c => c.DealerArea).Include(c => c.ReceverArea)).ToList();

            int totalCount = query.Count();

            var items = query.Select(x => new
            {
                AreaDealer = x.DealerArea.Name,
                AreaRecever = x.ReceverArea.Name,
                x.ReceverAreaId,
                x.DealerAreaId,
                x.Price,
                CanDiscounted = x.CanDiscount ? "يمكن" : "لا يمكن",
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

        // GET: Admin/AreasPrice/Index
        public IActionResult Index()
        {
            return View();
        }


        // GET: Admin/AreasPrice/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ListAreas"] = new SelectList(await _areaRepository.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Admin/AreasPrice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AreasPriceVM model)
        {
            if (ModelState.IsValid)
            {
                var newEntity = _mapper.Map<AreasPrice>(model);
                newEntity.CreateByUserId = UserId;
                var result = await _areasPriceRepository.AddAsync(newEntity);
                if (result != null)
                    return Content(ShowMessage.AddSuccessResult(), "application/json");
                return Content(ShowMessage.FailedResult(), "application/json");
            }
            ViewData["ListAreas"] = new SelectList(await _areaRepository.GetAll(), "Id", "Name");
            return View(model);
        }

        // GET: Admin/AreasPrice/Edit/5
        public async Task<IActionResult> Edit(int? receverAreaId, int? dealerAreaId)
        {
            if (receverAreaId == null || dealerAreaId == null)
            {
                return NotFound();
            }
            var model = await _areasPriceRepository.FindSingle(x => x.ReceverAreaId == receverAreaId && x.DealerAreaId == dealerAreaId);
            if (model == null)
            {
                return NotFound();
            }
            var newEntity = _mapper.Map<AreasPriceVM>(model);
            ViewData["ListAreas"] = new SelectList(await _areaRepository.GetAll(), "Id", "Name");
            return View(newEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int receverAreaId, int dealerAreaId, AreasPriceVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var baseEntoty = await _areasPriceRepository.FindSingle(x => x.ReceverAreaId == receverAreaId && x.DealerAreaId == dealerAreaId);
                    PropertyCopy.Copy(model, baseEntoty);
                    baseEntoty.UpdateAt = DateTime.Now;
                    baseEntoty.UpdateByUserId = UserId;
                    await _areasPriceRepository.UpdateAsync(baseEntoty);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await AreasPriceExists(receverAreaId, dealerAreaId))
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
            ViewData["ListAreas"] = new SelectList(await _areaRepository.GetAll(), "Id", "Name");
            return View(model);
        }

        // GET: Admin/AreasPrice/Delete/5
        public async Task<IActionResult> Delete(int? receverAreaId, int? dealerAreaId)
        {
            if (receverAreaId == null || dealerAreaId == null)
            {
                return NotFound();
            }
            var model = await _areasPriceRepository.FindSingle(x => x.ReceverAreaId == receverAreaId && x.DealerAreaId == dealerAreaId);
            if (model == null)
            {
                return NotFound();
            }
            var result = await _areasPriceRepository.DeleteAsync(model);
            if (result != null)
                return Content(ShowMessage.DeleteSuccessResult(), "application/json");
            return Content(ShowMessage.FailedResult(), "application/json");
        }

        private async Task<bool> AreasPriceExists(int receverAreaId, int dealerAreaId)
        {
            return await _areasPriceRepository.Any(e => e.ReceverAreaId == receverAreaId && e.DealerAreaId == dealerAreaId);
        }
    }
}

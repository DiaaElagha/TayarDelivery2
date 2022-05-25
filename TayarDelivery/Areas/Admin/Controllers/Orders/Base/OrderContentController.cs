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
    public class OrderContentController : BaseController
    {
        protected readonly IBaseRepository<OrderContent> _orderContentRepository;
        public OrderContentController(
            IBaseRepository<OrderContent> orderContentRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper) : base(userManager, roleManager, mapper)
        {
            this._orderContentRepository = orderContentRepository;
        }

        [HttpPost]
        public JsonResult AjaxData([FromBody] dynamic data)
        {
            DataTableHelper d = new DataTableHelper(data);

            var query = _orderContentRepository.Filter(
                filter: x => (d.SearchKey == null || x.Title.Contains(d.SearchKey)),
                orderBy: x => x.OrderBy(x => x.CreateAt)).ToList();

            int totalCount = query.Count();

            var items = query.Select(x => new
            {
                x.Id,
                x.Title,
                x.Description,
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
        public async Task<IActionResult> Create(OrderContentVM model)
        {
            if (ModelState.IsValid)
            {
                var newEntity = _mapper.Map<OrderContent>(model);
                var result = await _orderContentRepository.AddAsync(newEntity);
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
            var model = await _orderContentRepository.Get(id);
            if (model == null)
            {
                return NotFound();
            }
            var newEntity = _mapper.Map<OrderContentVM>(model);
            return View(newEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderContentVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var baseEntoty = await _orderContentRepository.Get(id);
                    PropertyCopy.Copy(model, baseEntoty);
                    await _orderContentRepository.UpdateAsync(baseEntoty);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await OrderContentExists(id))
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
            var model = await _orderContentRepository.Get(id);
            if (model == null)
            {
                return NotFound();
            }
            var result = await _orderContentRepository.DeleteAsync(model);
            if (result != null)
                return Content(ShowMessage.DeleteSuccessResult(), "application/json");
            return Content(ShowMessage.FailedResult(), "application/json");
        }


        private async Task<bool> OrderContentExists(int id)
        {
            return await _orderContentRepository.Any(e => e.Id == id);
        }
    }
}

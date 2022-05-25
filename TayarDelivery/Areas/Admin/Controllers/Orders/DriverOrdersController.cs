using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Helper;
using AutoMapper;
using TayarDelivery.Repository.Interfaces;
using TayarDelivery.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using TayarDelivery.Entity.Domins.LookUp;
using Microsoft.AspNetCore.Authorization;
using TayarDelivery.Models.ViewModel.Orders;
using TayarDelivery.Data.StaticModel;

namespace TayarDelivery.Areas.Admin.Controllers
{
    [Authorize(Roles = "driver")]
    public class DriverOrdersController : BaseController
    {
        protected readonly IBaseRepository<Order> _orderRepository;
        protected readonly IBaseRepository<OrderStatus> _orderStatusRepository;
        protected readonly IBaseRepository<OrderHistory> _orderHistoryRepository;
        protected readonly IBaseRepository<OrderType> _orderTypeRepository;
        protected readonly IBaseRepository<Area> _areaRepository;
        protected readonly IBaseRepository<AreasPrice> _areaPriceRepository;
        public DriverOrdersController(
            IBaseRepository<Order> orderRepository,
            IBaseRepository<OrderStatus> orderStatusRepository,
            IBaseRepository<OrderHistory> orderHistoryRepository,
            IBaseRepository<OrderType> orderTypeRepository,
            IBaseRepository<Area> areaRepository,
            IBaseRepository<AreasPrice> areaPriceRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper) : base(userManager, roleManager, mapper)
        {
            this._orderRepository = orderRepository;
            this._orderStatusRepository = orderStatusRepository;
            this._orderHistoryRepository = orderHistoryRepository;
            this._orderTypeRepository = orderTypeRepository;
            this._areaRepository = areaRepository;
            this._areaPriceRepository = areaPriceRepository;
        }

        // GET: Admin/DriverOrders/Index
        public IActionResult Index()
        {
            var listUsers = _userManager.Users.Include(x => x.UserType).Where(x => x.IsActive);
            ViewData["listOrderStatuses"] = new SelectList(_orderStatusRepository.Find(x => x.TitlePrograming.Equals(OrderStatusValues.REJECTED) 
            || x.TitlePrograming.Equals(OrderStatusValues.DONEDELIVERED)).Result, "Id", "TitleView");
            ViewData["listOrderStatusesAll"] = new SelectList(_orderStatusRepository.GetAll().Result, "Id", "TitleView");
            ViewData["listAreas"] = new SelectList(_areaRepository.GetAll().Result, "Id", "Name");
            ViewData["listOrderTypes"] = new SelectList(_orderTypeRepository.GetAll().Result, "Id", "Name");
            ViewData["listTraders"] = new SelectList(listUsers.Where(x => x.UserType.TitlePrograming.Equals(UserTypeValues.TRADER)), "Id", "FullName");

            return View("Index", UserId);
        }

        private async Task<bool> OrderExists(int id)
        {
            return await _orderRepository.Any(e => e.OrderId == id);
        }

        public async Task<IActionResult> AddNote(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await _orderRepository.Get(id);
            if (model == null)
            {
                return NotFound();
            }
            ViewBag.OrderId = id;
            return View(new AddNoteVM { text = model.NoteDriver });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNote(int id, AddNoteVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = await _orderRepository.Get(id);
                    if (entity == null)
                    {
                        return NotFound();
                    }
                    entity.NoteDriver = model.text;
                    await _orderRepository.UpdateAsync(entity);

                    {
                        OrderHistory orderHistory = new OrderHistory
                        {
                            Title = OrderHistoryValues.AddNoteTitle,
                            Description = OrderHistoryValues.AddNote(UserName),
                            CreateByUserId = UserId,
                            OrderId = id
                        };
                        await _orderHistoryRepository.AddAsync(orderHistory);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await OrderExists(id))
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

    }
}

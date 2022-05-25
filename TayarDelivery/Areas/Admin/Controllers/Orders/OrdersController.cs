using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Data.Data;
using TayarDelivery.Helper;
using System.Collections.Generic;
using AutoMapper;
using TayarDelivery.Repository.Interfaces;
using Newtonsoft.Json.Linq;
using TayarDelivery.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using TayarDelivery.Entity.Domins.LookUp;
using Microsoft.AspNetCore.Authorization;
using TayarDelivery.Data.StaticModel;
using TayarDelivery.Entity.Helper;
using TayarDelivery.Repository.Repository.Interfaces;

namespace TayarDelivery.Areas.Admin.Controllers
{
    //[Authorize(Roles = "administrator,manager,accountant")]
    public class OrdersController : BaseController
    {
        protected readonly IBaseRepository<Order> _orderRepository;
        protected readonly IBaseRepository<OrderStatus> _orderStatusRepository;
        protected readonly IBaseRepository<OrderType> _orderTypeRepository;
        protected readonly IBaseRepository<OrderContent> _orderContentRepository;
        protected readonly IBaseRepository<OrderHistory> _orderHistoryRepository;
        protected readonly IBaseRepository<Area> _areaRepository;
        protected readonly IBaseRepository<AreasPrice> _areaPriceRepository;
        protected readonly IOrderRepository _orderDataRepository;
        public OrdersController(
            IBaseRepository<Order> orderRepository,
            IBaseRepository<OrderStatus> orderStatusRepository,
            IBaseRepository<OrderType> orderTypeRepository,
            IBaseRepository<OrderContent> orderContentRepository,
            IBaseRepository<OrderHistory> orderHistoryRepository,
            IBaseRepository<Area> areaRepository,
            IBaseRepository<AreasPrice> areaPriceRepository,
            IOrderRepository orderDataRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper) : base(userManager, roleManager, mapper)
        {
            this._orderRepository = orderRepository;
            this._orderStatusRepository = orderStatusRepository;
            this._orderHistoryRepository = orderHistoryRepository;
            this._orderTypeRepository = orderTypeRepository;
            this._orderContentRepository = orderContentRepository;
            this._areaRepository = areaRepository;
            this._areaPriceRepository = areaPriceRepository;
            this._orderDataRepository = orderDataRepository;
        }

        // GET: Admin/Orders/Index
        public IActionResult Index()
        {
            var listUsers = _userManager.Users.Include(x => x.UserType).Where(x => x.IsActive);
            ViewData["listOrderStatuses"] = new SelectList(_orderStatusRepository.GetAll().Result, "Id", "TitleView");
            ViewData["listAreas"] = new SelectList(_areaRepository.GetAll().Result, "Id", "Name");
            ViewData["listOrderTypes"] = new SelectList(_orderTypeRepository.GetAll().Result, "Id", "Name");
            ViewData["listTraders"] = new SelectList(listUsers.Where(x => x.UserType.TitlePrograming.Equals(UserTypeValues.TRADER)), "Id", "FullName");
            ViewData["listDrivers"] = new SelectList(listUsers.Where(x => x.UserType.TitlePrograming.Equals(UserTypeValues.DRIVER)), "Id", "FullName");
            return View();
        }

        // GET: Admin/Orders/Create
        public IActionResult Create()
        {
            ViewData["listAreas"] = new SelectList(_areaRepository.GetAll().Result, "Id", "Name");
            ViewData["listOrderTypes"] = new SelectList(_orderTypeRepository.GetAll().Result, "Id", "Name");
            ViewData["listOrderContent"] = new SelectList(_orderContentRepository.GetAll().Result, "Id", "Title");
            return View();
        }

        // POST: Admin/Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderVM model)
        {
            if (ModelState.IsValid)
            {
                var newEntity = _mapper.Map<Order>(model);
                newEntity.AllowEdit = true;
                newEntity.IsArchive = false;
                newEntity.CreateByUserId = UserId;
                var statusEntity = await _orderStatusRepository.FindFirst(x => x.TitlePrograming.Equals(OrderStatusValues.WAITING));
                newEntity.OrderStatusId = statusEntity.Id;
                /*
                * START Set SerailNumber
                */
                var lastOrder = await _orderRepository.Get().OrderByDescending(x => x.CreateAt).FirstOrDefaultAsync();
                if (lastOrder == null)
                {
                    newEntity.SerialNumber = "00000001";
                }
                else
                {
                    var newSerialNumber = ExtensionMethods.AutoIncrement(lastOrder.SerialNumber);
                    newEntity.SerialNumber = newSerialNumber;
                }
                /*
                 * END Set SerailNumber
                 */

                //START calculate total price
                float totalPrice = await _orderDataRepository.CalculateOrderPrice(
                    mainPrice: model.MainPrice.Value,
                    additionalCost: model.AdditionalCost,
                    discountedCost: model.DiscountedCost,
                    traderId: model.UserTraderId,
                    areaIdReceiver: model.AreaIdReceiver,
                    areaIdSender: model.AreaIdSender,
                    orderTypeId: model.OrderTypeId,
                    isIncludeDeliveryPrice: false);

                if (totalPrice != -1 && totalPrice != 0)
                {
                    newEntity.TotalPrice = totalPrice;
                    var result = await _orderRepository.AddAsync(newEntity);
                    if (result != null)
                    {
                        {
                            OrderHistory orderHistory = new OrderHistory
                            {
                                Title = OrderHistoryValues.AddOrderTitle,
                                Description = OrderHistoryValues.AddOrder(UserName),
                                CreateByUserId = UserId,
                                OrderId = result.OrderId,
                            };
                            await _orderHistoryRepository.AddAsync(orderHistory);
                        }
                    }
                    return Content(ShowMessage.AddSuccessResult(), "application/json");
                }
                //END calculate total price
                return Content(ShowMessage.FailedResult(), "application/json");
            }
            ViewData["listAreas"] = new SelectList(_areaRepository.GetAll().Result, "Id", "Name");
            ViewData["listOrderTypes"] = new SelectList(_orderTypeRepository.GetAll().Result, "Id", "Name");
            ViewData["listOrderContent"] = new SelectList(_orderContentRepository.GetAll().Result, "Id", "Title");
            return View(model);
        }

        // GET: Admin/Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            var newEntity = _mapper.Map<OrderVM>(model);
            ViewData["listAreas"] = new SelectList(_areaRepository.GetAll().Result, "Id", "Name");
            ViewData["listOrderTypes"] = new SelectList(_orderTypeRepository.GetAll().Result, "Id", "Name");
            ViewData["listOrderContent"] = new SelectList(_orderContentRepository.GetAll().Result, "Id", "Title");
            return View(newEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var baseEntity = await _orderRepository.Get(id);
                    PropertyCopy.Copy(model, baseEntity);
                    baseEntity.UpdateAt = DateTime.Now;
                    baseEntity.UpdateByUserId = UserId;

                    //START calculate total price
                    float totalPrice = await _orderDataRepository.CalculateOrderPrice(
                        mainPrice: model.MainPrice.Value,
                        additionalCost: model.AdditionalCost,
                        discountedCost: model.DiscountedCost,
                        traderId: model.UserTraderId,
                        areaIdReceiver: model.AreaIdReceiver,
                        areaIdSender: model.AreaIdSender,
                        orderTypeId: model.OrderTypeId,
                        isIncludeDeliveryPrice: false);

                    if (totalPrice != -1 && totalPrice != 0)
                    {
                        baseEntity.TotalPrice = totalPrice;
                        var result = await _orderRepository.UpdateAsync(baseEntity);
                        if (result != null)
                        {
                            {
                                OrderHistory orderHistory = new OrderHistory
                                {
                                    Title = OrderHistoryValues.EditOrderTitle,
                                    Description = OrderHistoryValues.EditOrder(UserName),
                                    CreateByUserId = UserId,
                                    OrderId = result.OrderId,
                                };
                                await _orderHistoryRepository.AddAsync(orderHistory);
                            }
                        }
                        return Content(ShowMessage.EditSuccessResult(), "application/json");
                    }
                    //END calculate total price
                    return Content(ShowMessage.FailedResult(), "application/json");
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
            }
            ViewData["listAreas"] = new SelectList(_areaRepository.GetAll().Result, "Id", "Name");
            ViewData["listOrderTypes"] = new SelectList(_orderTypeRepository.GetAll().Result, "Id", "Name");
            ViewData["listOrderContent"] = new SelectList(_orderContentRepository.GetAll().Result, "Id", "Title");
            return View(model);
        }


        private async Task<bool> OrderExists(int id)
        {
            return await _orderRepository.Any(e => e.OrderId == id);
        }
    }
}

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
using TayarDelivery.Models.ViewModel.Orders;
using Microsoft.AspNetCore.Authorization;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using TayarDelivery.Data.StaticModel;
using TayarDelivery.Repository.Repository.Interfaces;
using TayarDelivery.Entity.DTO;
using Newtonsoft.Json;

namespace TayarDelivery.Areas.Admin.Controllers
{
    public class BaseOrdersController : BaseController
    {
        protected readonly IBaseRepository<Order> _orderRepository;
        protected readonly IBaseRepository<OrderStatus> _orderStatusRepository;
        protected readonly IBaseRepository<OrderType> _orderTypeRepository;
        protected readonly IBaseRepository<OrderHistory> _orderHistoryRepository;
        protected readonly IBaseRepository<Area> _areaRepository;
        protected readonly IBaseRepository<AreasPrice> _areaPriceRepository;
        protected readonly IBaseRepository<PriceType> _priceTypeRepository;
        protected readonly IBaseRepository<BillTahsil> _billTahsilRepository;
        protected readonly IOrderRepository _orderDataRepository;
        protected readonly IFCMSender _iFCMSender;
        protected readonly IBaseRepository<Entity.Domins.Notification.Notification> _notificationRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        public BaseOrdersController(
            IBaseRepository<Order> orderRepository,
            IBaseRepository<OrderStatus> orderStatusRepository,
            IBaseRepository<OrderType> orderTypeRepository,
            IBaseRepository<OrderHistory> orderHistoryRepository,
            IBaseRepository<Area> areaRepository,
            IBaseRepository<AreasPrice> areaPriceRepository,
            IBaseRepository<PriceType> priceTypeRepository,
            IBaseRepository<BillTahsil> billTahsilRepository,
            IOrderRepository orderDataRepository,
            IFCMSender IFCMSender,
            IBaseRepository<Entity.Domins.Notification.Notification> notificationRepository,
            IHostingEnvironment hostingEnvironment,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper) : base(userManager, roleManager, mapper)
        {
            this._orderRepository = orderRepository;
            this._orderStatusRepository = orderStatusRepository;
            this._orderTypeRepository = orderTypeRepository;
            this._orderHistoryRepository = orderHistoryRepository;
            this._areaRepository = areaRepository;
            this._areaPriceRepository = areaPriceRepository;
            this._billTahsilRepository = billTahsilRepository;
            this._priceTypeRepository = priceTypeRepository;
            this._orderDataRepository = orderDataRepository;
            this._iFCMSender = IFCMSender;
            this._notificationRepository = notificationRepository;
            this._hostingEnvironment = hostingEnvironment;
        }

        public JsonResult AjaxData([FromBody] dynamic data)
        {
            DataTableHelper d = new DataTableHelper(data);
            string jsonString = data.ToString();
            JObject ss = JObject.Parse(jsonString);
            int areaRecerverId = (int)ss["AreaRecerverId"];
            int orderStatusId = (int)ss["OrderStatusId"];
            int orderTypeId = (int)ss["OrderTypeId"];
            string traderId = (string)ss["TraderId"];
            string driverId = (string)ss["DriverId"];
            bool isArchive = (bool)ss["IsArchive"];
            int[] StatusIds = new int[] { };
            if (ss["Status"] != null)
                StatusIds = JsonConvert.DeserializeObject<int[]>(ss["Status"].ToString());
            string dateFrom = (string)ss["DateFilterFrom"];
            string dateTo = (string)ss["DateFilterTo"];
            DateTime? dateFilterFrom = Convert.ToDateTime(dateFrom);
            DateTime? dateFilterTo = Convert.ToDateTime(dateTo);
            if (String.IsNullOrEmpty(dateFrom) && !String.IsNullOrEmpty(dateTo))
            {
                dateFilterFrom = DateTime.Now;
            }
            if (String.IsNullOrEmpty(dateTo) && !String.IsNullOrEmpty(dateFrom))
            {
                dateFilterTo = DateTime.Now;
            }
            if (UserType.Equals(UserTypeValues.TRADER))
                traderId = UserId;
            if (UserType.Equals(UserTypeValues.DRIVER))
                driverId = UserId;
            var query = _orderRepository.Filter(
              filter: x => ((x.AreaIdReceiver == areaRecerverId) || (areaRecerverId == -1)) &&
                ((x.OrderStatusId == orderStatusId) || (orderStatusId == -1)) &&
                ((x.OrderTypeId == orderTypeId) || (orderTypeId == -1)) &&
                ((x.UserTraderId.Equals(traderId)) || (traderId.Equals("-1"))) &&
                ((x.UserDriverId.Equals(driverId)) || (driverId.Equals("-1"))) &&
                (x.IsArchive == isArchive) &&
                ((x.CreateAt.Value.Date >= dateFilterFrom.Value.Date
                    && x.CreateAt.Value.Date <= dateFilterTo.Value.Date) || (dateFrom == null && dateTo == null))
                    && (d.SearchKey == null
                        || x.Title.Contains(d.SearchKey) || x.PhoneNumberReceiver.Contains(d.SearchKey)
                        || x.Address.Contains(d.SearchKey) || x.SerialNumber.Contains(d.SearchKey))
                    && (StatusIds.Contains(x.OrderStatusId.Value) || StatusIds.Count() == 0),
              orderBy: x => x.OrderByDescending(x => x.CreateAt),
              include: x =>
                x.Include(c => c.Area)
                .Include(c => c.OrderType)
                .Include(c => c.OrderStatus)
                .Include(c => c.ApplicationUserDriver)
                .Include(c => c.ApplicationUserTrader)
                .Include(c => c.ApplicationUserCreate)).ToList();

            int totalCount = query.Count();

            var items = query.Select(x => new
            {
                x.OrderId,
                x.Title,
                x.SerialNumber,
                x.NameReceiver,
                x.PhoneNumberReceiver,
                x.IsSetDriver,
                AreaName = x.Area != null ? x.Area.Name : "غير مدخل",
                OrderStatusTitle = x.OrderStatus != null ? x.OrderStatus.TitleView : "غير مدخل",
                OrderStatusColor = x.OrderStatus != null ? x.OrderStatus.Color : "#0000ff",
                OrderStatusPro = x.OrderStatus != null ? x.OrderStatus.TitlePrograming : "غير مدخل",
                OrderTypeName = x.OrderType != null ? x.OrderType.Name : "غير مدخل",
                TraderName = x.ApplicationUserTrader != null ? x.ApplicationUserTrader.FullName : "غير مدخل",
                x.AllowEdit,
                x.Address,
                x.TotalPrice,
                x.MainPrice,
                x.IsArchive,
                x.CustomerLatitude,
                x.CustomerLongitude,
                createAt = x.CreateAt.Value.ToString("MM/dd/yyyy h:mm tt"),
                createBy = x.ApplicationUserCreate.FullName,
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

        // GET: Admin/ViewBarCode/1051888
        public IActionResult ViewBarCode(string id)
        {
            return View("ViewBarCode", id);
        }

        // POST: Admin/BillPolicy
        [HttpPost]
        public async Task<IActionResult> BillPolicy(int[] id)
        {
            var listOrders = await _orderRepository.Get().Where(x => id.Contains(x.OrderId))
                .Include(c => c.Area)
                .Include(c => c.ApplicationUserTrader)
                .ThenInclude(c => c.Area)
                .Include(c => c.ApplicationUserDriver)
                .ThenInclude(c => c.Area).ToListAsync();
            if (listOrders == null)
            {
                return NotFound();
            }
            BillPolicyVM vm = new BillPolicyVM();
            vm.OrdersItems = listOrders;
            try
            {
                foreach (var item in listOrders)
                {
                    OrderHistory orderHistory = new OrderHistory
                    {
                        Title = OrderHistoryValues.BillPolicyTitle,
                        Description = OrderHistoryValues.BillPolicy(UserName),
                        CreateByUserId = UserId,
                        OrderId = item.OrderId,
                    };
                    await _orderHistoryRepository.AddAsync(orderHistory);
                }
                return new ViewAsPdf("BillPolicy", vm)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.A5,
                };
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        // POST: Admin/BillErsalia
        [HttpPost]
        public async Task<IActionResult> BillErsalia(int[] id)
        {
            var listOrders = await _orderRepository.Get().Where(x => id.Contains(x.OrderId))
                .Include(c => c.Area)
                .Include(c => c.ApplicationUserTrader)
                .ThenInclude(c => c.Area)
                .Include(c => c.ApplicationUserDriver)
                .ThenInclude(c => c.Area)
                .ToListAsync();
            if (listOrders == null)
            {
                return NotFound();
            }
            BillErsaliaVM vm = new BillErsaliaVM();
            vm.listOrders = listOrders;
            try
            {
                foreach (var item in listOrders)
                {
                    OrderHistory orderHistory = new OrderHistory
                    {
                        Title = OrderHistoryValues.BillPolicyTitle,
                        Description = OrderHistoryValues.BillPolicy(UserName),
                        CreateByUserId = UserId,
                        OrderId = item.OrderId,
                    };
                    await _orderHistoryRepository.AddAsync(orderHistory);
                }
                return new ViewAsPdf("BillErsalia", vm)
                {
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                };
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        // POST: Admin/BillTahsil
        [HttpPost]
        public async Task<IActionResult> BillTahsil(int[] id, string traderId, string noteTahsil)
        {
            if (id.Count() == 0 || String.IsNullOrEmpty(traderId))
            {
                return NotFound();
            }

            var listOrders = await _orderRepository.Get().Where(x => id.Contains(x.OrderId))
                .Include(c => c.Area)
                .Include(c => c.ApplicationUserTrader)
                .ThenInclude(c => c.Area)
                .Include(c => c.ApplicationUserDriver)
                .ThenInclude(c => c.Area)
                .ToListAsync();
            if (listOrders == null)
            {
                return NotFound();
            }
            BillTahsilVM vm = new BillTahsilVM();
            vm.listOrders = listOrders;
            vm.noteTahsil = noteTahsil;
            vm.traderItem = listOrders.FirstOrDefault().ApplicationUserTrader;
            try
            {
                ViewAsPdf pdf = new ViewAsPdf("BillTahsil", vm)
                {
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                };
                byte[] pdfData = await pdf.BuildFile(ControllerContext);
                var fileName = string.Format("BillTahsil_{0}.pdf", Guid.NewGuid().ToString().Replace("-", ""));
                var mainPath = @"\files\filesSystem\" + fileName;
                var path = _hostingEnvironment.WebRootPath + mainPath;
                System.IO.File.WriteAllBytes(path, pdfData);

                BillTahsil billTahsil = new BillTahsil
                {
                    FilePath = mainPath,
                    TotalPrice = listOrders.Sum(x => x.TotalPrice),
                    CreateByUserId = UserId,
                    NumberOfOrder = listOrders.Count,
                    TraderId = traderId,
                };
                await _billTahsilRepository.AddAsync(billTahsil);
                foreach (var item in listOrders)
                {
                    OrderHistory orderHistory = new OrderHistory
                    {
                        Title = OrderHistoryValues.BillTahsilTitle,
                        Description = OrderHistoryValues.BillTahsil(UserName),
                        CreateByUserId = UserId,
                        OrderId = item.OrderId,
                    };
                    await _orderHistoryRepository.AddAsync(orderHistory);
                }
                return pdf;
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }
        }

        [Authorize(Roles = "administrator,manager")]
        [HttpGet]
        public async Task<IActionResult> SetAsReceivedCompany(int? id)
        {
            if (id == null)
            {
                throw new Exception("Invalid Id");
            }
            var entity = await _orderRepository.Get(id);
            if (entity == null)
            {
                throw new Exception("Invalid object");
            }
            entity.AllowEdit = false;
            var status = await _orderStatusRepository.FindFirst(x => x.TitlePrograming.Equals(OrderStatusValues.RECEIVEDCOMPANY));
            entity.OrderStatusId = status.Id;
            await _orderRepository.UpdateAsync(entity);
            {
                OrderHistory orderHistory = new OrderHistory
                {
                    Title = OrderHistoryValues.ChangeStatusOrderTitle,
                    Description = OrderHistoryValues.ChangeStatusOrder(UserName, status.TitleView),
                    CreateByUserId = UserId,
                    OrderId = id.Value
                };
                await _orderHistoryRepository.AddAsync(orderHistory);
            }
            return Content(ShowMessage.EditSuccessResult(), "application/json");
        }

        [HttpPost]
        public JsonResult AjaxDataBillTahsil([FromBody] dynamic data)
        {
            DataTableHelper d = new DataTableHelper(data);

            var query = _billTahsilRepository.Filter(
                filter: x => (d.SearchKey == null || x.TotalPrice.ToString().Contains(d.SearchKey)),
                orderBy: x => x.OrderByDescending(x => x.CreateAt),
                include: x => x.Include(v => v.ApplicationUserCreate)
                    .Include(c => c.ApplicationUserTrader)).ToList();

            int totalCount = query.Count();

            var items = query.Select(x => new
            {
                x.Id,
                x.FilePath,
                x.NumberOfOrder,
                x.TotalPrice,
                CreateUserObj = x.ApplicationUserCreate != null ? new
                {
                    Id = x.ApplicationUserCreate.Id,
                    FullName = x.ApplicationUserCreate.FullName,
                    MobileNumber = x.ApplicationUserCreate.MobileNumber1
                } : null,
                TraderUserObj = x.ApplicationUserTrader != null ? new
                {
                    Id = x.ApplicationUserTrader.Id,
                    FullName = x.ApplicationUserTrader.FullName,
                    MobileNumber = x.ApplicationUserTrader.MobileNumber1
                } : null,
                createAt = x.CreateAt.Value.ToString("MM/dd/yyyy hh:mm tt"),
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

        // GET: Admin/BaseOrders/IndexBillTahsil
        public IActionResult IndexBillTahsil()
        {
            return View();
        }

        public async Task<IActionResult> OrderTimeLine(int id)
        {
            List<OrderHistory> orderHistories = await _orderHistoryRepository.Filter(
                filter: x => x.OrderId == id,
                orderBy: x => x.OrderByDescending(p => p.CreateAt),
                include: x => x.Include(p => p.Order).Include(p => p.ApplicationUserCreate)).ToListAsync();

            Order order = await _orderRepository.Get()
                .Include(x => x.ApplicationUserCreate)
                .Include(x => x.ApplicationUserDriver)
                .Include(x => x.ApplicationUserTrader)
                .Include(x => x.ApplicationUserUpdate)
                .Include(x => x.Area)
                .Include(x => x.OrderStatus)
                .Include(x => x.OrderType)
                .SingleOrDefaultAsync(x => x.OrderId == id);

            return View(new OrderTimeLineVM
            {
                Order = order,
                orderHistories = orderHistories
            });
        }

        // POST: Admin/BaseOrders/SetOrdersStatus
        [HttpPost]
        public async Task<string> SetOrdersStatus(int[] id, int orderStatusId)
        {
            try
            {
                var statusItem = await _orderStatusRepository.Get(orderStatusId);
                for (int i = 0; i < id.Count(); i++)
                {
                    Order baseitem = await _orderRepository.Get(id[i]);
                    OrderStatus orderStatus = await _orderStatusRepository.Get(baseitem.OrderStatusId);
                    baseitem.AllowEdit = false;
                    baseitem.OrderStatusId = orderStatusId;
                    await _orderRepository.UpdateAsync(baseitem);
                    {
                        OrderHistory orderHistory = new OrderHistory
                        {
                            Title = OrderHistoryValues.ChangeStatusOrderTitle,
                            Description = OrderHistoryValues.ChangeStatusOrder(UserName, statusItem.TitleView),
                            CreateByUserId = UserId,
                            OrderId = id[i]
                        };
                        await _orderHistoryRepository.AddAsync(orderHistory);
                    }
                }
                return "ok";
            }
            catch (Exception)
            {
                return "error";
            }
        }

        // POST: Admin/BaseOrders/SetOrdersStatus
        [HttpPost]
        public async Task<string> SetOrdersDriver(int[] id, string driverId)
        {
            try
            {
                if (String.IsNullOrEmpty(driverId))
                    return "error";
                var driverItem = await _userManager.FindByIdAsync(driverId);
                for (int i = 0; i < id.Count(); i++)
                {
                    int orderId = id[i];

                    string title = "طلب توصيل";
                    string body = "لديك طلب توصيل";
                    RootNotification root = new RootNotification
                    {
                        to = driverItem.FcmToken,
                        notification = new NotificationDTO
                        {
                            title = title,
                            body = body,
                        },
                        data = new DataNotification
                        {
                            title = title,
                            body = body,
                            orderId = orderId
                        }
                    };
                    bool result = await _iFCMSender.Send(root, driverItem.FcmToken);
                    if (result)
                    {
                        var entityModel = new Entity.Domins.Notification.Notification
                        {
                            IsRead = false,
                            Message = body,
                            Title = title,
                            OrderId = orderId,
                            ReceverId = driverId,
                            SenderId = UserId
                        };
                        var resultAdd = await _notificationRepository.AddAsync(entityModel);
                    }

                    {
                        OrderHistory orderHistory = new OrderHistory
                        {
                            Title = OrderHistoryValues.SetDriverToOrderTitle,
                            Description = OrderHistoryValues.SetDriverToOrder(UserName, driverItem.UserName),
                            CreateByUserId = UserId,
                            OrderId = id[i]
                        };
                        await _orderHistoryRepository.AddAsync(orderHistory);
                    }

                    var orderItem = await _orderRepository.Get(orderId);
                    orderItem.IsSetDriver = true;
                    await _orderRepository.UpdateAsync(orderItem);
                }
                return "ok";
            }
            catch (Exception)
            {
                return "error";
            }
        }

        [HttpPost]
        public async Task<float> CalculateOrderPrice(float mainPrice, float? additionalCost,
            float? discountedCost, string traderId, int areaIdReceiver, int areaIdSender, int orderTypeId)
        {
            try
            {
                float totalPrice = await _orderDataRepository.CalculateOrderPrice(
                   mainPrice: mainPrice,
                   additionalCost: additionalCost,
                   discountedCost: discountedCost,
                   traderId: traderId,
                   areaIdReceiver: areaIdReceiver,
                   areaIdSender: areaIdSender,
                   orderTypeId: orderTypeId,
                   isIncludeDeliveryPrice: false);
                return totalPrice;
            }
            catch
            {
                return 0;
            }
        }

        [HttpGet]
        public async Task<float> GetUserPriceType(string userId)
        {
            try
            {
                if (String.IsNullOrEmpty(userId))
                    return -1;
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return -1;
                if (user.PriceTypeId == null)
                    return -1;
                var priceType = await _priceTypeRepository.FindSingle(x => x.Id == user.PriceTypeId);
                return priceType.DiscountPercentage.HasValue ? priceType.DiscountPercentage.Value : 0;
            }
            catch
            {
                return -1;
            }
        }

        [HttpGet]
        public async Task<float> GetTypeAddPrice(int typeid)
        {
            try
            {
                var item = await _orderTypeRepository.Get(typeid);
                if (item == null)
                    return -1;
                return item.DiscountPercentage.HasValue ? item.DiscountPercentage.Value : 0;
            }
            catch
            {
                return -1;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ArchiveOrder(int id)
        {
            var item = await _orderRepository.Get(id);
            if (item == null)
                return Content(ShowMessage.FailedResult(), "application/json");
            item.IsArchive = !item.IsArchive;
            await _orderRepository.UpdateAsync(item);
            return Content(ShowMessage.AddSuccessResult("s: تمت عملية تحديث الارشفة بنجاح"), "application/json");
        }

    }
}

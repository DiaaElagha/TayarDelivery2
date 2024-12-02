using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TayarDelivery.API.Dto.Base;
using TayarDelivery.API.Dto.Enums;
using TayarDelivery.API.Dto.Order;
using TayarDelivery.API.Helper;
using TayarDelivery.API.Helpers;
using TayarDelivery.Data.StaticModel;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Entity.Domins.LookUp;
using TayarDelivery.Repository.Interfaces;
using TayarDelivery.Repository.Repository.Interfaces;

namespace TayarDelivery.API.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IBaseRepository<Order> _ordersRepository;
        private readonly IBaseRepository<OrderType> _orderTypeRepository;
        private readonly IBaseRepository<AreasPrice> _areaPriceRepository;
        protected readonly IBaseRepository<OrderStatus> _orderStatusRepository;
        protected readonly IBaseRepository<OrderHistory> _orderHistoryRepository;
        protected readonly IOrderRepository _orderDataRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        public OrdersController(
            IBaseRepository<Order> ordersRepository,
            IBaseRepository<OrderType> orderTypeRepository,
            IBaseRepository<AreasPrice> areaPriceRepository,
            IBaseRepository<OrderStatus> orderStatusRepository,
            IBaseRepository<OrderHistory> orderHistoryRepository,
            IOrderRepository orderDataRepository,
            IConfiguration configuration,
            IHostingEnvironment hostingEnvironment,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
            : base(configuration, userManager, mapper)
        {
            _ordersRepository = ordersRepository;
            _orderTypeRepository = orderTypeRepository;
            _areaPriceRepository = areaPriceRepository;
            _orderStatusRepository = orderStatusRepository;
            _orderHistoryRepository = orderHistoryRepository;
            _orderDataRepository = orderDataRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/Orders/GetOrders
        [HttpGet]
        [Authorize(Roles = UserTypeValues.TRADER + "," + UserTypeValues.DRIVER)]
        public async Task<IActionResult> GetOrders(int page, DateTime dateFrom = default(DateTime), DateTime dateTo = default(DateTime),
            int areaIdReceiver = -1, int orderStatusId = -1, string nameReceiver = "-1", string userDriverName = "-1", string serialNumber = "-1")
        {
            if (dateFrom.Year == 1 && dateTo.Year != 1)
            {
                dateFrom = DateTime.Now;
            }
            if (dateTo.Year == 1 && dateFrom.Year != 1)
            {
                dateTo = DateTime.Now;
            }

            var itemUser = await GetCurrentUser();
            IQueryable<Order> listOrders = _ordersRepository.Filter(
                   filter: x => (x.AreaIdReceiver == areaIdReceiver || areaIdReceiver == -1)
                           && (x.OrderStatusId == orderStatusId || orderStatusId == -1)
                           && (x.ApplicationUserDriver.FullName.Equals(userDriverName) || userDriverName.Equals("-1"))
                           && (x.NameReceiver.Equals(nameReceiver) || nameReceiver.Equals("-1"))
                           && (x.SerialNumber.Equals(serialNumber) || serialNumber.Equals("-1"))
                           && ((x.CreateAt.Value.Date >= dateFrom.Date && x.CreateAt.Value.Date <= dateTo.Date)
                           || (dateFrom.Year == 1 && dateTo.Year == 1)),
                   include: x => x.Include(c => c.OrderStatus)
                                .Include(c => c.OrderType)
                                .Include(c => c.ApplicationUserDriver)
                                .Include(c => c.Area)
                                .Include(c => c.AreaSender));

            if (itemUser.UserType.TitlePrograming.Equals(UserTypeValues.TRADER))
            {
                listOrders = listOrders.OrderByDescending(x => x.CreateAt.Value).Where(x => x.UserTraderId.Equals(itemUser.Id));
            }
            else if (itemUser.UserType.TitlePrograming.Equals(UserTypeValues.DRIVER))
            {
                listOrders = listOrders.OrderBy(x => x.CreateAt.Value).Where(x => x.UserDriverId.Equals(itemUser.Id));
            }

            var rowPerPage = 10;

            var ordersCount = await listOrders.CountAsync();

            double numberOfPages = Math.Ceiling(ordersCount / (rowPerPage * 1.0));

            var skipValue = (page - 1) * rowPerPage;

            if (page < 1 || page > numberOfPages + 1)
            {
                return GetResponse("page number not valid", false, null);
            }

            List<Order> listOrdersData = await listOrders.Skip(skipValue).Take(rowPerPage).ToListAsync();

            List<OrderTraderVM> listAreasVM = _mapper.Map<List<OrderTraderVM>>(listOrdersData);

            PagingResponse pagingResponse = new PagingResponse()
            {
                TotalPageNumber = (int)numberOfPages,
                CurrentPage = page,
                NumberOfRows = listAreasVM.Count(),
                Data = listAreasVM
            };

            return GetResponse(ResponseMessages.READ, true, pagingResponse);
        }

        // POST: api/Orders/GetOrder
        [HttpPost]
        [Authorize(Roles = UserTypeValues.TRADER + "," + UserTypeValues.DRIVER)]
        public async Task<IActionResult> GetOrder([FromQuery] int id)
        {
            var itemUser = await GetCurrentUser();
            Order order = await _ordersRepository.Get()
                    .Include(c => c.OrderStatus)
                    .Include(c => c.OrderType)
                    .Include(c => c.Area)
                    .SingleOrDefaultAsync(x => x.OrderId == id);
            OrderTraderVM orderVM = _mapper.Map<OrderTraderVM>(order);
            if (orderVM == null)
            {
                return GetResponse(ResponseMessages.FAILED);
            }
            return GetResponse(ResponseMessages.READ, true, new List<OrderTraderVM> { orderVM });
        }

        // GET: api/Orders/GetOrder/00000005
        [HttpGet("{serialNumber}")]
        [Authorize(Roles = UserTypeValues.TRADER + "," + UserTypeValues.DRIVER)]
        public async Task<IActionResult> GetOrderBySerialNumber(string serialNumber)
        {
            var itemUser = await GetCurrentUser();
            Order order = await _ordersRepository.Get()
                    .Include(c => c.OrderStatus).Include(c => c.OrderType).Include(c => c.Area).FirstOrDefaultAsync(x => x.SerialNumber == serialNumber);
            if (itemUser.UserType.TitlePrograming.Equals(UserTypeValues.TRADER))
            {
                if (order != null)
                    if (!order.UserTraderId.Equals(itemUser.Id))
                        return GetResponse(ResponseMessages.FAILED);
            }
            else if (itemUser.UserType.TitlePrograming.Equals(UserTypeValues.DRIVER))
            {
                if (order != null)
                    if (!order.UserDriverId.Equals(itemUser.Id))
                        return GetResponse(ResponseMessages.FAILED);
            }
            OrderTraderVM orderVM = _mapper.Map<OrderTraderVM>(order);
            if (orderVM == null)
            {
                return GetResponse(ResponseMessages.FAILED);
            }
            return GetResponse(ResponseMessages.READ, true, new List<OrderTraderVM> { orderVM });
        }

        // POST: api/Orders/SetOrderStatus
        [HttpPost]
        [Authorize(Roles = UserTypeValues.TRADER + "," + UserTypeValues.DRIVER)]
        public async Task<IActionResult> SetOrderStatus(int orderId, int orderStatusId)
        {
            var itemUser = await GetCurrentUser();
            Order item = await _ordersRepository.Get(orderId);
            OrderStatus itemStatus = await _orderStatusRepository.Get(orderStatusId);
            if (itemUser.UserType.TitlePrograming.Equals(UserTypeValues.TRADER))
            {
                if (itemUser.Id.Equals(item.UserTraderId))
                {
                    item.OrderStatusId = orderStatusId;
                    Order resultItem = await _ordersRepository.UpdateAsync(item);
                    OrderTraderVM orderVM = _mapper.Map<OrderTraderVM>(resultItem);
                    return GetResponse(ResponseMessages.UPDATE, true, orderVM);
                }
            }
            else if (itemUser.UserType.TitlePrograming.Equals(UserTypeValues.DRIVER))
            {
                if (itemUser.Id.Equals(item.UserDriverId))
                {
                    item.OrderStatusId = orderStatusId;
                    Order resultItem = await _ordersRepository.UpdateAsync(item);
                    OrderTraderVM orderVM = _mapper.Map<OrderTraderVM>(resultItem);
                    return GetResponse(ResponseMessages.UPDATE, true, orderVM);
                }
            }
            await ChangeStatusHistoryMethod(itemUser, orderId, itemStatus);
            return GetResponse(ResponseMessages.FAILED);
        }

        private async Task ChangeStatusHistoryMethod(ApplicationUser itemUser, int orderId, OrderStatus itemStatus)
        {
            OrderHistory orderHistory = new OrderHistory
            {
                Title = OrderHistoryValues.ChangeStatusOrderTitle,
                Description = OrderHistoryValues.ChangeStatusOrder(itemUser.UserName, itemStatus.TitleView),
                CreateByUserId = itemUser.Id,
                OrderId = orderId
            };
            await _orderHistoryRepository.AddAsync(orderHistory);
        }

        // POST: api/Orders/PostOrder
        [HttpPost]
        [Authorize(Roles = UserTypeValues.TRADER)]
        public async Task<IActionResult> PostOrder([FromBody] OrderAddDTO orderAddDTO)
        {
            var itemUser = await GetCurrentUser();

            var order = _mapper.Map<Order>(orderAddDTO);
            order.AllowEdit = true;
            order.UserTraderId = itemUser.Id;
            order.CreateByUserId = itemUser.Id;

            var statusEntity = await _orderStatusRepository.FindFirst(x => x.TitlePrograming.Equals(OrderStatusValues.WAITING));

            order.OrderStatusId = statusEntity.Id;
            /*
             * START Set SerailNumber
             */
            var lastOrder = await _ordersRepository.Get().OrderByDescending(x => x.CreateAt).FirstOrDefaultAsync();
            if (lastOrder == null)
            {
                order.SerialNumber = "00000001";
            }
            else
            {
                var newSerialNumber = Helpers.Extensions.AutoIncrement(lastOrder.SerialNumber);
                order.SerialNumber = newSerialNumber;
            }
            /*
             * END Set SerailNumber
             */

            var result = await _ordersRepository.AddAsync(order);
            if (result != null)
            {
                {
                    OrderHistory orderHistory = new OrderHistory
                    {
                        Title = OrderHistoryValues.AddOrderTitle,
                        Description = OrderHistoryValues.AddOrder(itemUser.UserName),
                        CreateByUserId = itemUser.Id,
                        OrderId = result.OrderId,
                    };
                    await _orderHistoryRepository.AddAsync(orderHistory);
                }
                return GetResponse(ResponseMessages.CREATE, true, order);
            }
            return GetResponse(ResponseMessages.FAILED);
        }

        // POST: api/Orders/UpdateOrder
        [HttpPost]
        [Authorize(Roles = UserTypeValues.TRADER)]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderAddDTO orderAddDTO)
        {
            if (ModelState.IsValid)
            {
                var itemUser = await GetCurrentUser();

                var baseEntity = await _ordersRepository.Get().Include(x => x.OrderStatus).SingleOrDefaultAsync(x => x.OrderId == id);
                PropertyCopy.Copy(orderAddDTO, baseEntity);
                baseEntity.UpdateAt = DateTime.Now;
                baseEntity.UpdateByUserId = itemUser.Id;

                if (!baseEntity.OrderStatus.TitlePrograming.Equals(OrderStatusValues.WAITING))
                {
                    return GetResponse("Cant Update Order because order status", false);
                }

                var result = await _ordersRepository.UpdateAsync(baseEntity);
                if (result != null)
                {
                    {
                        OrderHistory orderHistory = new OrderHistory
                        {
                            Title = OrderHistoryValues.EditOrderTitle,
                            Description = OrderHistoryValues.EditOrder(itemUser.UserName),
                            CreateByUserId = itemUser.Id,
                            OrderId = result.OrderId,
                        };
                        await _orderHistoryRepository.AddAsync(orderHistory);
                    }
                    return GetResponse(ResponseMessages.CREATE, true, _mapper.Map<OrderAddDTO>(baseEntity));
                }
                return GetResponse(ResponseMessages.FAILED);
            }
            return Ok(General.GetValidationErrores(ModelState));
        }

        // GET: api/Orders/UploadSignature
        [HttpPost]
        [Authorize(Roles = UserTypeValues.DRIVER)]
        public async Task<IActionResult> UploadSignature([FromForm] AddSignature model)
        {
            var itemUser = await GetCurrentUser();
            Order item = await _ordersRepository.Get(model.TaskId);
            if (item.UserDriverId.Equals(itemUser.Id))
            {
                var fileName = await ImageHelper.SaveImage(model.SignatureImage, _hostingEnvironment, "Files/SignaturesImages");
                item.FilePathTraderSignature = "/Files/SignaturesImages/" + fileName;
                item.UpdateByUserId = itemUser.Id;
                var result = await _ordersRepository.UpdateAsync(item);
                if (result != null)
                {
                    {
                        OrderHistory orderHistory = new OrderHistory
                        {
                            Title = OrderHistoryValues.AddSignatureTitle,
                            Description = OrderHistoryValues.AddSignature(itemUser.UserName),
                            CreateByUserId = itemUser.Id,
                            OrderId = result.OrderId,
                        };
                        await _orderHistoryRepository.AddAsync(orderHistory);
                    }
                    return GetResponse(ResponseMessages.CREATE, true, item.FilePathTraderSignature);
                }
            }
            return GetResponse(ResponseMessages.FAILED);
        }

        // GET: api/Orders/DeleteOrder
        //[HttpDelete("{id}")]
        //[Authorize(Roles = UserTypeValues.TRADER)]
        //public async Task<IActionResult> DeleteOrder(int id)
        //{
        //    var itemUser = await GetCurrentUser();
        //    if (itemUser.UserType.TitlePrograming.Equals(UserTypeValues.TRADER))
        //    {
        //        Order item = await _ordersRepository.Get(id);
        //        if (itemUser.Id.Equals(item.UserTraderId))
        //        {
        //            await _ordersRepository.DeleteAsync(item);
        //            {
        //                OrderHistory orderHistory = new OrderHistory
        //                {
        //                    Title = OrderHistoryValues.DeleteOrder(itemUser.UserName),
        //                    CreateByUserId = itemUser.Id,
        //                    OrderId = id
        //                };
        //                await _orderHistoryRepository.AddAsync(orderHistory);
        //            }
        //            return GetResponse(ResponseMessages.DELETE, true);
        //        }
        //    }
        //    return GetResponse(ResponseMessages.FAILED);
        //}

        // GET: api/Orders/GetMyDashBorad
        [HttpPost]
        [Authorize(Roles = UserTypeValues.TRADER + "," + UserTypeValues.DRIVER)]
        public async Task<IActionResult> GetMyDashBorad()
        {
            var itemUser = await GetCurrentUser();
            int? totalCountOrders = 0;
            int? totalCountOrdersBeingDeliverey = 0;
            int? totalCountOrdersDoneDelivered = 0;
            int? totalCountOrdersDoneDeliveredInThisMonth = 0;
            int? totalCountOrdersDoneDeliveredInThisDay = 0;

            float? sumTotalPrice = 0;
            float? sumTotalDeliveryPrice = 0;
            try
            {
                var beingDeliverey = await _orderStatusRepository.FindFirst(x => x.TitlePrograming.Equals(OrderStatusValues.BEINGDELIVERED));
                var doneDelivered = await _orderStatusRepository.FindFirst(x => x.TitlePrograming.Equals(OrderStatusValues.DONEDELIVERED));
                if (itemUser.UserType.TitlePrograming.Equals(UserTypeValues.TRADER))
                {
                    IQueryable<Order> queryableData = _ordersRepository.Get().Where(x => x.UserTraderId.Equals(itemUser.Id));
                    totalCountOrders = await queryableData.CountAsync();
                    totalCountOrdersBeingDeliverey = await queryableData.CountAsync(x => x.OrderStatusId == beingDeliverey.Id);
                    totalCountOrdersDoneDelivered = await queryableData.CountAsync(x => x.OrderStatusId == doneDelivered.Id);
                    totalCountOrdersDoneDeliveredInThisMonth = await queryableData.CountAsync(x =>
                        x.OrderStatusId == doneDelivered.Id
                        && (x.CreateAt.Value.Month == DateTime.Now.Month) && (x.CreateAt.Value.Year == DateTime.Now.Year));
                    totalCountOrdersDoneDeliveredInThisDay = await queryableData.CountAsync(x =>
                      x.OrderStatusId == doneDelivered.Id
                      && (x.CreateAt.Value.Day == DateTime.Now.Day)
                      && (x.CreateAt.Value.Month == DateTime.Now.Month)
                      && (x.CreateAt.Value.Year == DateTime.Now.Year));

                    sumTotalPrice = queryableData.Sum(x => x.TotalPrice);
                    sumTotalDeliveryPrice = queryableData.Sum(x => x.TotalPrice - x.MainPrice);
                }
                else if (itemUser.UserType.TitlePrograming.Equals(UserTypeValues.DRIVER))
                {
                    IQueryable<Order> queryableData = _ordersRepository.Get().Where(x => x.UserDriverId.Equals(itemUser.Id));
                    totalCountOrders = await queryableData.CountAsync();
                    totalCountOrdersBeingDeliverey = await queryableData.CountAsync(x => x.OrderStatusId == beingDeliverey.Id);
                    totalCountOrdersDoneDelivered = await queryableData.CountAsync(x => x.OrderStatusId == doneDelivered.Id);
                    totalCountOrdersDoneDeliveredInThisMonth = await queryableData.CountAsync(x =>
                        x.OrderStatusId == doneDelivered.Id
                        && (x.CreateAt.Value.Month == DateTime.Now.Month) && (x.CreateAt.Value.Year == DateTime.Now.Year));
                    totalCountOrdersDoneDeliveredInThisDay = await queryableData.CountAsync(x =>
                        x.OrderStatusId == doneDelivered.Id
                        && (x.CreateAt.Value.Day == DateTime.Now.Day)
                        && (x.CreateAt.Value.Month == DateTime.Now.Month)
                        && (x.CreateAt.Value.Year == DateTime.Now.Year));

                    sumTotalPrice = queryableData.Sum(x => x.TotalPrice);
                    sumTotalDeliveryPrice = queryableData.Sum(x => x.TotalPrice - x.MainPrice);
                }
            }
            catch { }
            var model = new
            {
                totalCountOrders = totalCountOrders,
                totalCountOrdersBeingDeliverey = totalCountOrdersBeingDeliverey,
                totalCountOrdersDoneDelivered = totalCountOrdersDoneDelivered,
                totalCountOrdersDoneDeliveredInThisMonth = totalCountOrdersDoneDeliveredInThisMonth,

                sumTotalPrice = sumTotalPrice,
                sumTotalDeliveryPrice = sumTotalDeliveryPrice,
            };
            return GetResponse(ResponseMessages.READ, true, model);
        }

        [HttpPost]
        [Authorize(Roles = UserTypeValues.TRADER)]
        public async Task<IActionResult> CalculateOrderPrice(CalculateOrderPriceDTO calculateDTO)
        {
            try
            {
                var itemUser = await GetCurrentUser();
                float totalPrice = await _orderDataRepository.CalculateOrderPrice(
                   mainPrice: calculateDTO.mainPrice,
                   additionalCost: calculateDTO.additionalCost,
                   discountedCost: calculateDTO.discountedCost,
                   traderId: itemUser.Id,
                   areaIdReceiver: calculateDTO.areaIdReceiver,
                   areaIdSender: calculateDTO.areaIdSender,
                   isIncludeDeliveryPrice: calculateDTO.isIncludeDeliveryPrice);
                return GetResponse(ResponseMessages.READ, true, totalPrice);
            }
            catch
            {
                return GetResponse(ResponseMessages.FAILED);
            }
        }

        [HttpPost]
        [Authorize(Roles = UserTypeValues.TRADER)]
        public async Task<IActionResult> AddNoteTrader(int orderId, string note)
        {
            if (ModelState.IsValid)
            {
                var itemUser = await GetCurrentUser();
                var entity = await _ordersRepository.Get(orderId);
                if (entity == null)
                    return NotFound();
                entity.NoteTrader = note;
                await _ordersRepository.UpdateAsync(entity);
                {
                    OrderHistory orderHistory = new OrderHistory
                    {
                        Title = OrderHistoryValues.AddNoteTitle,
                        Description = OrderHistoryValues.AddNote(itemUser.UserName),
                        CreateByUserId = itemUser.Id,
                        OrderId = orderId
                    };
                    await _orderHistoryRepository.AddAsync(orderHistory);
                }
                return GetResponse(ResponseMessages.CREATE, true);
            }
            return Ok(General.GetValidationErrores(ModelState));
        }

        [HttpPost]
        [Authorize(Roles = UserTypeValues.DRIVER)]
        public async Task<IActionResult> AddNoteDriver(int orderId, string note)
        {
            if (ModelState.IsValid)
            {
                var itemUser = await GetCurrentUser();
                var entity = await _ordersRepository.Get(orderId);
                if (entity == null)
                    return NotFound();
                entity.NoteDriver = note;
                await _ordersRepository.UpdateAsync(entity);
                {
                    OrderHistory orderHistory = new OrderHistory
                    {
                        Title = OrderHistoryValues.AddNoteTitle,
                        Description = OrderHistoryValues.AddNote(itemUser.UserName),
                        CreateByUserId = itemUser.Id,
                        OrderId = orderId
                    };
                    await _orderHistoryRepository.AddAsync(orderHistory);
                }
                return GetResponse(ResponseMessages.CREATE, true);
            }
            return Ok(General.GetValidationErrores(ModelState));
        }

        [HttpPost]
        [Authorize(Roles = UserTypeValues.DRIVER)]
        public async Task<IActionResult> UpdateLocationDriver(double longitude, double Latitude)
        {
            if (ModelState.IsValid)
            {
                var itemUser = await GetCurrentUser();
                itemUser.DriverLatitude = Latitude;
                itemUser.DriverLongitude = longitude;
                var identityResult = await _userManager.UpdateAsync(itemUser);
                if (!identityResult.Succeeded)
                    return GetResponse("Can't get user object", false);
                else
                    return GetResponse(ResponseMessages.UPDATE, true);
            }
            return Ok(General.GetValidationErrores(ModelState));
        }

        [HttpPost]
        [Authorize(Roles = UserTypeValues.TRADER)]
        public async Task<IActionResult> GetLocationDriver(string driverId)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(driverId))
                    return GetResponse("Please pass a parameters", false);
                var itemUser = await _userManager.FindByIdAsync(driverId);
                if (itemUser == null)
                    return GetResponse("Can't get user object", false);
                var result = new
                {
                    DriverLatitude = itemUser.DriverLatitude,
                    DriverLongitude = itemUser.DriverLongitude
                };
                return GetResponse(ResponseMessages.READ, true, result);
            }
            return Ok(General.GetValidationErrores(ModelState));
        }

    }
}

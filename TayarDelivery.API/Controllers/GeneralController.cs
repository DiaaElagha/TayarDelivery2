using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TayarDelivery.API.Dto.Enums;
using TayarDelivery.API.Dto.General.Entity;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Entity.Domins.LookUp;
using TayarDelivery.Repository.Interfaces;

namespace TayarDelivery.API.Controllers
{
    public class GeneralController : BaseController
    {
        private readonly IBaseRepository<Area> _areaRepository;
        private readonly IBaseRepository<OrderContent> _orderContentRepository;
        private readonly IBaseRepository<OrderStatus> _orderStatusRepository;
        private readonly IBaseRepository<OrderType> _orderTypeRepository;
        private readonly IBaseRepository<AreasPrice> _areasPriceRepository;
        private readonly IBaseRepository<PriceType> _priceTypeRepository;
        private readonly IBaseRepository<UserType> _userTypeRepository;
        private readonly IBaseRepository<CompanyInformation> _companyInfoRepository;

        public GeneralController(
            IBaseRepository<Area> areaRepository,
            IBaseRepository<OrderContent> orderContentRepository,
            IBaseRepository<OrderStatus> orderStatusRepository,
            IBaseRepository<AreasPrice> areasPriceRepository,
            IBaseRepository<OrderType> orderTypeRepository,
            IBaseRepository<PriceType> priceTypeRepository,
            IBaseRepository<UserType> userTypeRepository,
            IBaseRepository<CompanyInformation> companyInfoRepository,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
            : base(configuration, userManager, mapper)
        {
            _orderContentRepository = orderContentRepository;
            _areaRepository = areaRepository;
            _orderStatusRepository = orderStatusRepository;
            _areasPriceRepository = areasPriceRepository;
            _priceTypeRepository = priceTypeRepository;
            _userTypeRepository = userTypeRepository;
            _companyInfoRepository = companyInfoRepository;
            _orderTypeRepository = orderTypeRepository;
        }

        // GET: api/General/GetOrderStatus
        [HttpGet]
        public async Task<IActionResult> GetOrderContents()
        {
            List<OrderContent> listOrderContents = await _orderContentRepository.GetAll();
            return GetResponse(ResponseMessages.READ, true, listOrderContents);
        }

        // GET: api/General/GetOrderStatus
        [HttpGet]
        public async Task<IActionResult> GetOrderStatus()
        {
            List<OrderStatus> listOrderStatus = await _orderStatusRepository.GetAll();
            List<OrderStatusVM> listOrderStatusVM = _mapper.Map<List<OrderStatusVM>>(listOrderStatus);
            return GetResponse(ResponseMessages.READ, true, listOrderStatusVM);
        }

        // GET: api/General/GetOrderTypes
        [HttpGet]
        public async Task<IActionResult> GetOrderTypes()
        {
            List<OrderType> listOrderTypes = await _orderTypeRepository.GetAll();
            List<OrderTypeVM> listOrderTypesVM = _mapper.Map<List<OrderTypeVM>>(listOrderTypes);
            return GetResponse(ResponseMessages.READ, true, listOrderTypesVM);
        }

        // GET: api/General/GetPriceTypes
        [HttpGet]
        public async Task<IActionResult> GetPriceTypes()
        {
            List<PriceType> listEntity = await _priceTypeRepository.GetAll();
            List<PriceTypeVM> listEntityVM = _mapper.Map<List<PriceTypeVM>>(listEntity);
            return GetResponse(ResponseMessages.READ, true, listEntityVM);
        }


        // GET: api/General/GetPriceTypes
        [HttpGet]
        public async Task<IActionResult> GetAreasPrice()
        {
            List<AreasPrice> listEntity = await _areasPriceRepository.Get().Include(c => c.DealerArea).Include(c => c.ReceverArea).ToListAsync();
            List<AreasPriceVM> listEntityVM = _mapper.Map<List<AreasPriceVM>>(listEntity);
            return GetResponse(ResponseMessages.READ, true, listEntityVM);
        }

        // GET: api/General/GetAreas
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAreas()
        {
            List<Area> listAreas = await _areaRepository.GetAll();
            List<AreaVM> listAreasVM = _mapper.Map<List<AreaVM>>(listAreas);
            return GetResponse(ResponseMessages.READ, true, listAreasVM);
        }

        // GET: api/General/GetUserTypes
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserTypes()
        {
            List<UserType> listAreas = await _userTypeRepository.GetAll();
            List<UserTypeVM> listAreasVM = _mapper.Map<List<UserTypeVM>>(listAreas);
            return GetResponse(ResponseMessages.READ, true, listAreasVM);
        }

        // GET: api/General/GetCompanyInfo
        [HttpGet]
        public IActionResult GetCompanyInfo()
        {
            CompanyInformation companyInformation = _companyInfoRepository.Get().FirstOrDefault();
            return GetResponse(ResponseMessages.READ, true, companyInformation);
        }


    }
}

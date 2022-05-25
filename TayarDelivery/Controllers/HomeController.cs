using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rotativa.AspNetCore;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Entity.Domins.Home;
using TayarDelivery.Entity.Domins.LookUp;
using TayarDelivery.Models;
using TayarDelivery.Models.ViewModel.Home;
using TayarDelivery.Repository.Interfaces;

namespace TayarDelivery.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBaseRepository<ContactUs> _contactUsRepository;
        private readonly IBaseRepository<RegisterTrader> _registerTraderRepository;
        private readonly IBaseRepository<HomeInfo> _homeInfoRepository;
        private readonly IBaseRepository<CompanyInformation> _companyInformationRepository;
        private readonly IBaseRepository<Services> _servicesRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IBaseRepository<ContactUs> contactUsRepository,
           IBaseRepository<RegisterTrader> registerTraderRepository,
            IBaseRepository<HomeInfo> homeInfoRepository,
            IBaseRepository<Services> servicesRepository,
            IBaseRepository<CompanyInformation> companyInformationRepository,
            ILogger<HomeController> logger,
            IMapper mapper,
            IHostingEnvironment hostingEnvironment) : base(userManager)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
            _contactUsRepository = contactUsRepository;
            _registerTraderRepository = registerTraderRepository;
            _homeInfoRepository = homeInfoRepository;
            _servicesRepository = servicesRepository;
            _companyInformationRepository = companyInformationRepository;
        }

        public async Task<IActionResult> Index()
        {
            //string domainName = Request.Scheme + System.Uri.SchemeDelimiter + Request.Host;
            var homeInfo = _homeInfoRepository.Get().FirstOrDefault();
            var HF = new HomeInfo
            {
                MainTitle = "",
                SubTitle = "",
                MainTitleColor = "#ffffff",
                SubTitleColor = "#ffffff",
                BackgroundImage = "",
                BackgroundImageHeight = "",
                BackgroundImageWidth = "",
                Description = "",
            };

            var companyInformation = _companyInformationRepository.Get().FirstOrDefault();

            var services = await _servicesRepository.GetAll();

            var vm = new HomeVM();

            vm.HomeInfo = homeInfo == null ? HF : homeInfo;
            vm.ListServices = services;
            vm.CompanyInformation = companyInformation == null ? new CompanyInformation() : companyInformation;
            return View(vm);
        }
        public async Task<IActionResult> Register()
        {
            var companyInformation = _companyInformationRepository.Get().FirstOrDefault();
            var vm = new HomeVM();
            vm.CompanyInformation = companyInformation == null ? new CompanyInformation() : companyInformation;
            return View(vm);
        }

        [HttpPost]
        public async Task<string> RegisterAsync(HomeVM contact)
        {
            var model = _mapper.Map<RegisterTrader>(contact.RegisterVM);
            var result = await _registerTraderRepository.AddAsync(model);
            if (result != null)
                return "success";
            return "";
        }


        [HttpPost]
        public async Task<string> ContactAsync(HomeVM contact)
        {
            var model = _mapper.Map<ContactUs>(contact.ContactUsVM);
            var result = await _contactUsRepository.AddAsync(model);
            if (result != null)
                return "success";
            return "";
        }

        public IActionResult Privacy()
        {
            return new ViewAsPdf("Privacy");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    internal interface IHonstingEnvironment
    {
    }
}

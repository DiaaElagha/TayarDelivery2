using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Entity.Domins.Home;
using TayarDelivery.Entity.Domins.LookUp;
using TayarDelivery.Models.ViewModel;
using TayarDelivery.Models.ViewModel.Auth;
using TayarDelivery.Models.ViewModel.Home;
using TayarDelivery.Models.ViewModel.Users;

namespace TayarDelivery.Helper
{
    public class ModelsProfiles : Profile
    {
        public ModelsProfiles()
        {
            //Mapper For Area
            CreateMap<AreaVM, Area>().ReverseMap();
            CreateMap<AreasPriceVM, AreasPrice>().ReverseMap();

            //Mapper For Orders
            CreateMap<OrderVM, Order>().ReverseMap();
            CreateMap<OrderTraderVM, Order>().ReverseMap();
            CreateMap<OrderTypeVM, OrderType>().ReverseMap();
            CreateMap<PriceTypeVM, PriceType>().ReverseMap();
            CreateMap<OrderContentVM, OrderContent>().ReverseMap();

            //Mapper For ApplicationUser
            CreateMap<AdministratorVM, ApplicationUser>().ReverseMap();
            CreateMap<TraderVM, ApplicationUser>().ReverseMap();
            CreateMap<DriverVM, ApplicationUser>().ReverseMap();

            //Mapper For Auth
            CreateMap<RoleVM, Role>().ReverseMap();
            CreateMap<LinkVM, Link>().ReverseMap();

            //Mapper For Home
            CreateMap<ContactUsVM, ContactUs>().ReverseMap();
            CreateMap<RegisterVM, RegisterTrader>().ReverseMap();
            CreateMap<ServiceVM, Services>().ReverseMap();

        }
    }
}

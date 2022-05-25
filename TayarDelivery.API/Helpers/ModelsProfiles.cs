using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TayarDelivery.API.Dto.Auth;
using TayarDelivery.API.Dto.General.Auth;
using TayarDelivery.API.Dto.General.Entity;
using TayarDelivery.API.Dto.Order;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Entity.Domins.LookUp;
using TayarDelivery.Entity.Domins.User;

namespace TayarDelivery.Helper
{
    public class ModelsProfiles : Profile
    {

        public ModelsProfiles()
        {

            //Mapper For ApplicationUser
            CreateMap<UserProfileDto, ApplicationUser>().ReverseMap();
            CreateMap<UserProfileSettingsDto, UserProfile>().ReverseMap();
            CreateMap<RegisterDto, ApplicationUser>()
                .ForSourceMember(x => x.Password, opt => opt.DoNotValidate()).ReverseMap();

            //Mapper For General
            CreateMap<UserTypeVM, UserType>().ReverseMap();
            CreateMap<AreaVM, Area>().ReverseMap();
            CreateMap<AreasPriceVM, AreasPrice>().ReverseMap();
            CreateMap<PriceTypeVM, PriceType>().ReverseMap();
            CreateMap<OrderStatusVM, OrderStatus>().ReverseMap();
            CreateMap<OrderTypeVM, OrderType>().ReverseMap();
            CreateMap<CompanyInfoVM, CompanyInformation>().ReverseMap();

            //Mapper For Order
            CreateMap<OrderAddDTO, Order>().ReverseMap();
            CreateMap<OrderTraderVM, Order>().ReverseMap().ForMember(dest =>
                dest.FilePathTraderSignature,
                opt => opt.MapFrom(src => src.FilePathTraderSignature));

        }
    }
}

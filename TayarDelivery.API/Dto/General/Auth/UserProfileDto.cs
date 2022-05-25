using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TayarDelivery.API.Dto.General.Entity;
using TayarDelivery.Entity.Domins;

namespace TayarDelivery.API.Dto.Auth
{
    public class UserProfileDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string MobileNumber1 { get; set; }
        public string MobileNumber2 { get; set; }

        public int? AreaId { get; set; }
        [ForeignKey(nameof(AreaId))]
        public AreaVM Area { get; set; }

        public int? PriceTypeId { get; set; }
        [ForeignKey(nameof(PriceTypeId))]
        public PriceTypeVM PriceType { get; set; }

        public int? UserTypeID { get; set; }
        [ForeignKey(nameof(UserTypeID))]
        public UserTypeVM UserType { get; set; }

        public double? DriverLongitude { get; set; }
        public double? DriverLatitude { get; set; }

        public string FcmToken { get; set; }

        public DateTime CreateAt { get; set; }
    }
}

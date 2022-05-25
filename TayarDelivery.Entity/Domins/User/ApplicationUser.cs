using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TayarDelivery.Entity.Domins.LookUp;

namespace TayarDelivery.Entity.Domins
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            CreateAt = DateTime.Now;
        }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string MobileNumber1 { get; set; }
        public string MobileNumber2 { get; set; }
        [ScaffoldColumn(false)]
        public bool IsActive { get; set; }

        public int? AreaId { get; set; }
        [ForeignKey(nameof(AreaId))]
        public Area Area { get; set; }

        public int? UserTypeID { get; set; }
        [ForeignKey(nameof(UserTypeID))]
        public UserType UserType { get; set; }

        public int? PriceTypeId { get; set; }
        [ForeignKey(nameof(PriceTypeId))]
        public PriceType PriceType { get; set; }

        public double? DriverLongitude { get; set; }
        public double? DriverLatitude { get; set; }

        public string DriverCarModel { get; set; }
        public string DriverCarType { get; set; }
        public string DriverCarNumber { get; set; }

        public string FcmToken { get; set; }
        public string AccessToken { get; set; }
        public int? ConfirmMobileCode { get; set; }
        public int? ForgetPasswordCode { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? CreateAt { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? UpdateAt { get; set; }

    }
}

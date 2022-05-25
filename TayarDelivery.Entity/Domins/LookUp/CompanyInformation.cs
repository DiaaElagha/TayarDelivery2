using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TayarDelivery.Entity.Domins.LookUp
{
    public class CompanyInformation
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string LicensedNumber { get; set; }
        public string FilePathCompanyLogo { get; set; }
        public string WorkTime { get; set; }
        public string WhatsUpNumber { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string SupportNumber { get; set; }
        public string SupportEmail { get; set; }
        public string Address { get; set; }
        public string facebookLink { get; set; }
        public string twitterLink { get; set; }
        public string instgramLink { get; set; }
        public string googlePlayAppLink { get; set; }
        public string appStoreAppLink { get; set; }
        public double? CompanyLongitude { get; set; }
        public double? CompanyLatitude { get; set; }
    }
}

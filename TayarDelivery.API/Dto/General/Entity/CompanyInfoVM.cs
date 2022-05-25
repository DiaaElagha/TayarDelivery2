using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.API.Dto.General.Entity
{
    public class CompanyInfoVM
    {
        public string CompanyName { get; set; }
        public string FilePathCompanyLogo { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
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

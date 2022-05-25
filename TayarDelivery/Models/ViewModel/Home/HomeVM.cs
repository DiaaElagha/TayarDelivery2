using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TayarDelivery.Entity.Domins.Home;
using TayarDelivery.Entity.Domins.LookUp;

namespace TayarDelivery.Models.ViewModel.Home
{
    public class HomeVM
    {
        public HomeInfo HomeInfo { get; set; } = new HomeInfo();
        public ContactUsVM ContactUsVM { get; set; } = new ContactUsVM();
        public RegisterVM RegisterVM { get; set; } = new RegisterVM();
        public CompanyInformation CompanyInformation { get; set; } = new CompanyInformation();
        public List<Services> ListServices { get; set; } = new List<Services>();
    }
}

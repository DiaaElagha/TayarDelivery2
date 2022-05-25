using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TayarDelivery.Entity.Domins.Home
{
    public class RegisterTrader
    {
        [Key]
        public int Id { get; set; }
        public string NameSocial { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
    }
}

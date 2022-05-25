using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.Data.StaticModel
{
    public class UserTypeValues
    {
        public const string ADMINISTRATOR = "administrator";
        public const string MANAGER = "manager";
        public const string TRADER = "trader";
        public const string DRIVER = "driver";
        public const string ACCOUNTANT = "accountant";
    }
}

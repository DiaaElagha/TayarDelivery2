using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TayarDelivery.API.Helpers
{
    public static class Extensions
    {
        public static string GetDescription(this Enum e)
        {
            var attribute =
                e.GetType()
                    .GetTypeInfo()
                    .GetMember(e.ToString())
                    .FirstOrDefault(member => member.MemberType == MemberTypes.Field)
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .SingleOrDefault()
                    as DescriptionAttribute;
            return attribute?.Description ?? e.ToString();
        }

        public static float? GetTotalPriceOrder(float? price, float? mainPrice, float? discount, float? plus)
        {
            var DiscountAmount = price - discount;
            var PlusAmount = price + plus;
            var PriceFinal = (price + PlusAmount) - DiscountAmount;
            return (PriceFinal + mainPrice);
        }

        public static string AutoIncrement(string lastSerialNum)
        {
            int id = Convert.ToInt32(lastSerialNum);
            id = id + 1;
            string autoId = String.Format("{0:00000000}", id);
            return autoId;
        }

    }
}

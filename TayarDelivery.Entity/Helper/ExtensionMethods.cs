using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.Entity.Helper
{
    public static class ExtensionMethods
    {
        public static string GetDate(DateTime date)
        {
            TimeSpan span = (DateTime.Now - date);
            var days = span.Days;
            var hours = span.Hours;
            var minutes = span.Minutes;
            var seconds = span.Seconds;
            if (minutes == 0 && days == 0 && hours == 0)
            {
                return String.Format("{0} seconds", seconds);
            }
            if (days == 0 && hours == 0)
            {
                return String.Format("{0} minutes", minutes);
            }
            if (days == 0 && hours != 0)
            {
                return String.Format("{0} hours, {1} minutes", hours, minutes);
            }
            if (days != 0)
            {
                return String.Format("{0} days, {1} hours", days, hours);
            }
            return String.Format("{0} days, {1} hours, {2} minutes", days, hours, minutes);
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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.Data.StaticModel
{
    public class OrderStatusValues
    {
        //انتظار
        public const string WAITING = "waiting";
        //تم الاستلام بالشركة
        public const string RECEIVEDCOMPANY = "receivedcompany";
        //قيد التوصيل
        public const string BEINGDELIVERED = "beingdelivered";
        //تم التوصيل
        public const string DONEDELIVERED = "donedelivered";
        //ملغي
        public const string CANCELED = "canceled";
        //مرفوض
        public const string REJECTED = "rejected";
    }
}

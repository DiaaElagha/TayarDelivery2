using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.Data.StaticModel
{
    public class OrderHistoryValues
    {
        public const string AddOrderTitle = "إنشاء طرد";
        public const string EditOrderTitle = "تعديل طرد";
        public const string DeleteOrderTitle = "حذف طرد";
        public const string SetDriverToOrderTitle = "تعيين سائق";
        public const string ChangeStatusOrderTitle = "تغيير حالة";
        public const string AddNoteTitle = "إضافة ملاحظات";
        public const string AddSignatureTitle = "إضافة توقيع";
        public const string BillPolicyTitle = "طباعة بوليصة";
        public const string BillErsaliaTitle = "طباعة إرسالية";
        public const string BillTahsilTitle = "طباعة كشف تحصيل";

        public static string AddOrder(string UserName)
        {
            string result = String.Format(" قام {0} بإنشاء الطرد ", UserName);
            return result;
        }

        public static string EditOrder(string UserName)
        {
            string result = String.Format(" قام {0} بتعديل بيانات الطرد ", UserName);
            return result;
        }

        public static string DeleteOrder(string UserName)
        {
            string result = String.Format(" قام {0} بحذف الطرد ", UserName);
            return result;
        }

        public static string SetDriverToOrder(string UserName,string driverName)
        {
            string result = String.Format(" قام {0} بتعيين السائق {1} لتوصيل الطرد ", UserName, driverName);
            return result;
        }

        public static string ChangeStatusOrder(string UserName, string statusName)
        {
            string baseString = " قام {0} بتغيير حالة الطرد الى ";
            string statusString =
                "<span style='font-size: 13px;margin-right: 5px;' class='label label-sm text-white rounded label-primary bg-primary label-inline font-weight-bold pr-2 pl-2'> " +
                statusName +
                " </span>";
            string result = String.Format(baseString + statusString, UserName);
            return result;
        }

        public static string AddNote(string UserName)
        {
            string result = String.Format(" قام {0} بإضافة ملاحظات على الطرد ", UserName);
            return result;
        }

        public static string AddSignature(string UserName)
        {
            string result = String.Format(" قام {0} بإضافة توقيع على الطرد ", UserName);
            return result;
        }

        public static string BillPolicy(string UserName)
        {
            string result = String.Format(" قام {0} بطباعة بوليصة ", UserName);
            return result;
        }

        public static string BillErsalia(string UserName)
        {
            string result = String.Format(" قام {0} بطباعة ارسالية ", UserName);
            return result;
        }
        public static string BillTahsil(string UserName)
        {
            string result = String.Format(" قام {0} بطباعة كشف تحصيل ", UserName);
            return result;
        }

    }
}

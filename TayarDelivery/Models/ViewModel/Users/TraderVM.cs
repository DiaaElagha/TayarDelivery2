using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.Models.ViewModel.Users
{
    public class TraderVM
    {
        [Required(ErrorMessage = "يرجى ادخال الاسم")]
        [Display(Name = "الاسم كامل")]
        public string FullName { get; set; }

       // [Remote(action: "VerifyUserName", controller: "Validation")]
        [Required(ErrorMessage = "يرجى ادخال اسم المستخدم")]
        [Display(Name = "اسم المستخدم للدخول")]
        public string UserName { get; set; }

        [Display(Name = "البريد الالكتروني")]
        [DataType(DataType.EmailAddress, ErrorMessage = "الرجاء ادخال صيغة صحيحة")]
        public string Email { get; set; }

        [Display(Name = "العنوان")]
        public string Address { get; set; }

        [Display(Name = "رقم الجوال")]
        [RegularExpression(@"^([0|\+[0-9]{1,5})?([0-9]{10})$", ErrorMessage = "يرجى ادخال رقم جوال صالح")]
        public string MobileNumber1 { get; set; }

        [Display(Name = "رقم الجوال 2")]
        [RegularExpression(@"^([0|\+[0-9]{1,5})?([0-9]{10})$", ErrorMessage = "يرجى ادخال رقم جوال صالح")]
        public string MobileNumber2 { get; set; }

        [Display(Name = "الفعالية")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "يرجى ادخال المنطقة")]
        [Display(Name = "منطقة التاجر")]
        public int? AreaId { get; set; }

    }
}

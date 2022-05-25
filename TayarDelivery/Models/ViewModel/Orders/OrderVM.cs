using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.Models.ViewModel
{
    public class OrderVM
    {
        [Required(ErrorMessage = "يرجى ادخال المحتوى")]
        [Display(Name = "محتوى الطرد")]
        public int OrderContentId { get; set; }

        [Required(ErrorMessage = "يرجى ادخال الاسم")]
        [Display(Name = "اسم المستقبل")]
        public string NameReceiver { get; set; }

        [Required(ErrorMessage = "يرجى ادخال الجوال")]
        [Display(Name = "رقم الجوال المستقبل")]
        [RegularExpression(@"^([0|\+[0-9]{1,5})?([0-9]{10})$", ErrorMessage = "يرجى ادخال رقم جوال صالح")]
        public string PhoneNumberReceiver { get; set; }

        [Display(Name = "رقم الجوال المستقبل 2")]
        [RegularExpression(@"^([0|\+[0-9]{1,5})?([0-9]{10})$", ErrorMessage = "يرجى ادخال رقم جوال صالح")]
        public string PhoneNumberReceiver2 { get; set; }

        [Display(Name = "عنوان المستقبل")]
        public string Address { get; set; }

        [Display(Name = "تفاصيل الطرد")]
        public string Description { get; set; }

        [Required(ErrorMessage = "يرجى ادخال السعر")]
        [Display(Name = "السعر الاساسي")]
        [Range(0, float.MaxValue, ErrorMessage = "يرجى ادخال قيمة صالحة")]
        public float? MainPrice { get; set; }

        [Display(Name = "التكلفة الإضافية")]
        [Range(0, float.MaxValue, ErrorMessage = "يرجى ادخال قيمة صالحة")]
        public float? AdditionalCost { get; set; }

        [Display(Name = "التكلفة المخصومة")]
        [Range(0, float.MaxValue, ErrorMessage = "يرجى ادخال قيمة صالحة")]
        public float? DiscountedCost { get; set; }

        [Required(ErrorMessage = "يرجى ادخال المنطقة")]
        [Display(Name = "منطقة المستقبل")]
        public int AreaIdReceiver { get; set; }

        [Required(ErrorMessage = "يرجى ادخال المرسل")]
        [Display(Name = "منطقة المرسل")]
        public int AreaIdSender { get; set; }

        [Display(Name = "اسم التاجر")]
        [Required(ErrorMessage = "يرجى ادخال التاجر")]
        public string UserTraderId { get; set; }

        [Required(ErrorMessage = "يرجى ادخال النوع")]
        [Display(Name = "نوع الطرد")]
        public int OrderTypeId { get; set; }
    }
}

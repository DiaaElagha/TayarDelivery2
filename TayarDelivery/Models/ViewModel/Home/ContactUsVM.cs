using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.Models.ViewModel.Home
{
    public class ContactUsVM
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        [RegularExpression(@"^[a-zA-Z'' '\u0621-\u064A\s]+$", ErrorMessage = "يرجى عدم ادخال رموز او ارقام")]
        public string Name { get; set; }

        [RegularExpression(@"^(\d{9,15})$", ErrorMessage = "يرجى ادخال صيغة صحيحة")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "البريد الالكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "يرجى ادخال صيغة صحيحة")]
        public string Email { get; set; }

        [Required(ErrorMessage = "عنوان الرسالة مطلوب")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "الرسالة مطلوبة")]
        public string Messege { get; set; }
    }
}

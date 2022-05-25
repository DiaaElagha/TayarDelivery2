using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.Models.ViewModel.Home
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        public string NameSocial { get; set; }

        [Required(ErrorMessage = "البريد الالكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "يرجى ادخال صيغة صحيحة")]
        public string Email { get; set; }

        [Required(ErrorMessage = "الاسم كامل مطلوب")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "المدينة مطلوبة")]
        public string City { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^([0|\+[0-9]{1,5})?([0-9]{10})$", ErrorMessage = "يرجى ادخال رقم جوال صالح")]
        public string Phone { get; set; }

    }
}

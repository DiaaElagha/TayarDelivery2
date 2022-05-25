using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.Models.ViewModel
{
    public class OrderTypeVM
    {
        [Required(ErrorMessage = "يرجى ادخال الاسم")]
        [Display(Name = "اسم النوع")]
        public string Name { get; set; }

        [Required(ErrorMessage = "يرجى ادخال القيمة")]
        [Display(Name = "الزيادة")]
        [Range(0, 1000, ErrorMessage = "يرجى ادخال قيمة صالحة")]
        public float DiscountPercentage { get; set; }
    }
}

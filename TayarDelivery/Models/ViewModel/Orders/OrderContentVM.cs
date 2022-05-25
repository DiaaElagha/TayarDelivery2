using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.Models.ViewModel
{
    public class OrderContentVM
    {
        [Required(ErrorMessage = "يرجى ادخال المحتوى")]
        [Display(Name = "اسم المحتوى")]
        public string Title { get; set; }

        [Display(Name = "الوصف")]
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.Models.ViewModel
{
    public class AreasPriceVM
    {

        [Required(ErrorMessage = "يرجى ادخال المنطقة")]
        [Display(Name = "منطقة التاجر")]
        public int DealerAreaId { get; set; }

        [Required(ErrorMessage = "يرجى ادخال المنطقة")]
        [Display(Name = "منطقة المستلم")]
        public int ReceverAreaId { get; set; }

        [Required(ErrorMessage = "يرجى ادخال السعر")]
        [Display(Name = "سعر التوصيل")]
        [Range(0, float.MaxValue, ErrorMessage = "يرجى ادخال قيمة صالحة")]
        public float Price { get; set; }

        [Required(ErrorMessage = "يرجى ادخال السعر")]
        [Display(Name = "سعر الخصم عند الارجاع")]
        [Range(0, float.MaxValue, ErrorMessage = "يرجى ادخال قيمة صالحة")]
        public float? DiscountPriceWhenReturn { get; set; }
        
        [Display(Name = "يمكن الخصم!")]
        [ScaffoldColumn(false)]
        public bool CanDiscount { get; set; }
    }
}

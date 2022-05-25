using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.Models.ViewModel
{
    public class AreaVM
    {
        [Required(ErrorMessage = "يرجى ادخال الاسم")]
        [Display(Name = "اسم المنطقة")]
        public string Name { get; set; }

        [Display(Name = "خط الطول")]
        [Range(double.MinValue, double.MaxValue, ErrorMessage = "يرجى ادخال قيمة صالحة")]
        public double? Longitude { get; set; }

        [Display(Name = "خط العرض")]
        [Range(double.MinValue, double.MaxValue, ErrorMessage = "يرجى ادخال قيمة صالحة")]
        public double? Latitude { get; set; }
    }
}

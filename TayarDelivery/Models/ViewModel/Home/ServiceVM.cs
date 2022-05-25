using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TayarDelivery.Models.ViewModel.Home
{
    public class ServiceVM
    {
        [Required(ErrorMessage = "العنوان مطلوب")]
        [Display(Name = "العنوان")]
        public string Name { get; set; }

        [Display(Name = "الوصف")]
        public string Description { get; set; }
        [Display(Name = "الايقونة")]
        public string IconName { get; set; }
    }
}

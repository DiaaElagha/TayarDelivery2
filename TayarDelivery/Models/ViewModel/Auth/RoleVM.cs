using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.Models.ViewModel.Auth
{
    public class RoleVM
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        [Display(Name = "اسم الصلاحية")]
        public string Title { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "الفعالية")]
        public bool IsActive { get; set; }
    }
}

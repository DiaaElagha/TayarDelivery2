using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.Models.ViewModel.Orders
{
    public class AddNoteVM
    {
        [Required(ErrorMessage = "الملاحظات مطلوبة")]
        [Display(Name = "ادخل ملاحظاتك حول الطلب")]
        public string text { get; set; }
    }
}

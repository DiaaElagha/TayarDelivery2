using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.Models.ViewModel.Mail
{
    public class SendNotficationVM
    {
        [Required(ErrorMessage = "يرجى ادخال العنوان")]
        [Display(Name = "عنوان الاشعار")]
        public string Title { get; set; }

        [Display(Name = "نص الاشعار")]
        public string Messege { get; set; }

        [Required(ErrorMessage = "يرجى ادخال المستقبلين")]
        [Display(Name = "اسم المستقبلين")]
        public string[] ReceversId { get; set; }
    }
}

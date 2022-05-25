using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TayarDelivery.Entity.Domins.Notification
{
    public class MessageSMS
    {
        public MessageSMS()
        {
            SendDateAt = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public String Title { get; set; }
        public String Message { get; set; }
        public DateTime? SendDateAt { get; set; }
        public string ReceverId { get; set; }
        [ForeignKey(nameof(ReceverId))]
        public ApplicationUser ApplicationUserRecever { get; set; }
    }
}

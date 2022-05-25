using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TayarDelivery.Entity.Domins.Notification
{
    public class Notification
    {
        public Notification()
        {
            SendDateAt = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public String Title { get; set; }
        public String Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime? SendDateAt { get; set; }

        public bool? Status { get; set; }

        public int? OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

        public string ReceverId { get; set; }
        [ForeignKey(nameof(ReceverId))]
        public ApplicationUser ApplicationUserRecever { get; set; }

        public string SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public ApplicationUser ApplicationUserSender { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TayarDelivery.Repository.Repository.Interfaces
{
    public interface IFCMSender
    {
        Task<bool> Send(object notification, string token);
    }
}

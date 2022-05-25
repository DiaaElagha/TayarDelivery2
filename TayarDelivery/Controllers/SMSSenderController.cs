using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TayarDelivery.Repository.Repository.Interfaces;

namespace TayarDelivery.Controllers
{
    public class SMSSenderController : Controller
    {
        private ISMSSender sender;

        public SMSSenderController(ISMSSender sender)
        {
            this.sender = sender;
        }

        public async Task<IActionResult> TestAsync()
        {
            bool result = await sender.SendSMS();
            return Ok("");
        }

    }
}

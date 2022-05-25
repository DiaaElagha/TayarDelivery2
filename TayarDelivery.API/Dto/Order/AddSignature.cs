using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.API.Dto.Order
{
    public class AddSignature
    {
        [Required]
        public int TaskId { get; set; }

        [FromForm(Name = "SignatureImage")]
        [Required]
        public IFormFile SignatureImage { get; set; }
    }
}

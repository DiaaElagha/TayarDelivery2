using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TayarDelivery.API.Dto.Base;
using TayarDelivery.API.Dto.Enums;
using TayarDelivery.API.Helpers;
using TayarDelivery.Entity.Domins;

namespace TayarDelivery.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected UserManager<ApplicationUser> _userManager;
        protected readonly IConfiguration _configuration;
        protected IMapper _mapper;
        protected APIResponse _response;

        public BaseController(IConfiguration configuration, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _response = new APIResponse();
        }

        [NonAction]
        public IActionResult GetErrorResponse()
        {
            _response.Status = false;
            _response.Message = "Sorry! an error occurred, please call technical support.";
            return Ok(_response);
        }

        [NonAction]
        public IActionResult GetResponse(ResponseMessages responseType)
        {
            _response.Status = false;
            _response.Message = responseType.GetDescription();
            return Ok(_response);
        }

        [NonAction]
        public async Task<ApplicationUser> GetCurrentUser()
        {
            string userId = User.FindFirst("suserid").Value;
            var userItem = await _userManager.Users
                .Include(x => x.UserType).Include(x => x.PriceType).Include(x => x.Area).SingleOrDefaultAsync(x => x.Id.Equals(userId));
            return userItem;
        }

        [NonAction]
        public IActionResult GetResponse(ResponseMessages responseType, bool status)
        {
            _response.Status = status;
            _response.Message = responseType.GetDescription();
            return Ok(_response);
        }

        [NonAction]
        public IActionResult GetResponse(ResponseMessages responseType, bool status, Object data)
        {
            _response.Status = status;
            _response.Message = responseType.GetDescription();
            _response.Data = data;
            return Ok(_response);
        }

        [NonAction]
        public IActionResult GetResponse(string message, bool status, Object data)
        {
            _response.Status = status;
            _response.Message = message;
            _response.Data = data;
            return Ok(_response);
        }

        [NonAction]
        public IActionResult GetResponse(string responseType, bool status)
        {
            _response.Status = status;
            _response.Message = responseType;
            return Ok(_response);
        }

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TayarDelivery.API.Dto.Base;

namespace TayarDelivery.API.Helpers
{
    public class General
    {
        public static APIResponse GetValidationErrores(ModelStateDictionary ModelState)
        {
            APIResponse response = new APIResponse();

            response.Status = false;
            response.Message = "Some Filed Required";

            var errorList = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            response.Data = errorList;
            return response;
        }

        public static long ConvertToTimestamp(DateTime value)
        {
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (long)span.TotalSeconds;
        }

        public static int GenerateRandomNo()
        {
            //int _min = 1000;
            //int _max = 9999;
            //Random _rdm = new Random();
            return 123456;
        }
    }
}

using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TayarDelivery.Repository.Repository.Interfaces;

namespace TayarDelivery.Repository.Repository.Repositores
{
    public class SMSSender : ISMSSender
    {
        private string BASE_URL = "http://api.beecell.com/index.php";
        public async Task<bool> SendSMS()
        {
            NameValueCollection data = new NameValueCollection()
            {
               { "r", "api/PutMT" },
               { "msisdn", "972597079557" },
               { "shortCode", "374410" },
               { "msg", "Test Test" },
               { "opCode", "4" },
               { "countryCode", "972" },
               { "sender", "Diaa" },
               { "service", "7019" },
               { "lang", "1" },
            };

            string queryString = ToQueryString(data);
            string URL = BASE_URL + queryString;

            RestClient client = new RestClient();
            RestRequest request = new RestRequest(URL, Method.POST);
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {

                return response.IsSuccessful;
            }
            return response.IsSuccessful;
        }


        private string ToQueryString(NameValueCollection queryData)
        {
            var array = (from key in queryData.AllKeys
                         from value in queryData.GetValues(key)
                         select string.Format(CultureInfo.InvariantCulture, "{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
                .ToArray();
            return "?" + string.Join("&", array);
        }


    }
}

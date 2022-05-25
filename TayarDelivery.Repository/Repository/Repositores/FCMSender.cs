using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TayarDelivery.Data.Data;
using TayarDelivery.Entity.DTO;
using TayarDelivery.Repository.Repository.Interfaces;

namespace TayarDelivery.Repository.Repository.Repositores
{
    public class FCMSender : IFCMSender
    {
        private readonly Lazy<HttpClient> LazyHttp;
        private readonly ApplicationDbContext _Context;

        public FCMSender(ApplicationDbContext context)
        {
            LazyHttp = new Lazy<HttpClient>();
            _Context = context;
        }

        public async Task<bool> Send(object notification, string token)
        {
            var jsonObject = JObject.FromObject(notification);
            jsonObject.Remove("to");
            jsonObject.Add("to", JToken.FromObject(token));
            var json = jsonObject.ToString();
            try
            {
                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, FCMSetting.FcmUrl))
                {
                    httpRequest.Headers.TryAddWithoutValidation("Authorization", String.Format("key={0}", FCMSetting.ServerKey));
                    httpRequest.Headers.TryAddWithoutValidation("Sender", String.Format("id={0}", FCMSetting.SenderId));
                    httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var response = await LazyHttp.Value.SendAsync(httpRequest))
                    {
                        response.EnsureSuccessStatusCode();
                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return false;
        }

        public void Dispose()
        {
            if (LazyHttp.IsValueCreated)
            {
                LazyHttp.Value.Dispose();
            }
        }
    }
}

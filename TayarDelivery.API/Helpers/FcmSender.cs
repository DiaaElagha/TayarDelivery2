using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TayarDelivery.API.Helpers
{
    public class FcmSender
    {
        private readonly string fcmUrl = "https://fcm.googleapis.com/fcm/send";
        public static string serverKey;
        public static string senderId;
        private readonly Lazy<HttpClient> lazyHttp = new Lazy<HttpClient>();


        public void Dispose()
        {
            if (lazyHttp.IsValueCreated)
            {
                lazyHttp.Value.Dispose();
            }
        }

        public async Task SendAsync(string deviceId, object payload)
        {

            var jsonObject = JObject.FromObject(payload);
            jsonObject.Remove("to");
            jsonObject.Add("to", JToken.FromObject(deviceId));
            var json = jsonObject.ToString();

            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, fcmUrl))
            {
                httpRequest.Headers.Add("Authorization", $"key = {serverKey}");
                httpRequest.Headers.Add("Sender", $"id = {senderId}");
                httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var response = await lazyHttp.Value.SendAsync(httpRequest))
                {
                    response.EnsureSuccessStatusCode();
                }

            };
        }

    }
}

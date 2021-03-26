using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gear.helper
{
    class APICall
    {
        static string ApiUrl = "https://sgpc-iot.krabd.com";
        Uri baseAddress = new Uri(ApiUrl);
        string apiurl;
        string postdata;
        string isLogin;
       // public ErrorMessageData errorMessage;
        //public Object resultObject;

        public APICall(string apiurl, string postdata, string islogin)
        {
            this.apiurl = apiurl;
            this.postdata = postdata;
            this.isLogin = islogin;
            //errorMessage = null;
        }
        public T APICallResult<T>()
        {
            try
            {
                var client = new HttpClient { BaseAddress = baseAddress };
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var req = new HttpRequestMessage(HttpMethod.Get, apiurl);
                req.Content = new StringContent(postdata, Encoding.UTF8, "application/json");
                string stringObtained = "";               
                Task<string> task = Task.Run(async () => await Threading(client, req));
                task.Wait();
                stringObtained = task.Result;
                //var jsonObtained = Regex.Unescape(stringObtained);
                var resultJSON = stringObtained;
                T resultObject;//Generic type object
                try
                {
                    resultObject = JsonConvert.DeserializeObject<T>(resultJSON);
                    return resultObject;
                }
                catch (Exception ex)
                {
                    
                    return default(T);
                }
            }
            catch (Exception ex)
            {
              
                return default(T);
            }
        }
        async Task<string> Threading(HttpClient client, HttpRequestMessage req)
        {
            var resp = await client.SendAsync(req);
            resp.EnsureSuccessStatusCode();
            string stringObtained = await resp.Content.ReadAsStringAsync();
            return stringObtained;
        }
    }
}

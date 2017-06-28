using HackAtHome.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HackAtHome.SAL
{
    public class Security
    {
        private string _webAPIBaseAddress = "https://ticapacitacion.com/hackathome/";
           
        public async Task<ResultInfo> AuthenticateAsyc(
            string studentEmail
            , string studentPassword)
        {
            ResultInfo result = null;

            string eventID = "xamarin30";
            string requestUri = "api/evidence/Authenticate";

            UserInfo user = new UserInfo
            {
                Email = studentEmail
                ,
                Password = studentPassword
                ,
                EventID = eventID
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_webAPIBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var jsonUserInfo = JsonConvert.SerializeObject(user);

                    HttpResponseMessage response =
                        await client.PostAsync(
                            requestUri
                            , new StringContent(jsonUserInfo.ToString()
                            , Encoding.UTF8
                            , "application/json"));

                    var resultWebAPI = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResultInfo>(resultWebAPI);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HackAtHome.Entities;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace HackAtHome.SAL
{
    public class EvidenceService
    {
        private string _webAPIBaseAddress = "https://ticapacitacion.com/hackathome/";

        public async Task<List<Evidence>> GetListAsync(string token)
        {
            List<Evidence> evidenceList = null;

             string Uri = $"{_webAPIBaseAddress}api/evidence/getevidences?token={token}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.GetAsync(Uri);
                    if (response.StatusCode== System.Net.HttpStatusCode.OK)
                    {
                        var resultWebAPI = await response.Content.ReadAsStringAsync();
                        evidenceList = JsonConvert.DeserializeObject<List<Evidence>>(resultWebAPI);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return evidenceList;
        }

        public async Task<EvidenceDetail> GetItemByIdAsync(string token, int evidenceId)
        {
            EvidenceDetail evidenceDetail = null;

            string Uri = $"{_webAPIBaseAddress}api/evidence/getevidencebyid?token={token}&&evidenceId={evidenceId}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.GetAsync(Uri);
                    var resultWebAPI = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        evidenceDetail = JsonConvert.DeserializeObject<EvidenceDetail>(resultWebAPI);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return evidenceDetail;
        }

    }
}
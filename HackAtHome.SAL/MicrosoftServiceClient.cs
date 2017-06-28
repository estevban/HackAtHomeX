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
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace HackAtHome.SAL
{
    public class MicrosoftServiceClient
    {
        MobileServiceClient _client;

        private IMobileServiceTable<LabItem> _labItemTable;

        public async Task SendEvidence(LabItem userEvidence)
        {
            _client = new MobileServiceClient(@"http://xamarin-diplomado.azurewebsites.net/");
            _labItemTable = _client.GetTable<LabItem>();
            await _labItemTable.InsertAsync(userEvidence);
        }
    }
}
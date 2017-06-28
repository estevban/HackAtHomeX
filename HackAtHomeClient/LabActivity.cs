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

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = false, Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo")]
    public class LabActivity : Activity
    {
        string _token;
        string _userName;
        ListView _listViewEvidence;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (string.IsNullOrWhiteSpace(_token))
            {
                _token = Intent.GetStringExtra("Token");
                _userName = Intent.GetStringExtra("UserName");
            }

            if (savedInstanceState != null)
            {
                _token = savedInstanceState.GetString("Token");
                _userName = savedInstanceState.GetString("UserName");
            }

            SetContentView(Resource.Layout.Lab);

            var textViewUserName = FindViewById<TextView>(Resource.Id.textViewUserName);
            var _listViewEvidence = FindViewById<ListView>(Resource.Id.listViewEvidences);

            List<Evidence> evidenceList;

            var evidenceFragment = (EvidenceFragment)FragmentManager.FindFragmentByTag("evidence");
            if (evidenceFragment==null)
            {
                HackAtHome.SAL.EvidenceService evidenceService = new HackAtHome.SAL.EvidenceService();
                evidenceList = await evidenceService.GetListAsync(_token);
                evidenceFragment = new EvidenceFragment();
                evidenceFragment.EvidenceList = evidenceList;
                FragmentManager.BeginTransaction().Add(evidenceFragment, "evidence").Commit();
            }
            else
            {
                evidenceList = evidenceFragment.EvidenceList;
            }
                        
            _listViewEvidence.Adapter = new HackAtHome.CustomAdapters.EvidencesAdapter(
                    this
                    , Resource.Layout.LabItem
                    , Resource.Id.textViewLabItemDescription
                    , Resource.Id.textViewLabItemStatus
                    , evidenceList);

            _listViewEvidence.ItemClick += _listViewEvidence_ItemClick;

            textViewUserName.Text = _userName;
        }

        private void _listViewEvidence_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent(this, typeof(LabDetailActivity));
            var description = e.View.FindViewById<TextView>(Resource.Id.textViewLabItemDescription);
            var status = e.View.FindViewById<TextView>(Resource.Id.textViewLabItemStatus);
            
            intent.PutExtra("Token", _token);
            intent.PutExtra("UserName", _userName);
            intent.PutExtra("Id", e.Id);
            intent.PutExtra("Description", description.Text);
            intent.PutExtra("Status", status.Text);
            StartActivity(intent);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutString("Token", _token);
            outState.PutString("UserName", _userName);
            base.OnSaveInstanceState(outState);
        }
    }
}
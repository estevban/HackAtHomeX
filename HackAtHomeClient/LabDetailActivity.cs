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
using Android.Webkit;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo")]
    public class LabDetailActivity : Activity
    {
        string _token;
        string _userName;
        string _description;
        string _status;
        long _id;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (string.IsNullOrWhiteSpace(_token))
            {
                _token = Intent.GetStringExtra("Token");
                _userName = Intent.GetStringExtra("UserName");
                _id = Intent.GetLongExtra("Id", long.MinValue);
                _description = Intent.GetStringExtra("Description");
                _status = Intent.GetStringExtra("Status");
            }

            if (savedInstanceState != null)
            {
                _token = savedInstanceState.GetString("Token");
                _userName = savedInstanceState.GetString("UserName");
                _id = savedInstanceState.GetLong("Id");
                _description = savedInstanceState.GetString("Description");
                _status = savedInstanceState.GetString("Status");
            }

            SetContentView(Resource.Layout.LabDetail);

            EvidenceDetail evidenceDetailItem;

            var evidenceDetailFragment = (EvidenceDetailFragment)FragmentManager.FindFragmentByTag("evidenceDetail");
            if (evidenceDetailFragment == null)
            {
                HackAtHome.SAL.EvidenceService evidenceService = new HackAtHome.SAL.EvidenceService();
                evidenceDetailItem = await evidenceService.GetItemByIdAsync(_token, (int)_id);
                evidenceDetailFragment = new EvidenceDetailFragment();
                evidenceDetailFragment.EvidenceDetail = evidenceDetailItem;
                FragmentManager.BeginTransaction().Add(evidenceDetailFragment, "evidenceDetail");
            }
            else
            {
                evidenceDetailItem = evidenceDetailFragment.EvidenceDetail;
            }

            var textViewDetailUserName = FindViewById<TextView>(Resource.Id.textViewDetailUserName);
            var textViewLabDetailDescription = FindViewById<TextView>(Resource.Id.textViewLabDetailDescription);
            var textViewLabDetailStatus = FindViewById<TextView>(Resource.Id.textViewLabDetailStatus);
            var webViewDetailDescription = FindViewById<WebView>(Resource.Id.webViewDetailDescription);
            var resourcePosition = IsLandScape(this) ? Resource.String.LandScapeText : Resource.String.Portrait;
            var imageViewContent = FindViewById<ImageView>(Resource.Id.imageViewContent);

            textViewDetailUserName.Text = _userName;
            textViewLabDetailDescription.Text = $"{_description} ({GetString(resourcePosition)})";
            textViewLabDetailStatus.Text = _status;
            webViewDetailDescription.LoadDataWithBaseURL(null, $"<html><head><style type='text/css'>body{{color:#fff}}</style></head><body>{evidenceDetailItem.Description}</body></html>", "text/html", "utf-8", null);
            webViewDetailDescription.SetBackgroundColor(Android.Graphics.Color.Transparent);
           
            Koush.UrlImageViewHelper.SetUrlDrawable(imageViewContent, evidenceDetailItem.Url);
        }

        bool IsLandScape(Activity activity)
        {
            var orientation = activity.WindowManager.DefaultDisplay.Rotation;
            return orientation == SurfaceOrientation.Rotation90 || orientation == SurfaceOrientation.Rotation270;
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutString("Token", _token);
            outState.PutString("UserName", _userName);
            outState.PutLong("Id", _id);
            outState.PutString("Description", _description);
            outState.PutString("Status", _status);
            base.OnSaveInstanceState(outState);
        }
    }
}
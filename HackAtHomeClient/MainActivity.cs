using Android.App;
using Android.Widget;
using Android.OS;
using HackAtHome.Entities;
using Android.Content;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo")]
    public class MainActivity : Activity
    {
        EditText _editTextEmail;
        EditText _editTextPassword;
        Button _buttonValidate;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _editTextEmail = FindViewById<EditText>(Resource.Id.editTextEmail);
            _editTextPassword = FindViewById<EditText>(Resource.Id.editTextPassword);
            _buttonValidate = FindViewById<Button>(Resource.Id.buttonValidate);
            _buttonValidate.Click += _buttonValidate_Click;

        }

        private void _buttonValidate_Click(object sender, System.EventArgs e)
        {
            Validate();
        }

        public async void Validate()
        {
            HackAtHome.SAL.Security security = new HackAtHome.SAL.Security();
            var result = await security.AuthenticateAsyc(_editTextEmail.Text, _editTextPassword.Text);
            if (result.Status == Status.Success)
            {
                string myDevice = Android.Provider.Settings.Secure.GetString(ContentResolver
                 , Android.Provider.Settings.Secure.AndroidId);

                var labItem = new LabItem
                {
                    DeviceId = myDevice
                     ,
                    Email = _editTextEmail.Text
                     ,
                    Lab = "Hack@Home"
                };

                HackAtHome.SAL.MicrosoftServiceClient microsoftServiceClient = new HackAtHome.SAL.MicrosoftServiceClient();
                await microsoftServiceClient.SendEvidence(labItem);

                var intent = new Intent(this, typeof(LabActivity));
                intent.PutExtra("Token", result.Token);
                intent.PutExtra("UserName", result.FullName);
                StartActivity(intent);
            }
            else
            {
                Android.App.AlertDialog.Builder builer =
                new AlertDialog.Builder(this);
                AlertDialog alert = builer.Create();
                alert.SetTitle(GetString(Resource.String.ErrorTitle));
                alert.SetIcon(Resource.Drawable.Icon);
                alert.SetMessage(GetString(Resource.String.ErrorDescription));
                alert.SetButton(GetString(Resource.String.Accept), (s, e) => { });
                alert.Show();
            }
        }

    }
}


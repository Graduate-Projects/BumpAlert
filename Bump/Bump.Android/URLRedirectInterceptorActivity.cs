using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bump.Droid
{
    [Activity(Label = "URLRedirectInterceptorActivity", NoHistory = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataSchemes = new[] { "com.googleusercontent.apps.676373510849-52ueeeg45fs3sao5g8h5lab0f23gvdu0" },
        DataPath = "/oauth2redirect")]
    public class URLRedirectInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var uri = new Uri(Intent.Data.ToString());
            Utils.AuthenticationState.Authenticator.OnPageLoading(uri);
            Finish();
        }
    }
}
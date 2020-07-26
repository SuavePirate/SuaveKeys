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
using SuaveKeys.Clients.Services;

namespace SuaveKeys.Clients.Droid.Services
{
    public class AndroidAuthClientSettings : IAuthClientSettings
    {
        public string ClientId => "292845a4-48ee-49bf-be74-980ae6a8390b";

        public string ClientSecret => "YWE4YmQxMmQtNTVlYi00MzAzLTkxMmYtMmJkYjdmNzM4ZjctNzZmOGZlMzEtYWE5Ny00M2IwLWIzODktZGQzOGIyZWIyNzQ2";
    }
}
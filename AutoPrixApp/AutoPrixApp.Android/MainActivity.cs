using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using PCLAppConfig;
using CarouselView.FormsPlugin.Droid;
using Acr.UserDialogs;


namespace AutoPrixApp.Droid
{
    [Activity(Label = "AutoPrixApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private bool doubleBackToExitPressedOnce = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            InitControl();

            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            try
            {
                ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            }
            catch (Exception e) { }

            UserDialogs.Init(this);
            LoadApplication(new App());
        }

        public void InitControl() {
            CarouselViewRenderer.Init();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
                Rg.Plugins.Popup.Services.PopupNavigation.PopAsync();
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
                //long currentTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
                //// source https://stackoverflow.com/a/14006485/3814729
                //if (currentTime - lastPress > 5000)
                //{
                //    Toast.MakeText(this, "Vuelve a presionar para salir", ToastLength.Long).Show();
                //    lastPress = currentTime;
                //}
                //else
                //{
                //    base.OnBackPressed();
                //}
                
            }
            /*
            if (doubleBackToExitPressedOnce)
            {
                base.OnBackPressed();
                Java.Lang.JavaSystem.Exit(0);
                return;
            }


            this.doubleBackToExitPressedOnce = true;
            Toast.MakeText(this, "Saliendo del Aplicativo", ToastLength.Short).Show();

            new Handler().PostDelayed(() =>
            {
                doubleBackToExitPressedOnce = false;
            }, 2000);*/
        }

    }
}
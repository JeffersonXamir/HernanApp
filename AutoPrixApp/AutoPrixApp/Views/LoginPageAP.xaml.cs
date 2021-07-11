using AutoPrixApp.Services;
using AutoPrixApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPrixApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPageAP : ContentPage
    {
        public LoginPageAP()
        {
            InitializeComponent();
            this.BindingContext = new LoginPageAPViewModel(Navigation);
            /*bool ob = Preferences.Get("IsLoggin",false);
            if (ob)
            {
                Shell.Current.GoToAsync("//AboutPage");
            }
            else {
                Shell.Current.GoToAsync("//LoginPage");
                //Preferences.Clear();
                //Navigation.PushModalAsync(new LoginPageAP());
            }
            */
        }
    }
}
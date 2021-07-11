using AutoPrixApp.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPrixApp.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            this.BindingContext = new AboutViewModel();
            /*
            if (Application.Current.Properties.ContainsKey("IsLoggin"))
            {
                var isLogging = Application.Current.Properties["IsLoggin"];
                
                if (isLogging.ToString() == "1")
                    Shell.Current.GoToAsync("///AboutPage");
                else
                    Shell.Current.GoToAsync("//LoginPage");
            }*/
            //var id = Application.Current.Properties["id"] as int;
        }
    }
}
using System;
using System.Collections.Generic;
using AutoPrixApp.Services;
using AutoPrixApp.ViewModels;
using AutoPrixApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AutoPrixApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //CURRENT INTEM TE MUESTRA LA PAG PRINCIPAL DE TU APP
            CurrentItem = Acerca;
            Acerca.CurrentItem = AcercaTab1;
            AcercaTab1.CurrentItem = AboutPage;

        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                Preferences.Clear();
                //await Shell.Current.GoToAsync("//LoginPage");
                Application.Current.MainPage = new LoginPageAP();
                //Application.Current.MainPage = new NavigationPage(new LoginPageAP());

            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("error", "\n" + ex.Message, "ok");
            }
        }
        

        /*protected override void OnAppearing()
        {
            base.OnAppearing();
            FlyoutHeader = new HeaderContentView();

        }*/
    }
}

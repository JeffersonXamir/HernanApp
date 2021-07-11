using AutoPrixApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPrixApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShellAdministrador : Shell
    {
        public AppShellAdministrador()
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
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("error", "\n" + ex.Message, "ok");
            }
        }
    }
}
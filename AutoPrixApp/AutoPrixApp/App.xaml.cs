using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AutoPrixApp.Services;
using AutoPrixApp.Views;
using Xamarin.Essentials;
using AutoPrixApp.Views.MantenimientoGnrl;
using Plugin.Connectivity;

namespace AutoPrixApp
{
    public partial class App : Application
    {

        public App()
        {

            InitializeComponent();
            RegistrerRoutes();
            DependencyService.Register<MockDataStore>();
            //MainPage = new AppShell();
            //intent2
            bool Conection = CrossConnectivity.Current.IsConnected;
            bool ob = Preferences.Get("IsLoggin", false);
            Int64 rol = Int64.Parse(Preferences.Get("IdRol", 0l).ToString());

            if (Conection)
            {
                if (ob)
                {
                    if (rol == 1)
                    {//ADMINISTRADOR
                        MainPage = new AppShellAdministrador();
                    }
                    else if (rol == 2)
                    {//EMPLEADO
                        MainPage = new AppShell();
                    }
                    else if (rol == 3)
                    {//CLIENTE
                        MainPage = new AppShellCliente();//EN DESARROLLO
                        //Preferences.Clear();
                    }

                }
                else
                {
                    MainPage = new LoginPageAP();
                }
            }
            else
            {
                MainPage = new AppSinConexion();
            }


        }

        private void RegistrerRoutes()
        {
            //Routing.RegisterRoute(nameof(RegistroUsuarioPage), typeof(RegistroUsuarioPage));
            Routing.RegisterRoute("RegistroUsuario", typeof(RegistroUsuarioPage));
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute("LoginPage", typeof(LoginPageAP));
            //Routing.RegisterRoute("RecepcionVehiculo", typeof(RecepcionVehiculoPage));
            Routing.RegisterRoute(nameof(RecepcionVehiculoPage), typeof(RecepcionVehiculoPage));
            Routing.RegisterRoute(nameof(TrabajoDetallePage), typeof(TrabajoDetallePage));
            //Routing.RegisterRoute(nameof(EstadoLlantasPage), typeof(EstadoLlantasPage));
            Routing.RegisterRoute("EstadoLlantasPage", typeof(EstadoLlantasPage));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

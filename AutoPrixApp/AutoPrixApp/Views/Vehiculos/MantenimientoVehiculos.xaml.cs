using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoPrixApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPrixApp.Views.Vehiculos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MantenimientoVehiculos : ContentPage
    {
        public List<string> listaVehi = new List<string> { "juan", "polo", "merardo", "pepe" };
        public MantenimientoVehiculos()
        {
            InitializeComponent();
            //this.PerformSearch = new Command(this.ClickBuscar);
            //ListaVehiculos.ItemsSource = listaVehi;
            BindingContext = new MantenimientoVehiculosViewModel();
        }

        public Command PerformSearch { get; set; }

        private void ClickBuscar(object obj)
        {
            Application.Current.MainPage.DisplayAlert("Prueba", "\n" + "", "ok");
        }

        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoPrixApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPrixApp.Views.MantenimientoGnrl
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MantenimientoGnrlPage : ContentPage
    {
        public List<string> listaVehi = new List<string> { "juan", "polo", "merardo", "pepe" };
        public MantenimientoGnrlPage()
        {
            InitializeComponent();
            //this.PerformSearch = new Command(this.ClickBuscar);
            //ListaVehiculos.ItemsSource = listaVehi;
            BindingContext = new MantenimientoGnrlViewModel();
        }

        public Command PerformSearch { get; set; }

        private void ClickBuscar(object obj)
        {
            Application.Current.MainPage.DisplayAlert("Prueba", "\n" + "", "ok");
        }

        
    }
}
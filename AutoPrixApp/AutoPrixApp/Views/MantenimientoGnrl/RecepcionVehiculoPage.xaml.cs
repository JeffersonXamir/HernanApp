using AutoPrixApp.Models;
using AutoPrixApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPrixApp.Views.MantenimientoGnrl
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecepcionVehiculoPage : ContentPage
    {
        public RecepcionVehiculoPage(VehiculosClientes item,Int64 tipoTrabajo)
        {
            InitializeComponent();
            BindingContext = new RecepcionVehiculoViewModel(item,tipoTrabajo);
        }
    }
}
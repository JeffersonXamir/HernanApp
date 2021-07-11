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
    public partial class EstadoLlantasPage : ContentPage
    {
        public EstadoLlantasPage(OrdenTrabajoCab obj,List<ImagenTrabajos> items, Int64 tipoTrabajo)
        {
            InitializeComponent();
            BindingContext = new EstadoLlantasViewModel(obj,items,tipoTrabajo);
            //BindingContext = new RecepcionVehiculoViewModel();
        }
    }
}
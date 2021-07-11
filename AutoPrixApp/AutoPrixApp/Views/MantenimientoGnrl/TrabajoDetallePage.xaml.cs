using AutoPrixApp.Models;
using AutoPrixApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPrixApp.Views.MantenimientoGnrl
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrabajoDetallePage : ContentPage
    {
        
        public TrabajoDetallePage(OrdenTrabajoCab obj, List<ImagenTrabajos> items, List<OrdenTrabajoLlantas> LsLlantas, Int64 tipoTrabajo)
        {
            InitializeComponent();
            BindingContext = new TrabajoDetalleViewModel(obj,items,LsLlantas, tipoTrabajo);
           
        }
        
    }
}
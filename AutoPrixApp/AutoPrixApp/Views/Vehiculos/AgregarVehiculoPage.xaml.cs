using AutoPrixApp.Models;
using AutoPrixApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPrixApp.Views.Vehiculos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarVehiculoPage : ContentPage
    {
        public AgregarVehiculoPage()
        {
            InitializeComponent();
            BindingContext = new AgregarVehiculoViewModel();

        }
    }
}
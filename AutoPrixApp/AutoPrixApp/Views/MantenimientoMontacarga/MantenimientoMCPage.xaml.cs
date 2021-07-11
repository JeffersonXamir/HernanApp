using AutoPrixApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPrixApp.Views.MantenimientoMontacarga
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MantenimientoMCPage : ContentPage
    {
        public MantenimientoMCPage()
        {
            InitializeComponent();
            BindingContext = new MontaCargasViewModel();
        }
    }
}
using AutoPrixApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPrixApp.Views.Mecanica
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class mecanicaPage : ContentPage
    {
        public mecanicaPage()
        {
            InitializeComponent();
            BindingContext = new MecanicaViewModel();
        }

        public Command PerformSearch { get; set; }
    }
}
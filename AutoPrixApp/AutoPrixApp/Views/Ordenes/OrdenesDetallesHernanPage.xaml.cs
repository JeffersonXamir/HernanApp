using AutoPrixApp.Models;
using AutoPrixApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPrixApp.Views.Ordenes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdenesDetallesHernanPage : ContentPage
    {
        public OrdenesDetallesHernanPage(OrdenTrabajoCab item)
        {
            InitializeComponent();
            BindingContext = new OrdenesDetallesHernanViewModel(item);
        }
    }
}
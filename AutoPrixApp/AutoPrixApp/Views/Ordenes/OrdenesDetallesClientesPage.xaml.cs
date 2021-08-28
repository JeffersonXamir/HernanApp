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
    public partial class OrdenesDetallesClientesPage : ContentPage
    {
        public OrdenesDetallesClientesPage(OrdenTrabajoCab item)
        {
            InitializeComponent();
            BindingContext = new OrdenesDetallesViewModel(item);
        }
    }
}
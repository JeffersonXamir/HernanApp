using AutoPrixApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPrixApp.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupAprobarOrdenView
    {
        public PopupAprobarOrdenView(Int64 idorden)
        {
            InitializeComponent();
            BindingContext = new PopupAprobarOrdenViewModel(idorden); 
        }
    }
}
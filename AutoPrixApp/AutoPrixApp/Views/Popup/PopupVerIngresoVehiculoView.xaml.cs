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
    public partial class PopupVerIngresoVehiculoView
    {
        public PopupVerIngresoVehiculoView(Int64 idorden,string modo)
        {
            InitializeComponent();
            BindingContext = new PopupVerIngresoVehiculoViewModel(idorden, modo);
        }
    }
}
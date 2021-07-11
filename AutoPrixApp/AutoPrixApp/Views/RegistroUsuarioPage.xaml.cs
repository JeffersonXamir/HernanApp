using AutoPrixApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPrixApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistroUsuarioPage : ContentPage
    {
        public RegistroUsuarioPage()
        {
            InitializeComponent();
            BindingContext = new RegistroUsuarioViewModel(Navigation);
        }
    }
}
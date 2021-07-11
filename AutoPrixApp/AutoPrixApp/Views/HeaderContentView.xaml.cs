using AutoPrixApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPrixApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderContentView : ContentView
    {
        private string usuario;
        public static string variable;
        

        public HeaderContentView()
        {
            InitializeComponent();
            this.BindingContext = new HeaderContentViewModel();
            //OnNavigated();
            //textLabel1.Text = Preferences.Get("UserLog", "None");
            //textLabel1.Text = variable;
            //Console.WriteLine("xdxd: ",variable);
        }

        public string Usuario { get => usuario; set => usuario = value; }

        

    }
    }
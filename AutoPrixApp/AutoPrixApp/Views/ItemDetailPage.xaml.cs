using System.ComponentModel;
using Xamarin.Forms;
using AutoPrixApp.ViewModels;

namespace AutoPrixApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
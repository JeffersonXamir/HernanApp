using AutoPrixApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AutoPrixApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "Quienes Somos";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamain-quickstart"));

            Slides = new ObservableCollection<Slide>(new[]
            {
                //new Slide("slide3.jpg", "Some description for slide one."),
                new Slide("slide8.jpg", "Some description for slide two."),
                new Slide("slide14.jpg", "Some description for slide three."),
                new Slide("slide15.jpg", "Some description for slide three."),
                new Slide("slide16.jpg", "Some description for slide three."),
                new Slide("slide17.jpg", "Some description for slide three.")
            });
        }

        public ICommand OpenWebCommand { get; }

        public ObservableCollection<Slide> Slides { get; }
    }
}
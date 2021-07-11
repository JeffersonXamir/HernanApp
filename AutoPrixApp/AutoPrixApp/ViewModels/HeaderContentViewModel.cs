using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace AutoPrixApp.ViewModels
{
    public class HeaderContentViewModel : BaseViewModel
    {
        private string user;
        private string rol;

        public HeaderContentViewModel()
        {
            user = Preferences.Get("UserLog", "None");
            rol = Preferences.Get("Rol","None");
        }

        public string User { 
            get => user;
            set {
                if (this.user == value)
                {
                    return;
                }

                this.user = value;
                OnPropertyChanged();
            } 
        }

        public string Rol {
            get => rol;
            set {
                if (this.rol == value) {
                    return;
                }
                this.rol = value;
                OnPropertyChanged();
            }
        
        }
        

    }
}

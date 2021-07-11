using AutoPrixApp.Models;
using AutoPrixApp.Views;
using AutoPrixWebApi.Entidades;
using AutoPrixWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.ComponentModel;

namespace AutoPrixApp.ViewModels
{
    public class LoginPageAPViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private string username;
        private string email;
        private string password;
        List<Usuario> lsusuario = new List<Usuario>();
        public INavigation Navigation { get; set; }

        public LoginPageAPViewModel(INavigation navigation) {
            this.Navigation = navigation;
            this.GoToViewCommand = new Command(this.LoadView);
            this.LoginCommand = new Command(this.ExecuteLogin);
            this.ResetPasswordCommand = new Command(this.SubmitPasswordReset);
            #if DEBUG
                    this.username = "ADMIN_HERNAN";
                    this.password = "CAS1001JA";
            #endif

        }

        public string Username
        {
            get => this.username;
            set
            {
                if (this.username == value)
                {
                    return;
                }

                this.username = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => this.email;
            set
            {
                if (this.email == value)
                {
                    return;
                }

                this.email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => this.password;
            set
            {
                if (this.password == value)
                {
                    return;
                }

                this.password = value;
                OnPropertyChanged();
            }
        }

        public Command GoToViewCommand { get; set; }

        public Command LoginCommand { get; set; }

        public Command ResetPasswordCommand { get; set; }

        public async void LoadView(ViewType viewType)
        {
            try
            {
                switch (viewType)
                {
                    case ViewType.LoginView:
                        //this.Content = new LoginView();
                        //await Shell.Current.GoToAsync($"//{nameof(LoginPageAP)}"); 
                        await Shell.Current.GoToAsync("LoginPage");
                        break;
                    case ViewType.SignUpView:
                        //await Shell.Current.GoToAsync("RegistroUsuarioPage");
                        await Navigation.PushModalAsync(new RegistroUsuarioPage());
                        //await Shell.Current.GoToAsync(nameof(RegistroUsuarioPage));
                        break;
                    case ViewType.PasswordResetView:
                        //this.Content = new PasswordResetView();
                        break;
                    case ViewType.Dashboard:

                        break;
                }
            }
            catch (Exception e) {
                await Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
            }
        }

        private void LoadView(object viewType)
        {
            this.LoadView((ViewType)viewType);
        }

        private void ExecuteLogin(object obj)
        {
            switch ((LoginType)obj)
            {
                case LoginType.Normal:
                    
                    this.IsBusy = true;
                    OnLoginUsuario();

                    break;
                case LoginType.SignUp:
                    // Use Username, Email and Password properties
                    Application.Current.MainPage.DisplayAlert("Signup Login", $"You have signed up with:\r\nUsername: {Username}\nEmail:{Email}\nPassword: {Password}", "ok");
                    break;
                case LoginType.PasswordReset:
                    // Use Email property for reset request
                    Application.Current.MainPage.DisplayAlert("Password Reset Requested", $"Password reset for: Email:{Email}", "ok");
                    break;
            }
        }

        private void SubmitPasswordReset()
        {
            // Use the Email property
            Application.Current.MainPage.DisplayAlert("Password Reset", $"You have requested a password reset for: {Email}", "ok");
        }

        #region Funciones definition
        public async void OnAppearing()
        {
            try
            {
                /*pruebas instancia task*/
                String parametros = "";
                var json = await Globales.GetApiApp("usuario", parametros);
                jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());

                Usuario c = null;
                foreach (var item in jsonres.RESULTADO)
                {
                    c = JsonConvert.DeserializeObject<Usuario>(item.ToString());
                    lsusuario.Add(c);
                }

                String str = Encrypt.GetSHA256(lsusuario[0].Pass.ToString());
                await Application.Current.MainPage.DisplayAlert("Retorno de Api", $"Login: " + lsusuario[0].Login + "\n" + "Contra: " + lsusuario[0].Pass + "\n" + "Contra SHA: " + str + "\n", "ok");
                //LoadView(ViewType.Dashboard);
                //Application.Current.MainPage.DisplayAlert("POSI", "\n" + "SIGA NO MA \n", "ok");


            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
            }
        }

        public async void OnLoginUsuario()
        {
            try
            {
                /*pruebas instancia task*/
                Usuario usu = new Usuario();
                usu.Login = Username;
                usu.Pass = Encrypt.GetSHA256(Password);
                var jsonObj = JsonConvert.SerializeObject(usu);
                
                String parametros = "";
                var json = await Globales.PostApiApp("usuario/PostVerificaUsuario", jsonObj);
                jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());

                Usuario c = null;
                foreach (var item in jsonres.RESULTADO)
                {
                    c = JsonConvert.DeserializeObject<Usuario>(item.ToString());
                    lsusuario.Add(c);
                }

                if (c != null)
                {

                    //await Application.Current.MainPage.DisplayAlert("Retorno de Api", $"Login: " + lsusuario[0].Login + "\n" + "Contra: " + lsusuario[0].Pass + "\n", "ok");
                    //Application.Current.Properties["User"] = lsusuario[0].Login;                    
                    //Application.Current.Properties["IsLoggin"] = 1;
                    Preferences.Set("UserLog",lsusuario[0].Login.ToString());
                    Preferences.Set("NombreUsu",lsusuario[0].Nombre.ToString());
                    Preferences.Set("IdUsuario",lsusuario[0].IdUsuario);
                    Preferences.Set("IdRol",lsusuario[0].IdRol);
                    Preferences.Set("Rol",lsusuario[0].Rol);
                    bool log = (lsusuario.Count > 0?true:false);
                    Preferences.Set("IsLoggin", log);
                    //await Application.Current.SavePropertiesAsync();
                    //await App.Current.SavePropertiesAsync();
                    //await Shell.Current.GoToAsync("//AboutPage");
                    //intent2
                    this.IsBusy = false;
                    Int64 rol = Int64.Parse(Preferences.Get("IdRol", 0l).ToString());
                    if (rol == 1)
                    {//ADMINISTRADOR
                        Application.Current.MainPage = new AppShellAdministrador();
                        await Shell.Current.GoToAsync("//AboutPage");
                    }
                    else if (rol == 2)
                    {//EMPLEADO
                        Application.Current.MainPage = new AppShell();
                        await Shell.Current.GoToAsync("//AboutPage");
                    }
                    else if (rol == 3)
                    {//CLIENTE
                        Preferences.Clear();
                        Application.Current.MainPage = new AppShellCliente();//EN DESARROLLO
                    }
                    //Application.Current.MainPage = new AppShell();
                    //await Shell.Current.GoToAsync("//AboutPage");

                }
                else
                {
                    this.IsBusy = false;
                    await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", $"Contraseña o Usuario Incorrecto.\n", "Aceptar");
                }
            }
            catch (Exception e)
            {
                this.IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
            }
        }

      
        #endregion
    }
}

using AutoPrixWebApi.Entidades;
using AutoPrixApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using AutoPrixApp.Models;
using Newtonsoft.Json;
using AutoPrixWebApi.Models;
using System.ComponentModel;
using Acr.UserDialogs;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Plugin.Connectivity;

namespace AutoPrixApp.ViewModels
{
    public class RegistroUsuarioViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public UsuarioEnte user = null;
        private string nombre1;
        private string nombre2;
        private string apellido1;
        private string apellido2;
        private string cedula;
        private string telefono;
        private string usuario;
        private string contrasenia;
        private string email;
        private string texBtnRegistrar;
        private DateTime fechaNacimiento;
        public INavigation Navigation { get; set; }

        public RegistroUsuarioViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            this.GoToViewCommand = new Command(this.LoadView);
            this.LoginCommand = new Command(this.ExecuteLogin);
            this.Accept = new Command(this.OnAccept);
            this.Cancel = new Command(this.OnCancel);
            fechaNacimiento = new DateTime(2020,01,01);
            Title = Int64.Parse(Preferences.Get("IdRol", 0l).ToString()) == 0 ? "" : "REGISTRO DE EMPLEADO";
            TexBtnRegistrar = Int64.Parse(Preferences.Get("IdRol",0l).ToString()) == 0 ? "REGISTRARSE" : "REGISTRAR";
            IsBusy = Int64.Parse(Preferences.Get("IdRol",0l).ToString()) == 0 ? true : false;
        }

        private void OnAccept(object obj)
        {
            //Application.Current.MainPage.DisplayAlert("Prueba", "\n" + fechaNacimiento, "ok");
        }

        private void OnCancel(object obj)
        {
            // implement your custom logic here
        }
        public string Nombre1 { get => nombre1; set { nombre1 = value; } }
        public string Nombre2 { get => nombre2; set { nombre2 = value; } }
        public string Apellido1 { get => apellido1; set { apellido1 = value; } }
        public string Apellido2 { get => apellido2; set { apellido2 = value; } }
        public string Cedula { get => cedula; set { cedula = value; } }
        public string Telefono { get => telefono; set { telefono = value; } }
        public string Email { get => email; set { email = value; } }
        public string Usuario { get => usuario; set { usuario = value; } }
        public string Contrasenia { get => contrasenia; set { contrasenia = value; } }
        public string TexBtnRegistrar { get => texBtnRegistrar; set { texBtnRegistrar = value; } }
        public DateTime FechaNacimiento { get => fechaNacimiento; set => fechaNacimiento = value; }



        #region Comandos definition
        public Command GoToViewCommand { get; set; }
        public Command LoginCommand { get; set; }
        public Command OnDateSelected { get; set; }
        public Command Accept { get; set; }
        public Command Cancel { get; set; }

        #endregion

        #region VistasIr definition
        private void LoadView(object viewType)
        {
            this.LoadView((ViewType)viewType);
        }
        public async void LoadView(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.LoginView:
                    //await Shell.Current.GoToAsync("//LoginPage");
                    await Navigation.PopModalAsync();
                    break;
                case ViewType.SignUpView:
                    await Shell.Current.GoToAsync("RegistroUsuario");
                    break;
                case ViewType.PasswordResetView:
                    //this.Content = new PasswordResetView();
                    break;
                case ViewType.Dashboard:

                    break;
            }
        }
        #endregion

        #region Accion definition
        private void ExecuteLogin(object obj)
        {
            switch ((LoginType)obj)
            {
                case LoginType.Normal:
                    // OnLoginUsuario();
                    break;
                case LoginType.SignUp:
                    OnRegistrarUsuario();
                    //Application.Current.MainPage.DisplayAlert("Signup Login", $"You have signed up with:\r\nUsername: {Usuario}\nEmail:{Email}\nPassword: {Contrasenia}", "ok");

                    break;
                case LoginType.PasswordReset:
                    // Use Email property for reset request
                    //Application.Current.MainPage.DisplayAlert("Password Reset Requested", $"Password reset for: Email:{Email}", "ok");
                    break;
            }
        }
        #endregion

        #region Funciones definition
        public async void OnRegistrarUsuario()
        {
            try
            {
                bool Conection = CrossConnectivity.Current.IsConnected;
                if (Conection) {
                    user = await ObtenerUsuario();
                    if (user == null) {
                        return;
                    }
                    var jsonObj = JsonConvert.SerializeObject(user);
                    UserDialogs.Instance.ShowLoading("Guardando usuario...");
                    String parametros = "";
                    var json = await Globales.PostApiApp("usuario/PostCrearUsuario", jsonObj);
                    jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());

                    if (jsonres.MENSAJE == "Ok")
                    {
                        UserDialogs.Instance.HideLoading();
                        Int64 id = Int64.Parse(Preferences.Get("IdRol", 0l).ToString());
                        if (id == 0) {
                            await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", $"" + jsonres.STACK.ToString() + "\n", "Aceptar");
                            await Navigation.PopModalAsync();
                        }
                        else {
                            await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", $"" + "Usuario Registrado Con Éxito" + "\n", "Aceptar");
                            await Shell.Current.GoToAsync("//AboutPage");
                        }

                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", $"" + jsonres.STACK.ToString() + "\n", "Aceptar");
                    }
                }
                else 
                {
                    await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", $"" + "No hay conexión a Internet, Intentelo más tarde... " + "\n", "Aceptar");
                }
            }
            catch (Exception e)
            {
                UserDialogs.Instance.HideLoading();
                await Application.Current.MainPage.DisplayAlert("Error", "\n" + e.Message, "ok");
            }
        }

        public async Task<UsuarioEnte>  ObtenerUsuario() {
            UsuarioEnte usu = new UsuarioEnte();
            try
            {

                if (nombre1.Equals(""))
                {
                    throw new Exception("Ingrese un nombre..");
                }

                if (apellido1.Equals(""))
                {
                    throw new Exception("Ingrese un apellido..");
                }
                if (cedula.Equals(""))
                {
                    throw new Exception("Ingrese una cedula..");
                }
                if (telefono.Equals(""))
                {
                    throw new Exception("Ingrese un telefono..");
                }
                if (usuario.Equals(""))
                {
                    throw new Exception("Ingrese un usuario..");
                }
                if (email.Equals(""))
                {
                    throw new Exception("Ingrese un correo electronico..");
                }
                if (fechaNacimiento.Equals(""))
                {
                    throw new Exception("Ingrese la fecha de nacimiento..");
                }
                if (contrasenia.Equals(""))
                {
                    throw new Exception("Ingrese un contraseña..");
                }
                usu = new UsuarioEnte();
                usu.nombre1 = nombre1.ToUpper();
                usu.nombre2 = nombre2.ToUpper();
                usu.apellido1 = apellido1.ToUpper();
                usu.apellido2 = apellido2.ToUpper();
                usu.cedula = cedula;
                usu.telefono = telefono;
                usu.Login = usuario.ToUpper();
                usu.email = email;
                usu.fechaNacimiento = fechaNacimiento;
                usu.Pass = Encrypt.GetSHA256(contrasenia);
                Int64 id = Int64.Parse(Preferences.Get("IdUsuario", 0l).ToString());
                usu.UsuarioCreacion = (id==1 ? 1 : id == 0 ? 1 : 0);
                Int64 Idrol = Int64.Parse(Preferences.Get("IdRol", 0l).ToString());
                usu.IdRol = Idrol == 0 ? 3 : Idrol == 1 ? 2 : 2;
            }
            catch (Exception e) {
                await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", "\n" + e.Message, "ok");
                usu = null;
            }
            return usu;
        }

        #endregion
    }
}

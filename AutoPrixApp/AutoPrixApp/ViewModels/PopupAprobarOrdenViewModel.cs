using Acr.UserDialogs;
using AutoPrixApp.Entidades;
using AutoPrixApp.Models;
using AutoPrixWebApi.Entidades;
using AutoPrixWebApi.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AutoPrixApp.ViewModels
{
    public class PopupAprobarOrdenViewModel
    {
        public Int64 _ordenId;
        public PopupAprobarOrdenViewModel(Int64 idOrden) {
            ItemsEmpleados = new ObservableCollection<Usuario>();
            _ordenId=idOrden;
            cargarEmpleados();
            BtnAutorizaOrden = new Command(this.OnBtnAutorizaOrden);
        }

        

        public Usuario _empleadoSeleccionado;
        public Command BtnAutorizaOrden { get; }
        public ObservableCollection<Usuario> ItemsEmpleados { get; set; }

        public Usuario EmpleadoSeleccionado
        {
            get
            {
                return _empleadoSeleccionado;
            }

            set
            {
                _empleadoSeleccionado = value;
                //OnPropertyChanged("EmpleadoSeleccionado");
                //CargarModelos();
            }
        }

        

        public async void cargarEmpleados()
        {
            try
            {
                ItemsEmpleados = new ObservableCollection<Usuario>();
                //var idCab = _ordenTrabajoCab.IdOrdenTrabajoCab.ToString();
                string cadena = obtenerCadenaJson("EMPLEADOS");
                var json = await Globales.PostApiApp("usuario/PostUsuariosRoles", cadena);
                jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());
                ItemsEmpleados.Clear();

                if (jsonres.MENSAJE == "Ok")
                {
                    Usuario c = null;
                    foreach (var item in jsonres.RESULTADO)
                    {
                        c = JsonConvert.DeserializeObject<Usuario>(item.ToString());
                        ItemsEmpleados.Add(c);
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Automotriz Hernan Error", $"" + jsonres.STACK.ToString() + "\n", "Aceptar");
                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
            }
        }

        public async void OnBtnAutorizaOrden(object obj)
        {
            if (_empleadoSeleccionado == null) { await Application.Current.MainPage.DisplayAlert("AutoServicio Hernan", "\n" + "Debe seleccionar un Empleado", "Aceptar"); return; }

            try
            {
                var resp = await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", "¿Está seguro de Mandar a Autorizar la Orden?", "Aceptar", "Cancelar");
                if (resp)
                {
                    var cadena = obtenerCadenaJson("AUTORIZAORDEN");
                    var json = await Globales.PostApiApp("OrdenesClientes/PostAutorizarOrden", cadena);
                    jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());

                    if (jsonres.MENSAJE == "Ok")
                    {
                        await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", $"" + jsonres.STACK.ToString() + "\n", "Aceptar");
                        await PopupNavigation.PopAsync();
                        Int64 idusu = Int64.Parse(Preferences.Get("IdRol", 0l).ToString());
                        switch (idusu)
                        {
                            case 1: //ADMIN
                                await Shell.Current.GoToAsync("//OrdenesAdmin");//cambiar a OrdenesAdmin
                                break;
                            case 2: //EMPLEADO
                                await Shell.Current.GoToAsync("//OrdenClientes");//cambiar a OrdenClientes
                                break;
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Automotriz Hernan Error", $"" + jsonres.STACK.ToString() + "\n", "Aceptar");
                    }
                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Automotriz Hernan Error", $"" + e.Message + "\n", "Aceptar");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private string obtenerCadenaJson(string modo)
        {
            string cadena = "";
            Int64 idusu = Int64.Parse(Preferences.Get("IdUsuario", 0l).ToString());
            try
            {
                switch (modo) {
                    case "AUTORIZAORDEN":
                        var my_jsondata1 = new
                        {
                            idOrden = _ordenId,
                            idUsuario = idusu,
                            idEmpleado = _empleadoSeleccionado.IdUsuario,
                            observacion = "",
                            modo = "AUT"
                        };
                        cadena = JsonConvert.SerializeObject(my_jsondata1);
                        break;
                    case "EMPLEADOS":
                        var my_jsondata2 = new
                        {
                            idOrden =0,
                            idUsuario = idusu,
                            modo = "E"
                        };
                        cadena = JsonConvert.SerializeObject(my_jsondata2);
                        break;
                }
                

                //Tranform it to Json object
                //cadena = JsonConvert.SerializeObject(my_jsondata);

            }
            catch (Exception e)
            { return "Error: " + e.Message; }
            return cadena;
        }
    }
}

using Acr.UserDialogs;
using AutoPrixApp.Entidades;
using AutoPrixApp.Models;
using AutoPrixApp.Views.Ordenes;
using AutoPrixApp.Views.Vehiculos;
using AutoPrixWebApi.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AutoPrixApp.ViewModels
{
    public class AgregarVehiculoViewModel : BaseViewModel
    {

        public DateTime FechaActual = DateTime.Now;
        public ObservableCollection<Marca> ItemsMarca { get; set; }
        public ObservableCollection<Modelo> ItemsModelo { get; set; }
        public Marca _marcaSeleccionada;
        public Modelo _modeloSeleccionado;
        public string _placa;
        public string _anio;

        public AgregarVehiculoViewModel()
        {
            Title = "Vehículos";
            ItemsMarca = new ObservableCollection<Marca>();
            ItemsModelo = new ObservableCollection<Modelo>();

            BtnGuardar = new Command(this.OnBtnGuardarVehiculo);
            BuscarMarcasVechiculos();
        }

        public Command BtnGuardar { get; }
        public string Placa { get => _placa; set => SetProperty(ref _placa, value); }
        public string Anio { get => _anio; set => SetProperty(ref _anio, value); }
        public Marca MarcaSeleccionada
        {
            get
            {
                return _marcaSeleccionada;
            }

            set
            {
                _marcaSeleccionada = value;
                OnPropertyChanged("MarcaSeleccionada");
                CargarModelos();
            }
        }
        public Modelo ModeloSeleccionado
        {
            get
            {
                return _modeloSeleccionado;
            }

            set
            {
                _modeloSeleccionado = value;
                OnPropertyChanged("ModeloSeleccionado");
            }
        }
        public async void BuscarMarcasVechiculos()
        {
            try
            {
                //UserDialogs.Instance.ShowLoading("Realizando Búsqueda...");
                var idMarca = "0";//Preferences.Get("IdUsuario", "0");
                var json = await Globales.GetApiAppRoute("VehiculosClientes/GetVehiculosMarcas", idMarca);
                jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());
                ItemsMarca.Clear();

                if (jsonres.MENSAJE == "Ok")
                {
                    Marca c = null;
                    foreach (var item in jsonres.RESULTADO)
                    {
                        c = JsonConvert.DeserializeObject<Marca>(item.ToString());
                        ItemsMarca.Add(c);
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
            finally {
                //UserDialogs.Instance.HideLoading();
            }
        }

        public async void BuscarModeloVechiculos(string idMarca)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Cargando...");
                var json = await Globales.GetApiAppRoute("VehiculosClientes/GetVehiculosModelos", idMarca);
                jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());
                ItemsModelo.Clear();

                if (jsonres.MENSAJE == "Ok")
                {
                    Modelo c = null;
                    foreach (var item in jsonres.RESULTADO)
                    {
                        c = JsonConvert.DeserializeObject<Modelo>(item.ToString());
                        ItemsModelo.Add(c);
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
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        public async void OnBtnGuardarVehiculo()
        {
            try
            {
                //UserDialogs.Instance.ShowLoading("Cargando...");
                if (validarCampos()) { return; };
                bool Conection = CrossConnectivity.Current.IsConnected;
                if (Conection)
                {
                    var resp = await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", "¿Esta Seguro de Guardar El Vehículo?", "Aceptar", "Cancelar");
                    if (resp)
                    {
                        Int64 idusu = Int64.Parse(Preferences.Get("IdUsuario", 0l).ToString());
                        UserDialogs.Instance.ShowLoading("Guardando Información del Vehículo...");
                        VehiculosClientes GuardarObj = new VehiculosClientes();
                        GuardarObj.Marca = _marcaSeleccionada.IdMarca.ToString();
                        GuardarObj.Modelo = _modeloSeleccionado.IdModelo.ToString();
                        GuardarObj.placa = _placa;
                        GuardarObj.Anio = _anio;
                        GuardarObj.cliente = idusu.ToString();

                        String jsonGuardar = JsonConvert.SerializeObject(GuardarObj);
                        var json = await Globales.PostApiApp("VehiculosClientes/PostGuardarVehiculo", jsonGuardar);

                        jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());

                        if (jsonres.MENSAJE == "Ok")
                        {
                            await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", $"" + jsonres.STACK.ToString() + "\n", "Aceptar");
                            await Shell.Current.GoToAsync("//ServicioVehiculos");//cambiar a vehiculos
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", $"" + jsonres.STACK.ToString() + "\n", "Aceptar");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        
        public async void CargarModelos() {
           //UserDialogs.Instance.ShowLoading("Cargando Información...");
            if (_marcaSeleccionada != null)
            {
                BuscarModeloVechiculos(_marcaSeleccionada.IdMarca.ToString());
            }
            else {
                ItemsModelo.Clear();
            }
           
        }

        public async void MostrarError(int id) {
            switch (id) {
                case 1 :
                    await Application.Current.MainPage.DisplayAlert("Hernan App", "\n Seleccione la Marca del Vehículo", "Aceptar");
                    break;
                case 2:
                    await Application.Current.MainPage.DisplayAlert("Hernan App", "\n Seleccione el Modelo del Vehículo", "Aceptar");
                    break;
                case 3:
                    await Application.Current.MainPage.DisplayAlert("Hernan App", "\n Ingrese la placa del Vehículo", "Aceptar");
                    break;
                case 4:
                    await Application.Current.MainPage.DisplayAlert("Hernan App", "\n Ingrese el Año del Vehículo", "Aceptar");
                    break;
                case 5:
                    break;
                case 6:
                    break;

            }
        
        }


        public bool validarCampos() {
            bool aux = false;

            if (_marcaSeleccionada == null)
            {
                MostrarError(1);
                aux = true; return aux;
            }

            if (_modeloSeleccionado == null)
            {
                MostrarError(2);
                aux = true; return aux;
            }

            if (_placa == null || _placa.Equals(""))
            {
                MostrarError(3);
                aux = true; return aux;
            }

            if (_anio == null || _anio.Equals(""))
            {
                MostrarError(4);
                aux = true; return aux;
            }

            return aux;
        }

    }
}

using Acr.UserDialogs;
using AutoPrixApp.Models;
using AutoPrixApp.Views.Ordenes;
using AutoPrixApp.Views.Vehiculos;
using AutoPrixWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AutoPrixApp.ViewModels
{
    public class MantenimientoVehiculosViewModel : BaseViewModel
    {

        public DateTime FechaActual = DateTime.Now;
        public List<OrdenTrabajoCab> LsOrdenes { get; set; }
        public ObservableCollection<OrdenTrabajoCab> ItemsOrden { get; }
        public OrdenTrabajoCab _ordenSeleccionado;
        public ObservableCollection<VehiculosClientes> Items { get; }
        public Command<VehiculosClientes> ItemTapped { get; }
        public Command PerformSearch { get; set; }

        public MantenimientoVehiculosViewModel()
        {
            Title = "Vehículos";
            ItemsOrden = new ObservableCollection<OrdenTrabajoCab>();
            LsOrdenes = new List<OrdenTrabajoCab>();
            BtnBuscar = new Command(this.OnBtnBuscarOrdenes);
            BtnAgregar = new Command(this.OnBtnAgregarVehiculo);
            Items = new ObservableCollection<VehiculosClientes>();
            DateMaximum = FechaActual;
            SelectedDate = FechaActual; //DateTime.Parse(FechaActual.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            SelectedDateEnd = FechaActual; //DateTime.Parse(FechaActual.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            LoadItemsCommand = new Command(this.ExecuteLoadItemsCommand);
            ItemTapped = new Command<VehiculosClientes>(OnItemSelected);
            PerformSearch = new Command(SearchBar_GetVehiculos);
            GetVehiculosClientes("none");
        }

       
        public OrdenTrabajoCab OrdenSeleccionado
        {
            get
            {
                return _ordenSeleccionado;
            }

            set
            {
                _ordenSeleccionado = value;
                OnPropertyChanged("OrdenSeleccionado");
                llamarVentana();
            }
        }

        public DateTime DateMaximum
        {
            get
            {
                return _dateMaximum;
            }
            set
            {
                _dateMaximum = value;
                OnPropertyChanged("DateMaximum");
            }
        }

        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                OnPropertyChanged("SelectedDate");
            }
        }
        public DateTime SelectedDateEnd
        {
            get
            {
                return _selectedDateEnd;
            }
            set
            {
                _selectedDateEnd = value;
                OnPropertyChanged("SelectedDateEnd");
            }
        }

        public Command BtnBuscar { get; }
        public Command BtnAgregar { get; }
        public Command LoadItemsCommand { get; }
        private DateTime _selectedDate;
        private DateTime _selectedDateEnd;
        private DateTime _dateMaximum;

        public async void OnBtnBuscarOrdenes()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Realizando Búsqueda...");
                await Task.Delay(2000);
                //await Application.Current.MainPage.DisplayAlert("Alerta: ", "\n" + _selectedDate+" - "+ _selectedDateEnd, "ok");
                if (validarFechas()) { await Application.Current.MainPage.DisplayAlert("Hernan App", "\n La fecha Inicio debe ser menor a la fecha Fin.", "Aceptar"); return; };
                var idUsuario = "1";//Preferences.Get("IdUsuario", "0");
                var json = await Globales.GetApiAppRoute("OrdenesClientes/GetOrdenesClientes", idUsuario);
                jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());
                ItemsOrden.Clear();

                if (jsonres.MENSAJE == "Ok")
                {
                    OrdenTrabajoCab c = null;
                    foreach (var item in jsonres.RESULTADO)
                    {
                        c = JsonConvert.DeserializeObject<OrdenTrabajoCab>(item.ToString());
                        ItemsOrden.Add(c);
                    }

                    PrettyInformation();
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
                UserDialogs.Instance.HideLoading();
            }
        }

        public async void OnBtnAgregarVehiculo()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Cargando...");
                await Task.Delay(1000);
                await Shell.Current.Navigation.PushAsync(new AgregarVehiculoPage());
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

        public void PrettyInformation() {
            foreach (OrdenTrabajoCab item in ItemsOrden)
            {
                item.OrdenNum = "Orden # 000" + item.IdOrdenTrabajoCab.ToString();
                item.FechaOrden = item.FechaIngreso.ToString("dd/M/yyyy");
                item.Estado = (item.Estado == null ? "ACTIVO" : item.Estado);
            }        
        
        }

        public bool validarFechas() { 
            bool aux = false;

            if (_selectedDate > _selectedDateEnd) { aux = true; }

            return aux;
        }

        public async void llamarVentana() {
            //await Application.Current.MainPage.DisplayAlert("Seleccionado: ", "\n" + _ordenSeleccionado.OrdenNum + " - "+ _ordenSeleccionado.Observacion, "Aceptar");
            UserDialogs.Instance.ShowLoading("Cargando Información...");
            await Task.Delay(2000);
            await Shell.Current.Navigation.PushAsync(new OrdenesDetallesClientesPage(_ordenSeleccionado));
        }

        public async Task GetOrdenesClientes(string cedula)
        {

            /*List<VehiculosClientes> lista = new List<VehiculosClientes>();
            try
            {
                var json = await Globales.GetApiAppRoute("VehiculosClientes/GetVehiculosClientes", cedula);
                jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());

                if (jsonres.MENSAJE == "Ok")
                {
                    VehiculosClientes c = null;
                    foreach (var item in jsonres.RESULTADO)
                    {
                        c = JsonConvert.DeserializeObject<VehiculosClientes>(item.ToString());
                        //lista.Add(c);
                        Items.Add(c);
                    }

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Automotriz Hernan Error", $"" + jsonres.STACK.ToString() + "\n", "Aceptar");
                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "\n" + e.Message, "ok");
            }
            finally
            {
                IsBusy = false;
            }*/

        }

        #region Funciones definition
        public async void SearchBar_GetVehiculos(object cedula)
        {
            string str = cedula.ToString();
            IsBusy = true;
            //await GetVehiculosClientes(str);
        }

        private async void ExecuteLoadItemsCommand(object placa)
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                string str = (placa==null ? "none" : placa.ToString());
                await GetVehiculosClientes(str);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task GetVehiculosClientes(string placa)
        {
            if (placa =="") { placa = "none"; }

            List<VehiculosClientes> lista = new List<VehiculosClientes>();
            try
            {
                Int64 idusu = Int64.Parse(Preferences.Get("IdUsuario", 0l).ToString());
                string texto = idusu.ToString()+","+ placa;
                var json = await Globales.GetApiAppRoute("VehiculosClientes/GetVehiculosClientesId", texto);
                jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());
                Items.Clear();
                if (jsonres.MENSAJE == "Ok")
                {
                    VehiculosClientes c = null;
                    foreach (var item in jsonres.RESULTADO)
                    {
                        c = JsonConvert.DeserializeObject<VehiculosClientes>(item.ToString());
                        //lista.Add(c);
                        Items.Add(c);
                    }

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Automotriz Hernan Error", $"" + jsonres.STACK.ToString() + "\n", "Aceptar");
                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "\n" + e.Message, "ok");
            }
            finally
            {
                IsBusy = false;
            }

        }
        async void OnItemSelected(VehiculosClientes item)
        {
            
        }

        #endregion
    }
}

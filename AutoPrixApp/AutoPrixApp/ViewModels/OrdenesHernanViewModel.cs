using Acr.UserDialogs;
using AutoPrixApp.Models;
using AutoPrixApp.Views.Ordenes;
using AutoPrixWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AutoPrixApp.ViewModels
{
    public class OrdenesHernanViewModel: BaseViewModel
    {

        public DateTime FechaActual = DateTime.Now;
        public List<OrdenTrabajoCab> LsOrdenes { get; set; }
        public ObservableCollection<OrdenTrabajoCab> ItemsOrden { get; }
        public OrdenTrabajoCab _ordenSeleccionado;
        private string _txtBuscar;

        public OrdenesHernanViewModel()
        {
            Title = "Ordenes";
            ItemsOrden = new ObservableCollection<OrdenTrabajoCab>();
            LsOrdenes = new List<OrdenTrabajoCab>();
            BtnBuscar = new Command(this.OnBtnBuscarOrdenes);
            DateMaximum = FechaActual;
            SelectedDate = new DateTime(FechaActual.Year, FechaActual.Month, FechaActual.Day); //FechaActual; //DateTime.Parse(FechaActual.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            SelectedDateEnd = new DateTime(FechaActual.Year, FechaActual.Month, FechaActual.Day);//FechaActual; //DateTime.Parse(FechaActual.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            OnBtnBuscarOrdenes();
        }

        public string TxtBuscar { get => _txtBuscar; set => SetProperty(ref _txtBuscar, value); }


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
                //var idUsuario = "1";//Preferences.Get("IdUsuario", "0");
                string cadena = obtenerCadenaJson();
                //var json = await Globales.GetApiAppRoute("OrdenesClientes/GetOrdenesClientes", cadena);
                var json = await Globales.PostApiApp("OrdenesClientes/PostOrdenesClientes", cadena);
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

        private string obtenerCadenaJson()
        {
            string cadena = "";
            Int64 idusu = Int64.Parse(Preferences.Get("IdUsuario", 0l).ToString());
            try {
                var my_jsondata = new
                {
                    idOrden = 0,
                    busqueda = _txtBuscar==null?"":_txtBuscar,
                    fechaInicio = _selectedDate,
                    fechaFin = _selectedDateEnd,
                    idUsuario = idusu,
                    modo = "ADM"
                };

                //Tranform it to Json object
                cadena = JsonConvert.SerializeObject(my_jsondata);

            } catch (Exception e)
            { return "Error: "+e.Message; }
            return cadena;
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
            await Shell.Current.Navigation.PushAsync(new OrdenesDetallesHernanPage(_ordenSeleccionado));
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
    }
}

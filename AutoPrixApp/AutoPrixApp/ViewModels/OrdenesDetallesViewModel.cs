using Acr.UserDialogs;
using AutoPrixApp.Models;
using AutoPrixApp.Views.MantenimientoGnrl;
using AutoPrixApp.Views.Popup;
using AutoPrixWebApi.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AutoPrixApp.ViewModels
{
    //[QueryProperty(nameof(VehiculosCli), nameof(VehiculosCli))]
    public class OrdenesDetallesViewModel : BaseViewModel
    {


        #region Variables definition
        private OrdenTrabajoCab _ordenTrabajoCab;
        public string _idOrden;
        public string _marca;
        public string _modelo;
        public string _fechaOrden;
        public string _placa;
        public string _estado;
        public string _observacion;
        public string _precion;
        public string _detLlanta;
        public bool _visibleApruebaOrden;
        public bool _visibleFinalizaOrden;
        public bool _visibleIngresoOrden;
        public bool _visibleSalidaOrden;
        #endregion

        #region Comandos definition
        public ObservableCollection<OrdenTrabajoDet> OrdenDet { get; set; }
        public ObservableCollection<ImagenTrabajos> OrdenDetImagenes { get; set; }
        public ObservableCollection<OrdenTrabajoLlantas> OrdenDetLlantas { get; set; }
        
        public Command BtnIngresoOrden { get; }
        public Command BtnSalidaOrden { get; }
        #endregion

        public OrdenesDetallesViewModel(OrdenTrabajoCab obj1) {
            try
            {
                Title = "Detalle de la Orden";
                _ordenTrabajoCab = obj1;
                llenarCamposCab();
                cargarLlantasdet();
                cargarDetTrabajosRealizados();
                UserDialogs.Instance.HideLoading();
                MuestraBotones();
                BtnIngresoOrden = new Command(this.OnBtnIngresoOrden);
                BtnSalidaOrden = new Command(this.OnBtnSalidaOrden);
            }
            catch (Exception e) {
                Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
            }
        }

        #region Constructores definition
        public string IdOrden { get => _idOrden; set => SetProperty(ref _idOrden, value); }
        public string Marca { get => _marca; set => SetProperty(ref _marca, value); }
        public string Modelo { get => _modelo; set => SetProperty(ref _modelo, value); }
        public string FechaOrden { get => _fechaOrden; set => SetProperty(ref _fechaOrden, value); }
        public string Placa { get => _placa; set => SetProperty(ref _placa, value); }
        public string Estado { get => _estado; set => SetProperty(ref _estado, value); }
        public string Observacion { get => _observacion; set => SetProperty(ref _observacion, value); }
        /*llantas*/
        public string Presion { get => _precion; set => SetProperty(ref _precion, value); }
        public string DetLlanta { get => _detLlanta; set => SetProperty(ref _detLlanta, value); }

        /*Botones Visibles*/
        public bool VisibleApruebaOrden { get => _visibleApruebaOrden; set => SetProperty(ref _visibleApruebaOrden, value); }
        public bool VisibleFinalizaOrden { get => _visibleFinalizaOrden; set => SetProperty(ref _visibleFinalizaOrden, value); }
        public bool VisibleIngresoOrden { get => _visibleIngresoOrden; set => SetProperty(ref _visibleIngresoOrden, value); }
        public bool VisibleSalidaOrden { get => _visibleSalidaOrden; set => SetProperty(ref _visibleSalidaOrden, value); }
        #endregion

        #region Funciones definition
        public void llenarCamposCab() {
            IdOrden = _ordenTrabajoCab.OrdenNum;
            Marca = _ordenTrabajoCab.Marca;
            Modelo = _ordenTrabajoCab.Modelo;
            FechaOrden = _ordenTrabajoCab.FechaOrden;
            Placa = _ordenTrabajoCab.Placa;
            Estado = _ordenTrabajoCab.Estado;
            Observacion = _ordenTrabajoCab.Observacion;
        }

        
        public async void cargarLlantasdet() {
            try
            {
                OrdenDetLlantas = new ObservableCollection<OrdenTrabajoLlantas>();
                var idCab = _ordenTrabajoCab.IdOrdenTrabajoCab.ToString();
                var json = await Globales.GetApiAppRoute("OrdenesDetallesClientes/GetOrdenesDetalleLlantas", idCab);
                jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());
                OrdenDetLlantas.Clear();

                if (jsonres.MENSAJE == "Ok")
                {
                    OrdenTrabajoLlantas c = null;
                    foreach (var item in jsonres.RESULTADO)
                    {
                        c = JsonConvert.DeserializeObject<OrdenTrabajoLlantas>(item.ToString());
                        switch (c.CodPosicion) {
                            case "1":
                                c.CodPosicion = c.CodPosicion + ".- Llanta delantera derecha";
                                break;
                            case "2":
                                c.CodPosicion = c.CodPosicion + ".- Llanta delantera izquierda";
                                break;
                            case "3":
                                c.CodPosicion = c.CodPosicion + ".- Llanta posterior derecha";
                                break;
                            case "4":
                                c.CodPosicion = c.CodPosicion + ".- Llanta posterior izquierda";
                                break;
                        }
                        OrdenDetLlantas.Add(c);
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

        public async void cargarDetTrabajosRealizados()
        {
            try
            {
                OrdenDet = new ObservableCollection<OrdenTrabajoDet>();
                var idCab = _ordenTrabajoCab.IdOrdenTrabajoCab.ToString();
                var json = await Globales.GetApiAppRoute("OrdenesDetallesClientes/GetOrdenesDetallesClientes", idCab);
                jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());
                OrdenDet.Clear();

                if (jsonres.MENSAJE == "Ok")
                {
                    OrdenTrabajoDet c = null;
                    foreach (var item in jsonres.RESULTADO)
                    {
                        c = JsonConvert.DeserializeObject<OrdenTrabajoDet>(item.ToString());
                        OrdenDet.Add(c);
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

        public void MuestraBotones()
        {
            Int64 idusu = Int64.Parse(Preferences.Get("IdRol", 0l).ToString());
            string estadoOrden = _ordenTrabajoCab.Estado;
            switch (idusu)
            {
                case 1: //ADMIN
                    _visibleApruebaOrden = true;
                    _visibleFinalizaOrden = true;
                    _visibleIngresoOrden = true;
                    _visibleSalidaOrden = true;
                    break;
                case 2: //EMPLEADO
                    _visibleApruebaOrden = false;
                    _visibleFinalizaOrden = true;
                    _visibleIngresoOrden = true;
                    _visibleSalidaOrden = true;
                    break;
                case 3: //CLIENTE
                    _visibleApruebaOrden = false;
                    _visibleFinalizaOrden = false;
                    _visibleIngresoOrden = true;
                    _visibleSalidaOrden = true;
                    break;
            }

            if (estadoOrden.Equals("ACT") || estadoOrden.Equals("ACTIVO"))
            {
                _visibleSalidaOrden = false;
            }
            if (estadoOrden.Equals("AUT") || estadoOrden.Equals("AUTORIZADO"))
            {
                _visibleSalidaOrden = false;
                _visibleApruebaOrden = false;
            }
            if (estadoOrden.Equals("EFE") || estadoOrden.Equals("FINALIZADO"))
            {
                _visibleSalidaOrden = true;
                _visibleApruebaOrden = false;
                _visibleFinalizaOrden = false;
            }
        }

        public async void OnBtnIngresoOrden()
        {
            try
            {

                bool Conection = CrossConnectivity.Current.IsConnected;
                if (Conection)
                {
                    UserDialogs.Instance.ShowLoading("Cargando Imagenes...");
                    await Task.Delay(2000);

                    Int64 idusu = Int64.Parse(Preferences.Get("IdUsuario", 0l).ToString());
                    await PopupNavigation.Instance.PushAsync(new PopupVerIngresoVehiculoView(_ordenTrabajoCab.IdOrdenTrabajoCab, "ING"));

                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
            }
            finally
            {
                //UserDialogs.Instance.HideLoading();
            }
        }

        public async void OnBtnSalidaOrden()
        {
            try
            {

                bool Conection = CrossConnectivity.Current.IsConnected;
                if (Conection)
                {
                    UserDialogs.Instance.ShowLoading("Cargando Imagenes...");
                    await Task.Delay(2000);

                    Int64 idusu = Int64.Parse(Preferences.Get("IdUsuario", 0l).ToString());
                    await PopupNavigation.Instance.PushAsync(new PopupVerIngresoVehiculoView(_ordenTrabajoCab.IdOrdenTrabajoCab, "SAL"));

                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
            }
            finally
            {
                //UserDialogs.Instance.HideLoading();
            }
        }
        #endregion
    }
}

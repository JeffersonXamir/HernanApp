using AutoPrixApp.Models;
using AutoPrixApp.Views.MantenimientoGnrl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AutoPrixApp.ViewModels
{
    [QueryProperty(nameof(_OrdenTrabajoCab), nameof(_OrdenTrabajoCab))]
    [QueryProperty(nameof(_Imagenes), nameof(_Imagenes))]
    public class EstadoLlantasViewModel : BaseViewModel
    {
        OrdenTrabajoCab Obj;
        List<ImagenTrabajos> Lsitems;
        public EstadoLlantasViewModel(OrdenTrabajoCab obj,List<ImagenTrabajos> items, Int64 tipoTrabajo) {

            BtnContinuar = new Command( this.OnBtnContinuar);
            Title = "Estado de Llantas";
            Obj = obj;
            Lsitems = items;
            TipoTrabajo = tipoTrabajo;
        }

        #region Variables definition
        private string _ordenTrabajoCab { get; set; }
        private string _imagenes { get; set; }
        private string presion1Observ;
        private string presion2Observ;
        private string presion3Observ;
        private string presion4Observ;
        private Decimal presion1;
        private Decimal presion2;
        private Decimal presion3;
        private Decimal presion4;
        public Int64 TipoTrabajo;
        #endregion

        #region Comandos definition
        public Command BtnContinuar { get; }
        #endregion
       
        #region Parametros definition
        public string _OrdenTrabajoCab
        {
            get
            {
                return _ordenTrabajoCab;
            }
            set
            {
                _ordenTrabajoCab = value;
                LoadCabecera(value);
            }
        }
        public string _Imagenes
        {
            get
            {
                return _imagenes;
            }
            set
            {
                _imagenes = value;
                LoadImagenes(value);
            }
        }
        public string Presion1Observ { get => presion1Observ; set => SetProperty(ref presion1Observ, value); }
        public string Presion2Observ { get => presion2Observ; set => SetProperty(ref presion2Observ, value); }
        public string Presion3Observ { get => presion3Observ; set => SetProperty(ref presion3Observ, value); }
        public string Presion4Observ { get => presion4Observ; set => SetProperty(ref presion4Observ, value); }
        public Decimal Presion1 { get => presion1; set => SetProperty(ref presion1, value); }
        public Decimal Presion2 { get => presion2; set => SetProperty(ref presion2, value); }
        public Decimal Presion3 { get => presion3; set => SetProperty(ref presion3, value); }
        public Decimal Presion4 { get => presion4; set => SetProperty(ref presion4, value); }
        
        #endregion

        #region Funciones definition
        public async void OnBtnContinuar()
        {
            try {
                List<OrdenTrabajoLlantas> objLlantas = validarCampos();
                if (objLlantas == null)
                    return;

                //var ObjTrabLlantas = JsonConvert.SerializeObject(objLlantas);

                //await Shell.Current.GoToAsync($"{nameof(TrabajoDetallePage)}?{nameof(TrabajoDetalleViewModel._OrdenTrabajoCab)}={_OrdenTrabajoCab}/{nameof(TrabajoDetalleViewModel._Imagenes)}={_Imagenes}/{nameof(TrabajoDetalleViewModel._OrdenTrabajoLlantas)}={ObjTrabLlantas}");
                await Shell.Current.Navigation.PushAsync(new TrabajoDetallePage(Obj,Lsitems,objLlantas, TipoTrabajo));
            }
            catch (Exception e) {
                await Application.Current.MainPage.DisplayAlert("AutoServicio Hernan", "\n" + e.Message, "ok");
            }
        }

        private List<OrdenTrabajoLlantas> validarCampos()
        {
            OrdenTrabajoLlantas objLlantas = null;
            List<OrdenTrabajoLlantas> lsllantas = new List<OrdenTrabajoLlantas>();
            try
            {
                if (Presion1.Equals("") || Presion1.Equals(0))
                {
                    throw new Exception("La Presión 1 debe ser mayor a 0");
                }
                var prec1 = Presion1 > 0 ? Presion1 : 0;
                objLlantas = new OrdenTrabajoLlantas();
                objLlantas.CodPosicion = "1";
                objLlantas.Estado = "A";
                objLlantas.Presion = Decimal.Parse(prec1.ToString().Replace(",", ".")); //Double.Parse(prec1.ToString().Replace(",", "."));
                objLlantas.Observacion = Presion1Observ;
                lsllantas.Add(objLlantas);
                objLlantas = null;

                if (Presion2.Equals("") || Presion2.Equals(0))
                {
                    throw new Exception("Presión 2 debe ser mayor a 0");
                }
                var prec2 = Presion2 > 0 ? Presion2 : 0;
                objLlantas = new OrdenTrabajoLlantas();
                objLlantas.CodPosicion = "2";
                objLlantas.Estado = "A";
                objLlantas.Presion = Decimal.Parse(prec2.ToString().Replace(",", "."));
                objLlantas.Observacion = Presion2Observ;
                lsllantas.Add(objLlantas);
                objLlantas = null;

                if (Presion3.Equals("") || Presion3.Equals(0))
                {
                    throw new Exception("Presión 3 debe ser mayor a 0");
                }
                var prec3 = Presion3 > 0 ? Presion3 : 0;
                objLlantas = new OrdenTrabajoLlantas();
                objLlantas.CodPosicion = "3";
                objLlantas.Estado = "A";
                objLlantas.Presion = Decimal.Parse(prec3.ToString().Replace(",", "."));
                objLlantas.Observacion = Presion3Observ;
                lsllantas.Add(objLlantas);
                objLlantas = null;

                if (Presion4.Equals("") || Presion4.Equals(0))
                {
                    throw new Exception("Presión 4 debe ser mayor a 0");
                }
                var prec4 = Presion4 > 0 ? Presion4 : 0;
                objLlantas = new OrdenTrabajoLlantas();
                objLlantas.CodPosicion = "4";
                objLlantas.Estado = "A";
                objLlantas.Presion = Decimal.Parse(prec4.ToString().Replace(",", "."));
                objLlantas.Observacion = Presion4Observ;
                lsllantas.Add(objLlantas);
                objLlantas = null;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return lsllantas;
        }

        public async void LoadImagenes(string item)
        {
            try
            {
                _Imagenes = Uri.UnescapeDataString(item.ToString());
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
            }
        }
        public async void LoadCabecera(string item)
        {
            try
            {
                _OrdenTrabajoCab = Uri.UnescapeDataString(item.ToString());
                /*VehiculosClientes jsonObj = JsonConvert.DeserializeObject<VehiculosClientes>(Uri.UnescapeDataString(item.ToString()));
                Vehiculo = jsonObj.vehiculo;
                Placa = jsonObj.placa;
                Cliente = jsonObj.cliente;*/
            }
            catch (Exception e)
            {             
                await Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
            }
        }
        #endregion
    }
}

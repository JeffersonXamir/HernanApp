using Acr.UserDialogs;
using AutoPrixApp.Models;
using AutoPrixApp.Views.MantenimientoGnrl;
using AutoPrixWebApi.Models;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
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
    [QueryProperty(nameof(VehiculosCli), nameof(VehiculosCli))]
    public class FinalizaOrdenViewModel : BaseViewModel
    {


        #region Variables definition
        private Decimal kilometraje;
        private string observacion;
        private string cliente;
        private string vehiculo;
        private string placa;
        private string itemId;
        private string vehiculosCli;
        VehiculosClientes jsonObj;
        public Int64 TipoTrabajo;
        public Int64 _IdOrden;

        #endregion

        #region Comandos definition
        public ObservableCollection<FormaPago> Items { get; set; }
        public ObservableCollection<Imagen> listaImagenes { get; set; }
        public Command<Imagen> ItemTapped { get; }
        public ImageSource source { get; set; }
        public Command BtnContinuar { get; set; }
        public Command BtnCamera { get; set; }

        #endregion

        public FinalizaOrdenViewModel(VehiculosClientes obj1,Int64 idOrden) {
            try
            {
                Title = "Finalizar Orden";
                this.BtnContinuar = new Command(this.GotoRegistroTrebajo);
                this.BtnCamera = new Command(this.GotoBtnCamera);
                this.listaImagenes = new ObservableCollection<Imagen>();
                this.ItemTapped = new Command<Imagen>(this.onItemTapped);
                this.Items = new ObservableCollection<FormaPago>()
                {
                   /* new FormaPago(1,"CONTADO"),
                    new FormaPago(2,"CREDITO"),
                    new FormaPago(3,"3 MESES"),
                    new FormaPago(4,"15 MESES")*/
                };
                observacion = "";
                jsonObj = obj1;
                Vehiculo = jsonObj.vehiculo;
                Placa = jsonObj.placa;
                Cliente = jsonObj.cliente;
                _IdOrden = idOrden;
            }
            catch (Exception e) {
                Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
            }
        }
       
        #region propiedades definition
        public Decimal Kilometraje { get => kilometraje; set => SetProperty(ref kilometraje, value); }
        public string Observacion { get => observacion; set => SetProperty(ref observacion, value); }
        public string Vehiculo { get => vehiculo; set => SetProperty(ref vehiculo, value); }
        public string Placa { get => placa; set => SetProperty(ref placa, value); }
        public string Cliente { get => cliente; set => SetProperty(ref cliente, value); }
        public ObservableCollection<Imagen> ListaImagenes
        {
            get => listaImagenes;
            set
            {
                if (value != listaImagenes)
                {
                    listaImagenes = value;
                    OnPropertyChanged(nameof(ListaImagenes));
                }
            }
        }
        public ImageSource Source
        {
            get => source;
            set
            {
                if (value != source)
                {
                    source = value;
                    OnPropertyChanged(nameof(Source));
                }
            }
        }

        public string VehiculosCli
        {
            get
            {
                return vehiculosCli;
            }
            set
            {
                vehiculosCli = value;
                LoadItemId(value);
            }
        }
        public IList<string> CountryList
        {
            get
            {
                return new List<string> { "CONTADO", "CREDITO", "3 MESES", "15 MESES" };
            }
        }

     
       
        #endregion

        #region Funciones definition
        public async void LoadItemId(string item)
        {
            try
            {
                jsonObj = JsonConvert.DeserializeObject<VehiculosClientes>(Uri.UnescapeDataString(item.ToString()));
                Vehiculo = jsonObj.vehiculo;
                Placa = jsonObj.placa;
                Cliente = jsonObj.cliente;
            }
            catch (Exception e)
            {
                //Debug.WriteLine("Failed to Load Item");
                await Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
            }
        }

        public async void GotoBtnCamera() {
            try
            {

            IsBusy = true;
            var cameraOptions = new StoreCameraMediaOptions();
            cameraOptions.PhotoSize = PhotoSize.Medium;
            cameraOptions.SaveToAlbum = true;

            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
            if (photo != null)
            {
                try
                {
                    
                    Source = ImageSource.FromStream(() => { return photo.GetStream(); });
                    Imagen item = new Imagen();
                    item.Id = listaImagenes.Count > 0 ? ListaImagenes.Max(x => x.Id) + 1 : 1;
                    item.SrcUrl = photo.Path;
                    item.Name = Path.GetFileName(photo.Path); //Path.GetFileNameWithoutExtension(filePath);
                    item.UrlSource = ImageSource.FromStream(() => { return photo.GetStream(); });
                    item.Tipo = Path.GetExtension(photo.Path);//==>Extension //Path.GetFileNameWithoutExtension(photo.Path);=> solo name
                    //var stream = photo.GetStream();
                    var stream = photo.GetStream();
                    var bytes = new byte[stream.Length];
                    item.ImageData = bytes;
                    await stream.ReadAsync(bytes, 0, (int)stream.Length);
                        //string base64 = System.Convert.ToBase64String(bytes);
                        //item.Base64 = base64;
                        item.Base64 = "x00";
                    ListaImagenes.Add(item);
                    
                }
                catch (Exception e)
                {
                    await Application.Current.MainPage.DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured it in Xamarin Insights! Thanks.", "OK");
                }
                finally { IsBusy = false; }
            }
            IsBusy = false;
            }
            catch (Exception ed)
            {
                await Application.Current.MainPage.DisplayAlert("Hernan Error Camera", "error: " + ed.Message.ToString() + " ", "OK");
            }
        }
       
        public async void GotoRegistroTrebajo() {
            UserDialogs.Instance.ShowLoading("Finalizando Trabajo...");
            while (IsBusy != false)
            {
                await Task.Delay(4000);
            }
            try
            {
                if (Observacion.Equals("") || Observacion.Length < 2){ await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", "\n" + "Ingrese una Observación", "ok"); return; }
                var resp = await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", "¿Esta Seguro de Finalizar El Trabajo?", "Aceptar", "Cancelar");
                if (resp)
                {
                    
                    List<ImagenTrabajos> lsimagen = ObtenerImagenes();
                    String imgObj = "";

                    var cadena = obtenerCadenaJson("AUTORIZAORDEN");
                    var json = await Globales.PostApiApp("OrdenesClientes/PostAutorizarOrden", cadena);
                    jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());

                    if (jsonres.MENSAJE == "Ok")
                    {
                        foreach (ImagenTrabajos obj in lsimagen)
                        {
                            obj.IdOrdenTrabajoCab = _IdOrden;
                            imgObj = JsonConvert.SerializeObject(obj);
                            var jsonImage = await Globales.PostApiApp("imagenes/PostGuardarImagen", imgObj);
                            imgObj = "";
                        }

                        await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", $"" + jsonres.STACK.ToString() + "\n", "Aceptar");
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
                    
                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Automotriz Hernan Error", "\n" + e.Message, "ok");
            }
            finally {
                UserDialogs.Instance.HideLoading();
            }
        }

        private List<ImagenTrabajos> ObtenerImagenes()
        {
            ImagenTrabajos obj = null;
            List<ImagenTrabajos> Lsimagenes = new List<ImagenTrabajos>();
            try {
                foreach (Imagen item in ListaImagenes) {
                    obj = new ImagenTrabajos();
                    obj.Imagen = item.Base64;
                    obj.Tipo = item.Tipo;
                    obj.Descripcion = item.Name;
                    obj.IdImagen = item.Id;
                    obj.UsuarioCreacion = Int64.Parse(Preferences.Get("IdUsuario", 0l).ToString());
                    obj.FechaCreacion = DateTime.Parse(DateTime.Now.ToString()); //dd-MM-yyyy HH:mm:ss
                    obj.SourceImage = item.ImageData;
                    //obj.ImageData = item.ImageData;
                    obj.Estado = "SAL";
                    Lsimagenes.Add(obj);
                    obj = null;
                }
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
            return Lsimagenes;
        }

        private OrdenTrabajoCab ValidarCampos()
        {
            OrdenTrabajoCab objcab = new OrdenTrabajoCab();
            try {
                
                if (Observacion.Equals("") || Observacion.Length < 2) {
                    throw new Exception("Ingrese una Observación");
                }
                objcab.Observacion = Observacion;
                DateTime fechaActual = DateTime.Now;//DateTime.Parse(DateTime.Now.Date.ToString("dd-MM-yyyy"));
                DateTime HoraActual = DateTime.Now;//DateTime.Parse(DateTime.Now.ToString("HH:mm:ss"));
                objcab.FechaIngreso = fechaActual;
                objcab.HoraIngreso = HoraActual; 
                objcab.IdClienteVehiculo = jsonObj.id;
                var idusuario = Preferences.Get("IdUsuario", 0l);
                objcab.UsuarioRecepcion = Int64.Parse(idusuario.ToString()); 
                objcab.UsuarioCreacion = Int64.Parse(idusuario.ToString());
                objcab.FechaCreacion = DateTime.Parse(DateTime.Now.Date.ToString());
                
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
            return objcab;
        }

        private async void onItemTapped(Imagen item)
        {
            try {
                //Debug.WriteLine(item.UrlSource);
                //Debug.WriteLine(item.Base64);
                //Console.WriteLine(item.UrlSource);
                var op = await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", "¿Desea Eliminar Esta Foto?", "Si","No");
                if (op) {
                    foreach (Imagen ls in ListaImagenes) {
                        Console.WriteLine("item for "+ls.Id);
                        if (ls.Id == item.Id) {
                            ListaImagenes.Remove(ls);                            
                            break;
                        }
                    }
                }
                //await Application.Current.MainPage.DisplayAlert("error", "\n" + item.Id +"/"+item.Name, "ok");
            } catch (Exception e) {
                await Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
                //System.Diagnostics.Debug.WriteLine("Error occured while checking if image is available: " + ex);
            }
        }

        private string obtenerCadenaJson(string modo)
        {
            string cadena = "";
            Int64 idusu = Int64.Parse(Preferences.Get("IdUsuario", 0l).ToString());
            try
            {
                switch (modo)
                {
                    case "AUTORIZAORDEN":
                        var my_jsondata1 = new
                        {
                            idOrden = _IdOrden,
                            idUsuario = idusu,
                            idEmpleado = idusu,
                            observacion = observacion,
                            modo = "SAL"
                        };
                        cadena = JsonConvert.SerializeObject(my_jsondata1);
                        break;
                    case "EMPLEADOS":
                        var my_jsondata2 = new
                        {
                            idOrden = 0,
                            idUsuario = idusu,
                            modo = "E"
                        };
                        cadena = JsonConvert.SerializeObject(my_jsondata2);
                        break;
                }

            }
            catch (Exception e)
            { return "Error: " + e.Message; }
            return cadena;
        }

        #endregion

    }
}

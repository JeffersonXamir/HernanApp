using Acr.UserDialogs;
using AutoPrixApp.Models;
using AutoPrixWebApi.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace AutoPrixApp.ViewModels
{
    [QueryProperty(nameof(_OrdenTrabajoCab), nameof(_OrdenTrabajoCab))]
    [QueryProperty(nameof(_Imagenes), nameof(_Imagenes))]
    [QueryProperty(nameof(_OrdenTrabajoLlantas), nameof(_OrdenTrabajoLlantas))]
    public class TrabajoDetalleViewModel : BaseViewModel
    {
        OrdenTrabajoCab Obj;
        List<ImagenTrabajos> Lsitems;
        List<OrdenTrabajoLlantas> Lsllantas;
        public TrabajoDetalleViewModel(OrdenTrabajoCab obj, List<ImagenTrabajos> items, List<OrdenTrabajoLlantas> LsLlantas, Int64 tipoTrabajo)
        {

            Title = "Detalle del Trabajo";
            ItemTapped = new Command<ListItem>(this.onItemSelected);
            OnGuardarTrabajo = new Command(this.OnItemSelected2);
            GetTrabajos = new Command(this.ObtenerTrabajos);
            //Accept = new Command(this.OnAccept);
            
            Obj = obj; Lsitems = items; Lsllantas = LsLlantas;
            TipoTrabajo = tipoTrabajo;
            Cargarcontenido();
            //OnItemTapped = new Command<LifeDemandData>(this.onItemSelected);
            Items = new ObservableCollection<FormaPago>()
                {
                    /*new FormaPago(1,"CONTADO"),
                    new FormaPago(2,"CREDITO"),
                    new FormaPago(3,"3 MESES"),
                    new FormaPago(4,"15 MESES")*/
                };
            Accept = new Command(this.Onaccept);
            LifeDemandList = new ObservableCollection<ListItem>()
            {
                /* new ListItem{ Id = 1, CheckboxImage = "uncheck.png", ItemImage = "level1.png", ItemName = "Cambios de aceite y filtros" ,isVisibleDetail = false},
                 new ListItem{ Id = 2, CheckboxImage = "uncheck.png", ItemImage = "level1.png", ItemName = "Alineación y balanceo" ,isVisibleDetail = false},
                 new ListItem{ Id = 3, CheckboxImage = "uncheck.png", ItemImage = "level1.png", ItemName = "Neumáticos" ,isVisibleDetail = false},
                 new ListItem{ Id = 4, CheckboxImage = "uncheck.png", ItemImage = "level1.png", ItemName = "Montacargas" ,isVisibleDetail = false},*/
                // new ListItem{ Id = 5, CheckboxImage = "check.png", ItemImage = "level1.png", ItemName = "Item5" ,isVisibleDetail = false},

            };
            //MultiSelectListView.ItemsSource = multiSelectListItems;

        }

        public async void Cargarcontenido()
        {
            //await Application.Current.MainPage.DisplayAlert("AutoPrix Info", "Yeah "+Obj.Observacion+"|"+Lsitems.Count+"|"+Lsllantas.Count+"" + "." + "", "ok");
            await OnCargarTrabajos(TipoTrabajo.ToString());
            await OnCargarFormaPagos();
        }
        #region Variables definition
      private string _myCity;
        public string MyCity
        {
            get { return _myCity; }
            set
            {
                if (_myCity != value)
                {
                    _myCity = value;
                    OnPropertyChanged();
                }
            }
        }
        private FormaPago _selectedCity { get; set; }
        public FormaPago SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;
                    // Do whatever functionality you want...When a selectedItem is changed..
                    // write code here..
                    MyCity = "Selected City : " + _selectedCity.Descripcion;
                }
            }
        }
     

        public ObservableCollection<ListItem> lifeDemandList;
        public ObservableCollection<ListItem> LifeDemandList
        {
            get => lifeDemandList; set
            {
                if (value != lifeDemandList)
                {
                    lifeDemandList = value;
                    OnPropertyChanged(nameof(LifeDemandList));
                }
            }
        }
        public ObservableCollection<ListItem> selectedList;
        private string _ordenTrabajoCab { get; set; }
        private string _imagenes { get; set; }
        private string _ordenTrabajoLlantas { get; set; }
        private bool hasItems;
        public Int64 TipoTrabajo;
        //public ObservableCollection<FormaPago> Items { get; set; }
        public ObservableCollection<FormaPago> items;
        public ObservableCollection<FormaPago> Items
        {
            get => items; set
            {
                if (value != items)
                {
                    items = value;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }
        #endregion

        #region Comandos definition
        public Command<ListItem> ItemTapped { get; }
        public Command OnGuardarTrabajo { get; }
        public Command GetTrabajos { get; }
        public Command OnItemTapped { get; }
        public ICommand Accept { get; set; }
        public Command RadListPicker_SelectionChanged { get; set; }
        #endregion

        //private bool hasItems;
        //public bool HasItems { get => hasItems; set { hasItems = value; } }

        #region Propiedades definition
        public bool HasItems
        {
            get => hasItems;
            set
            {
                if (value != hasItems)
                {
                    hasItems = value;
                    OnPropertyChanged(nameof(HasItems));
                }
            }
        }
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
        public string _OrdenTrabajoLlantas
        {
            get
            {
                return _ordenTrabajoLlantas;
            }
            set
            {
                _ordenTrabajoLlantas = value;
                LoadTrabajoLlantas(value);
            }
        }
        #endregion

        #region Funciones definition
        public void VisibleFrameDetalle()
        {
            int count = LifeDemandList.Where(x => x.CheckboxImage == "check.png").Count();
            hasItems = count > 0 ? true : false;
            OnPropertyChanged();
        }

        private async void onItemSelected(ListItem item)
        {

            try
            {
                UserDialogs.Instance.ShowLoading("Espere un momento...");
                selectedList = new ObservableCollection<ListItem>();
                foreach (var x in LifeDemandList)
                {
                    if (x.Id == item.Id)
                    {
                        x.CheckboxImage = x.CheckboxImage.Equals("uncheck.png") ? "check.png" : "uncheck.png";
                    }
                    selectedList.Add(x);
                }

                LifeDemandList.Clear();
                foreach (var y in selectedList)
                {
                    //if (y.CheckboxImage == "check.png") { y.isVisibleDetail = true; }
                    if (y.Id == item.Id)
                    {
                        y.isVisibleDetail = y.CheckboxImage.Equals("check.png") ? true : false;
                    }
                    LifeDemandList.Add(y);
                }

                var count = LifeDemandList.Where(x => x.CheckboxImage == "check.png").Count();
                HasItems = count > 0 ? true : false;
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception e)
            {
                UserDialogs.Instance.HideLoading();
                await Application.Current.MainPage.DisplayAlert("Automotriz Hernan Error", "\n " + e.Message + "." + "", "ok");
            }
        }


        async void OnItemSelected2()
        {
            try
            {
                bool Conection = CrossConnectivity.Current.IsConnected;
                if (Conection)
                {
                    UserDialogs.Instance.ShowLoading("Guardando Trabajo...");
                    List<OrdenTrabajoDet> lsdetalles = validarCampos();
                    //if (lsdetalles == null) { return; }
                    GuardarTrabajo GuardarObj = new GuardarTrabajo();
                    GuardarObj.CabeceraTrab = Obj;
                    GuardarObj.LlantasTrab = Lsllantas;
                    //GuardarObj.ImagenTrab = Lsitems;
                    GuardarObj.DetallesTrab = lsdetalles;

                    String jsonGuardar = JsonConvert.SerializeObject(GuardarObj);

                    var resp = await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", "¿Esta Seguro de Guardar El Trabajo?", "Aceptar", "Cancelar");
                    if (resp)
                    {
                        var json = await Globales.PostApiApp("trabajos/PostGuardarTrabajo", jsonGuardar);  //pendiente 

                        jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());

                        if (jsonres.MENSAJE == "Ok")
                        {
                            String imgObj = "";
                            foreach (ImagenTrabajos obj in Lsitems)
                            {
                                obj.IdOrdenTrabajoCab = Int64.Parse(jsonres.STACK);
                                imgObj = JsonConvert.SerializeObject(obj);
                                var jsonImage = await Globales.PostApiApp("imagenes/PostGuardarImagen", imgObj);
                                imgObj = "";
                            }

                            UserDialogs.Instance.HideLoading();
                            await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", $"Exito Al Guardar Tabajo" + "\n", "Aceptar");
                            await Shell.Current.GoToAsync("//AboutPage");
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", $"" + jsonres.STACK.ToString() + "\n", "Aceptar");
                        }
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
                await Application.Current.MainPage.DisplayAlert("Automotriz Hernan", "\n" + e.Message, "ok");
            }
        }

        private List<OrdenTrabajoDet> validarCampos()
        {
            OrdenTrabajoDet detalles = null;
            List<OrdenTrabajoDet> lsdetalles = new List<OrdenTrabajoDet>();
            try
            {
                foreach (ListItem item in LifeDemandList)
                {
                    if (item.CheckboxImage.Equals("check.png"))
                    {
                        detalles = new OrdenTrabajoDet();
                        detalles.IdTrabajoAutorizado = item.Id;
                        detalles.Estado = "A";
                        detalles.Detalle = item.CVC;
                        detalles.UsuarioCreacion = Int64.Parse(Preferences.Get("IdUsuario", 0l).ToString());
                        detalles.FechaCreacion = DateTime.Parse(DateTime.Now.ToString()); //dd-MM-yyyy HH:mm:ss
                        lsdetalles.Add(detalles);
                        detalles = null;
                    }
                }

                if (lsdetalles.Count <= 0) {  throw new Exception("Seleccione un trabajo a realizar"); }
                if (SelectedCity == null || SelectedCity.IdFormaPago <= 0) {  throw new Exception("Seleccione una forma de pago"); }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return lsdetalles;
        }
        private void Onaccept(object sender)
        {
            Obj.idFormaPago = SelectedCity.IdFormaPago;
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
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
            }
        }
        public async void LoadTrabajoLlantas(string item)
        {
            try
            {
                _OrdenTrabajoLlantas = Uri.UnescapeDataString(item.ToString());
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
            }
        }
        async Task OnCargarTrabajos(string parametros)
        {
            try
            {
                //String parametros = TipoTrabajo.ToString();
                var json = await Globales.GetApiAppRoute("trabajos/GetTrabajos", parametros);
                jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());
                ListItem obj = null;
                if (jsonres.MENSAJE == "Ok")
                {
                    Trabajos c = null;
                    int count = 0;
                    ObservableCollection<ListItem> lista = new ObservableCollection<ListItem>();
                    foreach (ListItem item in lifeDemandList)
                    {
                        lista.Add(item);
                    }
                    LifeDemandList.Clear();
                    foreach (var item in jsonres.RESULTADO)
                    {
                        c = JsonConvert.DeserializeObject<Trabajos>(item.ToString());
                        //lsusuario.Add(c);
                        obj = new ListItem { Id = c.IdTrabajoAutorizado, CheckboxImage = "uncheck.png", ItemImage = "level1.png", ItemName = c.Descripcion, isVisibleDetail = false };
                        LifeDemandList.Add(obj);
                        obj = null;
                        count++;
                    }
                    foreach (ListItem item in lista)
                    {
                        foreach (ListItem item2 in lifeDemandList)
                        {
                            if (item.Id == item2.Id)
                            {
                                item2.CheckboxImage = item.CheckboxImage;
                                item2.ItemName = item.ItemName;
                                item2.CVC = item.CVC;
                                item2.isVisibleDetail = item.isVisibleDetail;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "\n" + e.Message, "ok");
            }
        }
        async Task OnCargarFormaPagos()
        {
            try
            {
                //String parametros = TipoTrabajo.ToString();
                var json = await Globales.GetApiAppRoute("formapago/GetFormaPagos", "");
                jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());
                
                if (jsonres.MENSAJE == "Ok")
                {
                    FormaPago c = null;
                    foreach (var item in jsonres.RESULTADO)
                    {
                        c = new FormaPago();
                        c = JsonConvert.DeserializeObject<FormaPago>(item.ToString());
                        Items.Add(c);
                    }
                    
                }

            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "\n" + e.Message, "ok");
            }
        }
        async void ObtenerTrabajos()
        {
            await OnCargarTrabajos("0");

        }
        #endregion
    }
}

using AutoPrixApp.Models;
using AutoPrixApp.Views;
using AutoPrixApp.Views.MantenimientoGnrl;
using AutoPrixWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AutoPrixApp.ViewModels
{
    public class MantenimientoGnrlViewModel: BaseViewModel
    {
        
        public ObservableCollection<VehiculosClientes> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command PerformSearch { get; set; }
        public Command<VehiculosClientes> ItemTapped { get; }
        
        public MantenimientoGnrlViewModel()
        {
            Title = "General";
            Items = new ObservableCollection<VehiculosClientes>();
            LoadItemsCommand = new Command(this.ExecuteLoadItemsCommand);
            ItemTapped = new Command<VehiculosClientes>(OnItemSelected);
            //PerformSearch = new Command(this.SearchBar_GetVehiculos);
            PerformSearch = new Command(SearchBar_GetVehiculos);
        }

        public List<VehiculosClientes> ObtenerVehiculos() {

            List<VehiculosClientes> lsVehiculos = new List<VehiculosClientes>();
            lsVehiculos.Add(new VehiculosClientes() { id = 1, cedula = "0924876014", cliente = "Jefferson Anchundia", placa = "GSM-8727",vehiculo="NISSAN TIIDA ENTRY"});
            lsVehiculos.Add(new VehiculosClientes() { id = 2, cedula = "1300696364", cliente = "Ramon Anchundia", placa = "GSN-1084", vehiculo = "JEEP GRAND CHEROKEE" });
            lsVehiculos.Add(new VehiculosClientes() { id = 3, cedula = "0906365101", cliente = "Ursula Peñaherrera Martinez", placa = "SVD-9865", vehiculo = "MERCEDES BENZ E320" });

            return lsVehiculos;
        }

        private async void ExecuteLoadItemsCommand(object cedula)
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                string str = cedula.ToString();
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

        async void OnItemSelected(VehiculosClientes item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
            var cadenaObj = JsonConvert.SerializeObject(item);
            try
            {
                //await Shell.Current.GoToAsync($"{nameof(RecepcionVehiculoPage)}?{nameof(RecepcionVehiculoViewModel.VehiculosCli)}={cadenaObj.ToString()}");
                var id = (int)EnumTipos.MantenimientoGnrl;
                await Shell.Current.Navigation.PushAsync(new RecepcionVehiculoPage(item, Int64.Parse(id.ToString())));
                
            }
            catch (Exception e) {
               await Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
            }
                //await Application.Current.MainPage.DisplayAlert("AutoPrix", "\n " + item.placa + "." + "", "ok");
            //await Shell.Current.GoToAsync("RecepcionVehiculo");
        }

        public async void SearchBar_GetVehiculos(object cedula)
        {
            string str = cedula.ToString();
            IsBusy = true;
            //await GetVehiculosClientes(str);
        }

        #region Funciones definition
        public async Task GetVehiculosClientes(string cedula)
        {
            
            List<VehiculosClientes> lista = new List<VehiculosClientes>();
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
            finally {
                IsBusy = false;
            }
            
        }

        #endregion
    }
}

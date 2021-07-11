using AutoPrixApp.Models;
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
    public class PreventivosViewModel : BaseViewModel
    {
        public PreventivosViewModel()
        {
            Title = "Preventivos";
            Items = new ObservableCollection<VehiculosClientes>();
            LoadItemsCommand = new Command(this.ExecuteLoadItemsCommand);
            ItemTapped = new Command<VehiculosClientes>(OnItemSelected);
            //PerformSearch = new Command(this.SearchBar_GetVehiculos);
            PerformSearch = new Command(SearchBar_GetVehiculos);
        }
        #region variables definition
        public ObservableCollection<VehiculosClientes> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command PerformSearch { get; set; }
        public Command<VehiculosClientes> ItemTapped { get; }

        #endregion

        #region Funciones definition
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

            //var cadenaObj = JsonConvert.SerializeObject(item);
            try
            {
                //await Shell.Current.GoToAsync($"{nameof(RecepcionVehiculoPage)}?{nameof(RecepcionVehiculoViewModel.VehiculosCli)}={cadenaObj.ToString()}");
                var id = (int)EnumTipos.Preventivos;
                await Shell.Current.Navigation.PushAsync(new RecepcionVehiculoPage(item, Int64.Parse(id.ToString())));

            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("error", "\n" + e.Message, "ok");
            }

        }

        public async void SearchBar_GetVehiculos(object cedula)
        {
            string str = cedula.ToString();
            IsBusy = true;
            //await GetVehiculosClientes(str);
        }

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
            finally
            {
                IsBusy = false;
            }

        }
        #endregion
    }
}

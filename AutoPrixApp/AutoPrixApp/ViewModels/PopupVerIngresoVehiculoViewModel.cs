using Acr.UserDialogs;
using AutoPrixApp.Models;
using AutoPrixWebApi.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AutoPrixApp.ViewModels
{
    public class PopupVerIngresoVehiculoViewModel: BaseViewModel
    {
        public string _modo;
        public ObservableCollection<Imagen> _imagen { get; set; }
        public PopupVerIngresoVehiculoViewModel(Int64 idOrden,string modo) {
            _modo = modo;
            CargarFotografias(idOrden);

        }
        
        public ObservableCollection<Imagen> Views
    {
            get => _imagen;
            set
            {
                if (value != _imagen)
                {
                    _imagen = value;
                    OnPropertyChanged(nameof(Views));
                }
            }
        }



        private string obtenerCadenaJson(string modo,Int64 idOrden)
        {
            string cadena = "";
            Int64 idusu = Int64.Parse(Preferences.Get("IdUsuario", 0l).ToString());
            try
            {
                var my_jsondata = new
                {
                    idOrden = idOrden,
                    idUsuario = idusu,
                    modo = modo
                };

                //Tranform it to Json object
                cadena = JsonConvert.SerializeObject(my_jsondata);

            }
            catch (Exception e)
            { return "Error: " + e.Message; }
            return cadena;
        }

        public async void CargarFotografias(Int64 idorden) {
            try
            {
                List<ImagenTrabajos> LsimagenTrabajos = new List<ImagenTrabajos>();
                Views = new ObservableCollection<Imagen>();
                string cadena = obtenerCadenaJson(_modo,idorden);
                var json = await Globales.PostApiApp("imagenes/PostObtenerImagenes", cadena);
                jsonResult jsonres = JsonConvert.DeserializeObject<jsonResult>(json.ToString());
                LsimagenTrabajos.Clear(); Views.Clear();

                if (jsonres.MENSAJE == "Ok")
                {
                    ImagenTrabajos c = null;
                    foreach (var item in jsonres.RESULTADO)
                    {
                        c = JsonConvert.DeserializeObject<ImagenTrabajos>(item.ToString());
                        LsimagenTrabajos.Add(c);  
                    }

                    if (LsimagenTrabajos.Count <= 0) { await Application.Current.MainPage.DisplayAlert("Automotriz Hernan Error", $"" + "No existen Imagenes..." + "\n", "Aceptar"); await PopupNavigation.PopAsync(); return; }
                    Imagen x=null;
                    foreach (ImagenTrabajos imagen in LsimagenTrabajos)
                    {
                        try
                        {
                            x = new Imagen();
                            //x.UrlSource = ;
                            x.Id = imagen.IdImagen;
                            x.Name = imagen.Descripcion;
                            x.ImageData = imagen.SourceImage;
                            x.Base64 = imagen.Imagen;
                            x.UrlSource = obtenerImageSource(x.ImageData);
                            _imagen.Add(x);
                            x = null;
                        }
                        catch (Exception e){ Console.WriteLine(e.Message); }
                    }

                    //foreach (Imagen im in _imagen) {
                    //    im.UrlSource = obtenerImageSource(im.ImageData);
                    //}
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Automotriz Hernan Error", $"" + jsonres.STACK.ToString() + "\n", "Aceptar");
                }

            }
            catch (Exception e) {
                await Application.Current.MainPage.DisplayAlert("Automotriz Hernan Error", $"" + e.Message + "\n", "Aceptar");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        public ImageSource obtenerImageSource(Byte[] image) {
            ImageSource retval = null;

            try
            {
                if (image != null)
                {
                    retval = ImageSource.FromStream(() => new MemoryStream(image));
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return retval;
        }
    }
}

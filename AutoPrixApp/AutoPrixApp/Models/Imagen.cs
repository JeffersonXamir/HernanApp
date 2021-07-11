using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AutoPrixApp.Models
{
    public class Imagen
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public ImageSource UrlSource { get; set; }
        public string Base64 { get; set; }
        public string Tipo { get; set; }
        public string SrcUrl { get; set; }
        public byte[] ImageData { get; set; }

    }
}

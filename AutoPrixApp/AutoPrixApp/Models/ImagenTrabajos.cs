using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPrixApp.Models
{
    public class ImagenTrabajos
    {
        public Int64 IdImagen { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
        public string Imagen { get; set; }
        public Int64 IdOrdenTrabajoCab { get; set; }
        public Int64 UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Int64 UsuarioModifica { get; set; }
        public DateTime FechaModifica { get; set; }
        
        public byte[] SourceImage { get; set; }

    }
}

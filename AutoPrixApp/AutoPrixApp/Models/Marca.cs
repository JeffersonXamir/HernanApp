using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoPrixApp.Entidades
{
    public class Marca
    {
        public Int64 IdMarca { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public Int64 UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Int64 UsuarioModifica { get; set; }
        public DateTime FechaModifica { get; set; }
    }
}
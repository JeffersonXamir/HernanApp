using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoPrixWebApi.Entidades
{
    public class Usuario
    {
        public Int64 IdUsuario { get; set; }
        public String Login { get; set; }
        public String Pass { get; set; }
        public Int64 IdTipo { get; set; }
        public String Estado { get; set; }
        public Int64 UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Int64 UsuarioModifica { get; set; }
        public DateTime FechaModifica { get; set; }
        public Int64 IdRol { get; set; }
        public string Rol { get; set; }
        public string Nombre { get; set; }


    }
}
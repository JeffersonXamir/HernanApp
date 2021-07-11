using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPrixWebApi.Entidades
{
    public class UsuarioEnte
    {
        public string nombre1 { get; set; }
        public string nombre2 { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public string cedula { get; set; }
        public string telefono { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public string email { get; set; }
        public Int64 UsuarioCreacion { get; set; }
        public Int64 IdRol { get; set; }
    }
}

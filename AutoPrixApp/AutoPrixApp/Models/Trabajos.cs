using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoPrixWebApi.Models
{
    public class Trabajos
    {
        public Int64 IdTrabajoAutorizado { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public Decimal PrecioUnitario { get; set; }
        public Int64 UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Int64 UsuarioModifica { get; set; }
        public DateTime FechaModifica { get; set; }
        public Int64 IdTipoTrabajo { get; set; }
    }
}
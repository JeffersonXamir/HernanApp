using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPrixApp.Models
{
    public class OrdenTrabajoDet
    {
        public Int64 IdOrdenTrabajoDet { get; set; }
        public Int64 IdOrdenTrabajoCab { get; set; }
        public Int64 IdTrabajoAutorizado { get; set; }
        public string Estado { get; set; }
        public string Detalle { get; set; }
        public Int64 UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Int64 UsuarioModifica { get; set; }
        public Int64 FechaModifica { get; set; }

    }
}

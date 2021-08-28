using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPrixApp.Models
{
    public class OrdenTrabajoLlantas
    {
        public Int64 IdOrdenTrabajoLlantas { get; set; }
        public Int64 IdOrdenTrabajoCab { get; set; }
        public string CodPosicion { get; set; }
        public string Estado { get; set; }
        public Decimal Presion { get; set; }
        public string Observacion { get; set; }
    }
}

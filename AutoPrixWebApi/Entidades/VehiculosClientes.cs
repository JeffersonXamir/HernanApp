using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPrixWebApi.Entidades
{
    public class VehiculosClientes
    {
        public Int64 id { get; set; }
        public String placa { get; set; }
        public String cliente { get; set; }
        public String vehiculo { get; set; }
        public String cedula { get; set; }
    }
}

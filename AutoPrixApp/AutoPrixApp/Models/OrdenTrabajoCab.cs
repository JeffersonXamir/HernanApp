using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPrixApp.Models
{
    public class OrdenTrabajoCab
    {
        public Int64 IdOrdenTrabajoCab { get; set; }
        public Int64 IdClienteVehiculo { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime HoraIngreso { get; set; }
        public Int64 UsuarioRecepcion { get; set; }
        public double Kilometros { get; set; }
        public Int64 idFormaPago { get; set; }
        public string Observacion { get; set; }
        public Int64 UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Int64 UsuarioModifica { get; set; }
        public DateTime FechaModifica { get; set; }

        /*se agregan nuevos campos*/
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }

        /*para presentacion*/
        public string OrdenNum { get; set; }
        public string FechaOrden { get; set; }
        public string Estado { get; set; }
    }
}

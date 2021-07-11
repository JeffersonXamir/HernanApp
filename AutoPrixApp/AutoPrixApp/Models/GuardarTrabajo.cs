using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPrixApp.Models
{
    public class GuardarTrabajo
    {
        public OrdenTrabajoCab CabeceraTrab { get; set; }
        public List<OrdenTrabajoDet> DetallesTrab { get; set; }
        public List<OrdenTrabajoLlantas> LlantasTrab { get; set; }
        public List<ImagenTrabajos> ImagenTrab { get; set; }
        public byte[] Imagenes { get; set; }

    }
}

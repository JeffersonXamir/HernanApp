﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPrixWebApi.Entidades
{
    public class OrdenTrabajoLlantas
    {
        public Int64 IdOrdenTrabajoLlantas { get; set; }
        public Int64 IdOrdenTrabajoCab { get; set; }
        public Int64 CodPosicion { get; set; }
        public string Estado { get; set; }
        public Decimal Presion { get; set; }
        public string Observacion { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoPrixWebApi.Models
{
    public class jsonResult
    {
        public string MENSAJE { get; set; }
        public string STACK { get; set; }
        public List<object> RESULTADO { get; set; }
    }
}
using AutoPrixWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AutoPrixWebApi.Controllers
{
    public class VehiculosClientesController : ApiController
    {
        // GET: api/VehiculosClientes
        [HttpGet]
        [Route("api/VehiculosClientes/GetVehiculosClientes/{cedula}")]
        public HttpResponseMessage GetVehiculosClientes(string cedula)
        {
            jsonResult json = new jsonResult();
            try
            {
                DataAccess.BPVehiculos bp = new DataAccess.BPVehiculos();
                List<object> ens = bp.ObtenerVehiculosClientesApi(cedula);

                json.MENSAJE = "Ok";
                json.STACK = "";
                json.RESULTADO = ens;
            }
            catch (Exception ex)
            {
                json.MENSAJE = "Error";
                json.STACK = ex.Message;
                json.RESULTADO = null;
                return Request.CreateResponse(HttpStatusCode.NotFound, json);
            }

            return Request.CreateResponse(HttpStatusCode.OK, json);
        }

        // GET: api/VehiculosClientes/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/VehiculosClientes
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/VehiculosClientes/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/VehiculosClientes/5
        public void Delete(int id)
        {
        }
    }
}

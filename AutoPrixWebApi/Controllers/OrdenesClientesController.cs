using AutoPrixWebApi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AutoPrixWebApi.Controllers
{
    public class OrdenesClientesController : ApiController
    {
        // GET: api/VehiculosClientes
        [HttpGet]
        [Route("api/OrdenesClientes/GetOrdenesClientes/{cadena}")]
        public HttpResponseMessage GetOrdenesClientes(JObject cadena)
        {
            jsonResult json = new jsonResult();
            try
            {
                DataAccess.BPOrdenes bp = new DataAccess.BPOrdenes();
                List<object> ens = bp.ObtenerOrdenesCabApi(cadena);

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
        [HttpPost]
        [Route("api/OrdenesClientes/PostOrdenesClientes")]
        public HttpResponseMessage PostOrdenesClientes(JObject cadena)
        {
            jsonResult json = new jsonResult();
            try
            {
                DataAccess.BPOrdenes bp = new DataAccess.BPOrdenes();
                List<object> ens = bp.ObtenerOrdenesCabApi(cadena);

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

        [HttpPost]
        [Route("api/OrdenesClientes/PostAutorizarOrden")]
        public HttpResponseMessage PostAutorizarOrden(JObject cadena)
        {
            jsonResult json = new jsonResult();
            try
            {
                DataAccess.BPOrdenes bp = new DataAccess.BPOrdenes();
                string ens = bp.AutorizarOrden(cadena);

                json.MENSAJE = "Ok";
                json.STACK = ens;
                json.RESULTADO = null;
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

using AutoPrixWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AutoPrixWebApi.Controllers
{
    public class OrdenesDetallesClientesController : ApiController
    {
        // GET: api/OrdenesDetallesClientes
        [HttpGet]
        [Route("api/OrdenesDetallesClientes/GetOrdenesDetallesClientes/{idCab}")]
        public HttpResponseMessage GetOrdenesDetallesClientes(int idCab)
        {
            jsonResult json = new jsonResult();
            try
            {
                DataAccess.BPOrdenesDetalles bp = new DataAccess.BPOrdenesDetalles();
                List<object> ens = bp.ObtenerOrdenesDetApi(idCab,"DET");

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

        // GET: api/OrdenesDetallesClientes
        [HttpGet]
        [Route("api/OrdenesDetallesClientes/GetOrdenesDetalleImagenes/{idCab}")]
        public HttpResponseMessage GetOrdenesDetalleImagenes(int idCab)
        {
            jsonResult json = new jsonResult();
            try
            {
                DataAccess.BPOrdenesDetalles bp = new DataAccess.BPOrdenesDetalles();
                List<object> ens = bp.ObtenerOrdenesDetApi(idCab, "IMA");

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

        // GET: api/OrdenesDetallesClientes
        [HttpGet]
        [Route("api/OrdenesDetallesClientes/GetOrdenesDetalleLlantas/{idCab}")]
        public HttpResponseMessage GetOrdenesDetalleLlantas(int idCab)
        {
            jsonResult json = new jsonResult();
            try
            {
                DataAccess.BPOrdenesDetalles bp = new DataAccess.BPOrdenesDetalles();
                List<object> ens = bp.ObtenerOrdenesDetApi(idCab, "LLA");

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

using AutoPrixWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AutoPrixWebApi.Controllers
{
    public class FormaPagoController : ApiController
    {
        // GET: api/FormaPago
        [HttpGet]
        [Route("api/formapago/GetFormaPagos")]
        public HttpResponseMessage GetFormaPagos()
        {
            jsonResult json = new jsonResult();
            try
            {
                DataAccess.BPFormaPagos bp = new DataAccess.BPFormaPagos();
                List<object> ens = bp.ObtenerFormaPagosApi();

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

        // GET: api/FormaPago/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/FormaPago
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/FormaPago/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/FormaPago/5
        public void Delete(int id)
        {
        }
    }
}

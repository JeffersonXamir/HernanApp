using AutoPrixWebApi.Entidades;
using AutoPrixWebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace AutoPrixWebApi.Controllers
{
    public class TrabajosController : ApiController
    {
        // GET: api/Trabajos
        [HttpGet]
        [Route("api/trabajos/GetTrabajos/{id}")]
        public HttpResponseMessage GetTrabajos(int id)
        {
            jsonResult json = new jsonResult();
            try
            {
                DataAccess.BPTrabajos bp = new DataAccess.BPTrabajos();
                List<object> ens = bp.ObtenerTrabajosApi(id);

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
        // GET: api/Trabajos/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Trabajos
        [HttpPost]
        [Route("api/trabajos/PostGuardarTrabajo")]
        public HttpResponseMessage PostGuardarTrabajo(GuardarTrabajo obj)
        {

            jsonResult json = new jsonResult();
            try
            {
                //validar parametros 

                if (obj.Equals(null))
                {
                    throw new Exception("Campos necesarios");
                }

                DataAccess.BPTrabajos bp = new DataAccess.BPTrabajos();
                string ens = bp.GuardarTrabajoApi(obj);

                json.MENSAJE = "Ok";
                json.STACK = ens;
                json.RESULTADO = null;
            }
            catch (Exception ex)
            {
                json.MENSAJE = "Error";
                json.STACK = ex.Message;
                json.RESULTADO = null;
                return Request.CreateResponse(HttpStatusCode.BadRequest, json);
            }

            return Request.CreateResponse(HttpStatusCode.OK, json);
        }

        
        // PUT: api/Trabajos/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Trabajos/5
        public void Delete(int id)
        {
        }
    }
}

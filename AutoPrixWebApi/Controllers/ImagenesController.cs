using AutoPrixWebApi.Entidades;
using AutoPrixWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AutoPrixWebApi.Controllers
{
    public class ImagenesController : ApiController
    {
        // GET: api/Imagenes
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Imagenes/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Imagenes
        [HttpPost]
        [Route("api/imagenes/PostGuardarImagen")]
        public HttpResponseMessage PostGuardarImagen(ImagenTrabajos obj)
        {

            jsonResult json = new jsonResult();
            try
            {
                //validar parametros 

                if (obj.Equals(null))
                {
                    throw new Exception("Campos necesarios");
                }

                DataAccess.BPImagenes bp = new DataAccess.BPImagenes();
                string ens = bp.GuardarImagenApi(obj);

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

        // PUT: api/Imagenes/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Imagenes/5
        public void Delete(int id)
        {
        }
    }
}

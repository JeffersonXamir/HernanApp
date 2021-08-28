using AutoPrixWebApi.Entidades;
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
    public class UsuarioController : ApiController
    {
        

        // GET: api/Usuario/5
        [HttpGet]
        [Route("api/usuario/GetUserById/{id}")]
        public string GetUserById(int id)
        {
            return "chino";
        }

        [HttpGet]
        [Route("api/usuario/GetUserByName/{strname}")]
        public string GetUserByName(string strname)
        {
            return "nacho";
        }

        // POST: api/Usuario
        [HttpPost]
        [Route("api/usuario/PostVerificaUsuario")]
        public HttpResponseMessage PostVerificaUsuario(Usuario obj)
        {

            jsonResult json = new jsonResult();
            try
            {
                //validar parametros 

                if (obj.Equals(null))
                {
                    throw new Exception("Campos necesarios");
                }

                DataAccess.BPUsuario bp = new DataAccess.BPUsuario();
                List<object> ens = bp.VerificarUsuariosApi(obj);

                json.MENSAJE = "Ok";
                json.STACK = "";
                json.RESULTADO = ens;
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

        [HttpPost]
        [Route("api/usuario/PostCrearUsuario")]
        public HttpResponseMessage PostCrearUsuario(UsuarioEnte obj)
        {

            jsonResult json = new jsonResult();
            try
            {
                //validar parametros 

                if (obj.Equals(null))
                {
                    throw new Exception("Campos necesarios");
                }

                DataAccess.BPUsuario bp = new DataAccess.BPUsuario();
                string ens = bp.RegistarUsuariosApi(obj);

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

        // POST: api/Usuario
        [HttpPost]
        [Route("api/usuario/PostUsuariosRoles")]
        public HttpResponseMessage PostUsuariosRoles(JObject cadena)
        {
            jsonResult json = new jsonResult();
            try
            {
                DataAccess.BPUsuario bp = new DataAccess.BPUsuario();
                List<object> ens = bp.ObtenerUsuariosApi(cadena);

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

        // PUT: api/Usuario/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Usuario/5
        public void Delete(int id)
        {
        }
    }
}

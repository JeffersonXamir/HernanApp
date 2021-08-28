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
                List<object> ens = bp.ObtenerVehiculosClientesApi(cedula,"CED");

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

        [HttpGet]
        [Route("api/VehiculosClientes/GetVehiculosClientesId/{id}")]
        // GET: api/VehiculosClientes/5
        public HttpResponseMessage GetVehiculosClientesId(string id)
        {
            jsonResult json = new jsonResult();
            try
            {
                DataAccess.BPVehiculos bp = new DataAccess.BPVehiculos();
                List<object> ens = bp.ObtenerVehiculosClientesApi(id,"ID");

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

        [HttpGet]
        [Route("api/VehiculosClientes/GetVehiculosMarcas/{id}")]
        // GET: api/VehiculosClientes/5
        public HttpResponseMessage GetVehiculosMarcas(string id)
        {
            jsonResult json = new jsonResult();
            try
            {
                DataAccess.BPVehiculos bp = new DataAccess.BPVehiculos();
                List<object> ens = bp.ObtenerVehiculosClientesApi(id, "MAR");

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

        [HttpGet]
        [Route("api/VehiculosClientes/GetVehiculosModelos/{id}")]
        // GET: api/VehiculosClientes/5
        public HttpResponseMessage GetVehiculosModelos(string id)
        {
            jsonResult json = new jsonResult();
            try
            {
                DataAccess.BPVehiculos bp = new DataAccess.BPVehiculos();
                List<object> ens = bp.ObtenerVehiculosClientesApi(id, "MOD");

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
        [HttpGet]
        [Route("api/VehiculosClientes/GetVehiculoOrden/{orden}")]
        public HttpResponseMessage GetVehiculoOrden(string orden)
        {
            jsonResult json = new jsonResult();
            try
            {
                DataAccess.BPVehiculos bp = new DataAccess.BPVehiculos();
                List<object> ens = bp.ObtenerVehiculosClientesApi(orden, "ORD");

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

        // POST: api/VehiculosClientes
        [HttpPost]
        [Route("api/VehiculosClientes/PostGuardarVehiculo")]
        public HttpResponseMessage PostGuardarVehiculo(VehiculosClientes obj)
        {

            jsonResult json = new jsonResult();
            try
            {
                //validar parametros 

                if (obj.Equals(null))
                {
                    throw new Exception("Campos necesarios");
                }

                DataAccess.BPVehiculos bp = new DataAccess.BPVehiculos();
                string ens = bp.GuardarVehiculoApi(obj);

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

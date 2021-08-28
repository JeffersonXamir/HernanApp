using AutoPrixWebApi.Entidades;
using AutoPrixWebApi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AutoPrixWebApi.DataAccess
{
    public class BPOrdenes
    {
        public List<object> ObtenerOrdenesCabApi(JObject cadena)
        {
            DataSet ds = new DataSet();
            List<object> ens = new List<object>();
            var conection = System.Configuration.ConfigurationManager.ConnectionStrings["ApiConnectionString"].ConnectionString;
            DataTable dt = new DataTable();
            Int64 idOrden = 0;
            string busqueda = "";
            DateTime fechaInicio;
            DateTime fechaFin;
            Int64 idUsuario = 0;
            string modo = "";
            try
            {
                JObject json_object = cadena;//JObject.Parse(cadena);

                //set the parsed Json object
                idOrden = json_object["idOrden"]==null?0:Int64.Parse(json_object["idOrden"].ToString());
                busqueda = json_object["busqueda"]==null?"": (string)json_object["busqueda"];
                fechaInicio = DateTime.Parse(json_object["fechaInicio"].ToString());
                fechaFin = DateTime.Parse(json_object["fechaFin"].ToString());
                idUsuario = json_object["idUsuario"]==null?0:Int64.Parse(json_object["idUsuario"].ToString());
                modo = json_object["modo"]==null?"CLI": (string)json_object["modo"];
                using (SqlConnection sql = new SqlConnection(conection))
                {

                    using (SqlCommand cmd = new SqlCommand("GetOrdenesCabApi", sql))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@idUsuario", idUsuario));
                        cmd.Parameters.Add(new SqlParameter("@busqueda", busqueda));
                        cmd.Parameters.Add(new SqlParameter("@fechaInicio", fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@fechaFin", fechaFin));
                        cmd.Parameters.Add(new SqlParameter("@modo", modo));

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);

                        ens = ServicioTransporte.ConvertDataTableObject<OrdenTrabajoCab>(dt);
                        sql.Close();
                    }
                }

            }
            catch (Exception myex)
            {
                throw new Exception("Error--> " + myex.Message);
            }
            return ens;
        }

        public string AutorizarOrden(JObject cadena)
        {
            DataSet ds = new DataSet();
            string ens ="";
            var conection = System.Configuration.ConfigurationManager.ConnectionStrings["ApiConnectionString"].ConnectionString;
            DataTable dt = new DataTable();
            Int64 idOrden = 0;
            string busqueda = "";
            DateTime fechaInicio;
            DateTime fechaFin;
            Int64 idUsuario = 0, idEmpleado=0;
            string modo = "",observacion ="";
            try
            {
                JObject json_object = cadena;//JObject.Parse(cadena);

                //set the parsed Json object
                idOrden = json_object["idOrden"] == null ? 0 : Int64.Parse(json_object["idOrden"].ToString());
                idUsuario = json_object["idUsuario"] == null ? 0 : Int64.Parse(json_object["idUsuario"].ToString());
                idEmpleado = json_object["idEmpleado"] == null ? 0 : Int64.Parse(json_object["idEmpleado"].ToString());
                modo = json_object["modo"] == null ? "G" : (string)json_object["modo"];
                observacion = json_object["observacion"] == null ? "" : (string)json_object["observacion"];
                using (SqlConnection sql = new SqlConnection(conection))
                {

                    using (SqlCommand cmd = new SqlCommand("PostAutorizaOrdenApi", sql))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdOrden", idOrden));
                        cmd.Parameters.Add(new SqlParameter("@idUsuario", idUsuario));
                        cmd.Parameters.Add(new SqlParameter("@idEmpleado", idEmpleado));
                        cmd.Parameters.Add(new SqlParameter("@Descripcion1", observacion)); 
                        cmd.Parameters.Add(new SqlParameter("@modo", modo));

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        if (modo.Equals("AUT"))
                        {
                            ens = "Orden Autorizado Exitosamente";
                        } else if (modo.Equals("SAL")) {
                            ens = "Orden Finalizada Exitosamente";
                        }
                        sql.Close();
                    }
                }

            }
            catch (Exception myex)
            {
                throw new Exception("Error--> " + myex.Message);
            }
            return ens;
        }
    }
}
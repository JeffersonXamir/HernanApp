using AutoPrixWebApi.Entidades;
using AutoPrixWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AutoPrixWebApi.DataAccess
{
    public class BPTrabajos
    {
        public List<object> ObtenerTrabajosApi(int id)
        {
            DataSet ds = new DataSet();
            List<object> ens = new List<object>();
            var conection = System.Configuration.ConfigurationManager.ConnectionStrings["ApiConnectionString"].ConnectionString;
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sql = new SqlConnection(conection))
                {

                    using (SqlCommand cmd = new SqlCommand("GetTrabajosApi", sql))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@idtrabajo1", id));

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);

                        ens = ServicioTransporte.ConvertDataTableObject<Trabajos>(dt);
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

        public String GuardarTrabajoApi(GuardarTrabajo obj)
        {
            DataSet ds = new DataSet();
            List<object> ens = new List<object>();
            var conection = System.Configuration.ConfigurationManager.ConnectionStrings["ApiConnectionString"].ConnectionString;
            DataTable dt = new DataTable();
            string result = "";
            string retorno = "";
            int RETURN_VALUE_BUFFER_SIZE = 32767;
            /*Seteo valores Extraidos*/
            //GuardarTrabajo obj = JsonConvert.DeserializeObject<GuardarTrabajo>(obj1);
            OrdenTrabajoCab Cab = obj.CabeceraTrab;
            List<OrdenTrabajoDet> ListDetalles = obj.DetallesTrab;
            List<OrdenTrabajoLlantas> ListTrabajLlanta = obj.LlantasTrab;
            List<ImagenTrabajos> ListImagenes = obj.ImagenTrab;//JsonConvert.DeserializeObject<GuardarTrabajo>(obj.Imagenes) ;
            try
            {
                SqlConnection cnn = new SqlConnection(conection);
                SqlTransaction transaction;

                cnn.Open();
                transaction = cnn.BeginTransaction();

                try
                {

                    // Command Objects for the transaction
                    SqlCommand cmd1 = new SqlCommand("PostCrearOrdenTrabajoCabApi", cnn);          
                    cmd1.CommandType = CommandType.StoredProcedure;
                    /*
                    cmd1.Parameters.Add(new SqlParameter("@Param1", SqlDbType.NVarChar, 50));
                    cmd1.Parameters["@Param1"].Value = paramValue1;
                    */
                    /*GUARDO CABECERA*/
                    cmd1.Parameters.Add("@IdOrdenTrabajoCab1", SqlDbType.NVarChar, RETURN_VALUE_BUFFER_SIZE);
                    cmd1.Parameters["@IdOrdenTrabajoCab1"].Direction = ParameterDirection.Output;
                    cmd1.Parameters.Add(new SqlParameter("@IdClienteVehiculo1", Cab.IdClienteVehiculo));
                    cmd1.Parameters.Add(new SqlParameter("@FechaIngreso1", Cab.FechaIngreso));
                    cmd1.Parameters.Add(new SqlParameter("@HoraIngreso1", Cab.HoraIngreso));
                    cmd1.Parameters.Add(new SqlParameter("@UsuarioRecepcion1", Cab.UsuarioRecepcion));
                    cmd1.Parameters.Add(new SqlParameter("@Kilometros1", Cab.Kilometros));
                    cmd1.Parameters.Add(new SqlParameter("@idFormaPago1", Cab.idFormaPago));
                    cmd1.Parameters.Add(new SqlParameter("@Observacion1", Cab.Observacion));
                    cmd1.Parameters.Add(new SqlParameter("@UsuarioCreacion1", Cab.UsuarioCreacion));
                    cmd1.Parameters.Add(new SqlParameter("@FechaCreacion1", Cab.FechaCreacion));

                    try
                    {
                        cmd1.Transaction = transaction;
                        cmd1.ExecuteNonQuery();
                        result = cmd1.Parameters["@IdOrdenTrabajoCab1"].Value.ToString();
                    }
                    catch (SqlException sql) {
                        throw new Exception("Error Orden Cab=> "+sql.Message);
                    }

                    /*GUARDO DETALLE*/
                    SqlCommand cmd2;
                    try
                    {
                        foreach (OrdenTrabajoDet item in ListDetalles)
                        {
                            cmd2 = new SqlCommand("PostCrearOrdenTrabajoDetApi", cnn);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.Add(new SqlParameter("@IdOrdenTrabajoCab1", result));
                            cmd2.Parameters.Add(new SqlParameter("@IdTrabajoAutorizado1", item.IdTrabajoAutorizado));
                            cmd2.Parameters.Add(new SqlParameter("@Estado1", "A"));
                            cmd2.Parameters.Add(new SqlParameter("@Detalle1", item.Detalle));
                            cmd2.Parameters.Add(new SqlParameter("@UsuarioCreacion1", item.UsuarioCreacion));
                            cmd2.Parameters.Add(new SqlParameter("@FechaCreacion1", item.FechaCreacion));
                            cmd2.Transaction = transaction;
                            cmd2.ExecuteNonQuery();
                        }
                    }
                    catch (SqlException sql) {
                        
                        throw new Exception("Error Orden Det=> " + sql.Message);
                    }

                    /*GUARDO ORDEN TRABAJO LLANTAS*/
                    SqlCommand cmd3;
                    try
                    {
                        foreach (OrdenTrabajoLlantas item in ListTrabajLlanta)
                        {
                            cmd3 = new SqlCommand("PostCrearOrdenTrabLlantasApi", cnn);
                            cmd3.CommandType = CommandType.StoredProcedure;
                            cmd3.Parameters.Add(new SqlParameter("@IdOrdenTrabajoCab1", result));
                            cmd3.Parameters.Add(new SqlParameter("@CodPosicion1", item.CodPosicion));
                            cmd3.Parameters.Add(new SqlParameter("@Estado1", "A"));
                            cmd3.Parameters.Add(new SqlParameter("@Presion1", item.Presion));
                            cmd3.Parameters.Add(new SqlParameter("@Observacion1", item.Observacion == null ? " " : item.Observacion)); //aqui cayo xD
                            cmd3.Transaction = transaction;
                            cmd3.ExecuteNonQuery();
                        }
                    }
                    catch (SqlException sql) {
                        
                        throw new Exception("Error Orden Llantas=> " + sql.Message);
                    }

                    /*GUARDO IMAGENES
                    SqlCommand cmd4;
                    try
                    {
                        foreach (ImagenTrabajos item in ListImagenes)
                        {
                            cmd4 = new SqlCommand("PostGuardarImagenesApi", cnn);
                            cmd4.CommandType = CommandType.StoredProcedure;
                            cmd4.Parameters.Add(new SqlParameter("@Descripcion1", item.Descripcion));
                            cmd4.Parameters.Add(new SqlParameter("@Tipo1", item.Tipo));
                            cmd4.Parameters.Add(new SqlParameter("@Estado", "A"));
                            cmd4.Parameters.Add(new SqlParameter("@IdOrdenTrabajoCab1", result));
                            cmd4.Parameters.Add(new SqlParameter("@Imagen1", item.Imagen));
                            cmd4.Parameters.Add(new SqlParameter("@UsuarioCreacion1", item.UsuarioCreacion)); //revisar
                            cmd4.Parameters.Add(new SqlParameter("@FechaCreacion1", item.FechaCreacion));
                            cmd4.Transaction = transaction;
                            cmd4.ExecuteNonQuery();

                        }
                    }
                    catch (SqlException sql) {
                        
                        throw new Exception("Error Imagenes=> " + sql.Message);
                    }
                    */
                    retorno = result;//"Trabajo se Guardo Exitosamente!.";
                    transaction.Commit();
                }

                catch (SqlException sqlEx)
                {
                    transaction.Rollback();
                    throw new Exception(sqlEx.Message);
                }

                finally
                {
                    cnn.Close();
                    cnn.Dispose();
                }

            }
            catch (Exception myex)
            {
                throw new Exception("Error--> " + myex.Message);
            }
            return retorno;
        }
    }
}
using AutoPrixWebApi.Entidades;
using AutoPrixWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AutoPrixWebApi.DataAccess
{
    public class BPVehiculos
    {
        public List<object> ObtenerVehiculosClientesApi(string cedula,string modo)
        {
            DataSet ds = new DataSet();
            List<object> ens = new List<object>();
            var conection = System.Configuration.ConfigurationManager.ConnectionStrings["ApiConnectionString"].ConnectionString;
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sql = new SqlConnection(conection))
                {

                    using (SqlCommand cmd = new SqlCommand("GetVehiculosClientesApi", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Cedula1", cedula));
                        cmd.Parameters.Add(new SqlParameter("@modo", modo));
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);

                        switch (modo) {
                            case "CED":
                                ens = ServicioTransporte.ConvertDataTableObject<VehiculosClientes>(dt);
                                sql.Close();
                                break;
                            case "ID":
                                ens = ServicioTransporte.ConvertDataTableObject<VehiculosClientes>(dt);
                                sql.Close();
                                break;
                            case "MAR":
                                ens = ServicioTransporte.ConvertDataTableObject<Marca>(dt);
                                sql.Close();
                                break;
                            case "MOD":
                                ens = ServicioTransporte.ConvertDataTableObject<Modelo>(dt);
                                sql.Close();
                                break;
                            case "ORD":
                                ens = ServicioTransporte.ConvertDataTableObject<VehiculosClientes>(dt);
                                sql.Close();
                                break;

                        }
                        
                    }
                }

            }
            catch (Exception myex)
            {
                throw new Exception("Error--> " + myex.Message);
            }
            return ens;
        }

        public String GuardarVehiculoApi(VehiculosClientes obj)
        {
            DataSet ds = new DataSet();
            List<object> ens = new List<object>();
            var conection = System.Configuration.ConfigurationManager.ConnectionStrings["ApiConnectionString"].ConnectionString;
            DataTable dt = new DataTable();
            string result = "";
            string retorno = "";
            int RETURN_VALUE_BUFFER_SIZE = 32767;
            
            try
            {
                SqlConnection cnn = new SqlConnection(conection);
                SqlTransaction transaction;

                cnn.Open();
                transaction = cnn.BeginTransaction();

                try
                {

                    // Command Objects for the transaction
                    SqlCommand cmd1 = new SqlCommand("PostVehiculosClientesApi", cnn);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    /*
                    cmd1.Parameters.Add(new SqlParameter("@Param1", SqlDbType.NVarChar, 50));
                    cmd1.Parameters["@Param1"].Value = paramValue1;
                    */
                    /*GUARDO CABECERA*/
                    cmd1.Parameters.Add(new SqlParameter("@IdVehiculo1", "0"));
                    cmd1.Parameters.Add(new SqlParameter("@IdMarca1", obj.Marca));
                    cmd1.Parameters.Add(new SqlParameter("@IdModelo1", obj.Modelo));   
                    cmd1.Parameters.Add(new SqlParameter("@CodigoColor1",1));
                    cmd1.Parameters.Add(new SqlParameter("@Anio1",obj.Anio));
                    cmd1.Parameters.Add(new SqlParameter("@Placa1",obj.placa));
                    cmd1.Parameters.Add(new SqlParameter("@Estado1","A"));
                    cmd1.Parameters.Add(new SqlParameter("@Usuario1", obj.cliente));
                    cmd1.Parameters.Add(new SqlParameter("@Modo","I"));
                    cmd1.Parameters.Add("@respuesta", SqlDbType.NVarChar, RETURN_VALUE_BUFFER_SIZE);
                    cmd1.Parameters["@respuesta"].Direction = ParameterDirection.Output;
                    try
                    {
                        cmd1.Transaction = transaction;
                        cmd1.ExecuteNonQuery();
                        result = cmd1.Parameters["@respuesta"].Value.ToString();

                        retorno = result;
                        transaction.Commit();
                    }
                    catch (SqlException sql)
                    {
                        throw new Exception("Error Crear Vehiculo=> " + sql.Message);
                    }

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
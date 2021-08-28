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
    public class BPUsuario
    {
        public List<object> ObtenerUsuariosApi(JObject cadena)
        {
            DataSet ds = new DataSet();
            List<object> ens = new List<object>();
            var conection = System.Configuration.ConfigurationManager.ConnectionStrings["ApiConnectionString"].ConnectionString;
            DataTable dt = new DataTable();

            try
            {
                /*SqlConnection sqlCon = new SqlConnection(conection);
                SqlCommand cmd = new SqlCommand("GetUsuariosApi",sqlCon);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdUsuario1",1));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                sqlCon.Open();
                cmd.ExecuteNonQuery();
                da.Fill(dt);
                sqlCon.Close();
               // List<Usuario> listUsuario = new List<Usuario>();
               // listUsuario = ConvertDataTable<Usuario>(dt);

                ens = ServicioTransporte.ConvertDataTable<object>(dt);
                */
                //DataTable table = new DataTable();
                JObject json_object = cadena;
                string modo; Int64 idusuario;
                idusuario = json_object["idUsuario"] == null ? 0 : Int64.Parse(json_object["idUsuario"].ToString());
                modo = json_object["modo"] == null ? "E" : (string)json_object["modo"];

                using (SqlConnection sql = new SqlConnection(conection))
                {

                    using (SqlCommand cmd = new SqlCommand("GetUsuariosApi", sql))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdUsuario1", idusuario));
                        cmd.Parameters.Add(new SqlParameter("@modo", modo));

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);

                        ens = ServicioTransporte.ConvertDataTableObject<Usuario>(dt);
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

        public List<object> VerificarUsuariosApi(Usuario obj)
        {
            DataSet ds = new DataSet();
            List<object> ens = new List<object>();
            var conection = System.Configuration.ConfigurationManager.ConnectionStrings["ApiConnectionString"].ConnectionString;
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sql = new SqlConnection(conection))
                {

                    using (SqlCommand cmd = new SqlCommand("PostVerificarUsuarioApi", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Login1", obj.Login));
                        cmd.Parameters.Add(new SqlParameter("@Pass1", obj.Pass));
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);

                        ens = ServicioTransporte.ConvertDataTableObject<Usuario>(dt);
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

        public String RegistarUsuariosApi(UsuarioEnte obj)
        {
            DataSet ds = new DataSet();
            List<object> ens = new List<object>();
            var conection = System.Configuration.ConfigurationManager.ConnectionStrings["ApiConnectionString"].ConnectionString;
            DataTable dt = new DataTable();
            string result = "";
            int RETURN_VALUE_BUFFER_SIZE = 32767;
            try
            {
                using (SqlConnection sql = new SqlConnection(conection))
                {
                    sql.Open();
                    SqlTransaction trans = sql.BeginTransaction();
                    using (SqlCommand cmd = new SqlCommand("PostCrearUsuarioApi", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        cmd.Parameters.Add(new SqlParameter("@Cedula1", obj.cedula));
                        cmd.Parameters.Add(new SqlParameter("@PrimerNombre1", obj.nombre1));
                        cmd.Parameters.Add(new SqlParameter("@SegundoNombre1", obj.nombre2));
                        cmd.Parameters.Add(new SqlParameter("@PrimerApellido1", obj.apellido1));
                        cmd.Parameters.Add(new SqlParameter("@SegundoApellido1", obj.apellido2));
                        cmd.Parameters.Add(new SqlParameter("@FechaNacimiento1", obj.fechaNacimiento));
                        cmd.Parameters.Add(new SqlParameter("@Direccion1", "")); 
                        cmd.Parameters.Add(new SqlParameter("@Telefono1", obj.telefono));
                        cmd.Parameters.Add(new SqlParameter("@Correo1", obj.email));
                        cmd.Parameters.Add(new SqlParameter("@Login1", obj.Login));
                        cmd.Parameters.Add(new SqlParameter("@Pass1", obj.Pass));
                        cmd.Parameters.Add(new SqlParameter("@UsuarioCreacion1", obj.UsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@IdRol1", obj.IdRol));
                        cmd.Parameters.Add("@Respuesta", SqlDbType.NVarChar, RETURN_VALUE_BUFFER_SIZE);
                        cmd.Parameters["@Respuesta"].Direction = ParameterDirection.Output;
                        try
                        {
                            //var returnParameter = cmd.Parameters.Add("@Respuesta", SqlDbType.NVarChar);
                            //returnParameter.Direction = ParameterDirection.Output;
                            cmd.Transaction = trans;
                            cmd.ExecuteNonQuery();
                            result = cmd.Parameters["@Respuesta"].Value.ToString();
                            trans.Commit();

                        }
                        catch (Exception tx)
                        {
                            trans.Rollback();
                            throw new Exception(tx.Message);
                        }
                        finally {
                            sql.Close();
                        }
                    }
                }

            }
            catch (Exception myex)
            {
                throw new Exception("Error--> " + myex.Message);
            }
            return result;
        }
    }
}
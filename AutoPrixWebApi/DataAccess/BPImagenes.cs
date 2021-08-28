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
    public class BPImagenes
    {
        public String GuardarImagenApi(ImagenTrabajos obj)
        {
            DataSet ds = new DataSet();
            List<object> ens = new List<object>();
            var conection = System.Configuration.ConfigurationManager.ConnectionStrings["ApiConnectionString"].ConnectionString;
            DataTable dt = new DataTable();
            string retorno = "";
           
            try
            {
                SqlConnection cnn = new SqlConnection(conection);
                SqlTransaction transaction;

                cnn.Open();
                transaction = cnn.BeginTransaction();

                /*GUARDO IMAGENES*/
                SqlCommand cmd4;
                try
                {

                    cmd4 = new SqlCommand("PostGuardarImagenesApi", cnn);
                    cmd4.CommandType = CommandType.StoredProcedure;
                    cmd4.Parameters.Add(new SqlParameter("@Descripcion1", obj.Descripcion));
                    cmd4.Parameters.Add(new SqlParameter("@Tipo1", obj.Tipo));
                    cmd4.Parameters.Add(new SqlParameter("@Estado1", obj.Estado));
                    cmd4.Parameters.Add(new SqlParameter("@IdOrdenTrabajoCab1", obj.IdOrdenTrabajoCab));
                    cmd4.Parameters.Add(new SqlParameter("@Imagen1", obj.Imagen));
                    cmd4.Parameters.Add(new SqlParameter("@UsuarioCreacion1", obj.UsuarioCreacion)); //revisar //solucionao
                    cmd4.Parameters.Add(new SqlParameter("@FechaCreacion1", obj.FechaCreacion));
                    //cmd4.Parameters.Add(new SqlParameter("@SourceImage1",SqlDbType.Image, obj.SourceImage));
                    cmd4.Parameters.Add("@SourceImage1",SqlDbType.Image).Value = obj.SourceImage;
                    cmd4.Transaction = transaction;
                    cmd4.ExecuteNonQuery();

                    retorno = "EXITO";
                    transaction.Commit();
                }
                catch (SqlException sql)
                {
                    transaction.Rollback();
                    throw new Exception("Error Imagenes=> " + sql.Message);
                }
                finally
                {
                    cnn.Close();
                    cnn.Dispose();
                }


            }

            catch (SqlException sqlEx)
            {

                throw new Exception(sqlEx.Message);
            }

            return retorno;

        }

        public List<object> ObtenerImagenApi(JObject json_object)
        {
            DataSet ds = new DataSet();
            List<object> ens = new List<object>();
            var conection = System.Configuration.ConfigurationManager.ConnectionStrings["ApiConnectionString"].ConnectionString;
            DataTable dt = new DataTable();
            string retorno = "";

            try
            {
                SqlConnection cnn = new SqlConnection(conection);
                SqlTransaction transaction;

                cnn.Open();
                transaction = cnn.BeginTransaction();

                /*GUARDO IMAGENES*/
                SqlCommand cmd4;
                try
                {

                    var idOrden = json_object["idOrden"] == null ? 0 : Int64.Parse(json_object["idOrden"].ToString());
                    var modo = json_object["modo"] == null ? "ING" : (string)json_object["modo"];

                    using (SqlConnection sql = new SqlConnection(conection))
                    {

                        using (SqlCommand cmd = new SqlCommand("PostObtenerImagenesOrdenesApi", sql))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@id_orden1", idOrden));
                            cmd.Parameters.Add(new SqlParameter("@modo", modo));

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            sql.Open();
                            da.Fill(dt);

                            ens = ServicioTransporte.ConvertDataTableObject<ImagenTrabajos>(dt);
                            sql.Close();
                        }
                    }
                }
                catch (SqlException sql)
                {
                    transaction.Rollback();
                    throw new Exception("Error Imagenes=> " + sql.Message);
                }
                finally
                {
                    cnn.Close();
                    cnn.Dispose();
                }


            }

            catch (SqlException sqlEx)
            {

                throw new Exception(sqlEx.Message);
            }

            return ens;

        }
    }
}
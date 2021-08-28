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
    public class BPOrdenesDetalles
    {
        public List<object> ObtenerOrdenesDetApi(int idCab,string modo)
        {
            DataSet ds = new DataSet();
            List<object> ens = new List<object>();
            var conection = System.Configuration.ConfigurationManager.ConnectionStrings["ApiConnectionString"].ConnectionString;
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sql = new SqlConnection(conection))
                {

                    using (SqlCommand cmd = new SqlCommand("GetOrdenesDetApi", sql))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdCabecera", idCab));
                        cmd.Parameters.Add(new SqlParameter("@modo", modo));

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);

                        switch (modo) {
                            case "DET":
                                ens = ServicioTransporte.ConvertDataTableObject<OrdenTrabajoDet>(dt);
                                sql.Close();
                                break;
                            case "IMA":
                                ens = ServicioTransporte.ConvertDataTableObject<ImagenTrabajos>(dt);
                                sql.Close();
                                break;
                            case "LLA":
                                ens = ServicioTransporte.ConvertDataTableObject<OrdenTrabajoLlantas>(dt);
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

        
    }
}
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
        public List<object> ObtenerVehiculosClientesApi(string cedula)
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
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);

                        ens = ServicioTransporte.ConvertDataTableObject<VehiculosClientes>(dt);
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
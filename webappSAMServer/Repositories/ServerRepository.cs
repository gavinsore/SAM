using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;   
using System.Configuration;   
using System.Data;
using webappSAMServer.Models;
using Core;


namespace webappSAMServer.Repositories
{
    public class ServerRepository
    {
        private SqlConnection connection;   
        private SqlCommand qry;   
         
        private void InitiateConnection()
        {   
           string constr = ConfigurationManager.ConnectionStrings["dbServer"].ToString();   
           connection = new SqlConnection(constr);     
        }

        public void PostStats(BaseServer server)
        {
            InitiateConnection();
            using (qry = new SqlCommand("InsertStats", connection))
            {
                qry.CommandType = CommandType.StoredProcedure;
                qry.Parameters.AddWithValue("@ServerGUID", server.ServerID);
                qry.Parameters.AddWithValue("@CPUUsage", server.ProcessorTotal);
                qry.Parameters.AddWithValue("@MemoryUsage", server.MemoryInUse);
                qry.Parameters.AddWithValue("@MemoryAvailable", server.MemoryAvailable);
                connection.Open();
                qry.ExecuteNonQuery();
                connection.Close();
            }
           
        }   
    }
}
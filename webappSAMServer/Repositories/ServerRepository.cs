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
           //connection.
        }

        public void PostDiskStats(List<BaseDisks> disks)
        {
            InitiateConnection();

            try
            {
                connection.Open();

                using (qry = new SqlCommand("InsertDiskStats", connection))
                {
                    qry.CommandType = CommandType.StoredProcedure;
                    qry.Parameters.AddWithValue("@ServerGUID", disks.First().ServerID);
                    qry.Parameters.Add("@DriveLetter", SqlDbType.VarChar);
                    qry.Parameters.Add("@Totalsize", SqlDbType.Float);
                    qry.Parameters.Add("@FreeSpace", SqlDbType.Float);

                    foreach (BaseDisks disk in disks)
                    {
                        qry.Parameters["@DriveLetter"].Value = disk.DiskLetter;
                        qry.Parameters["@Totalsize"].Value = disk.TotalDiskSize;
                        qry.Parameters["@FreeSpace"].Value = disk.FreeDiskSpace;

                        qry.ExecuteNonQuery();
                    }
                }
            }
            finally
            {
                connection.Close();
            }


        }

        public void PostStats(BaseServer server)
        {
            InitiateConnection();

            try
            {
                connection.Open();

                using (qry = new SqlCommand("InsertStats", connection))
                {
                    qry.CommandType = CommandType.StoredProcedure;
                    qry.Parameters.AddWithValue("@ServerGUID", server.ServerID);
                    qry.Parameters.AddWithValue("@CPUUsage", server.ProcessorTotal);
                    qry.Parameters.AddWithValue("@MemoryUsage", server.MemoryInUse);
                    qry.Parameters.AddWithValue("@MemoryAvailable", server.MemoryAvailable);
                
                    qry.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }

        }   

        public void PostServerHeader(BaseServer server)
        {
            InitiateConnection();

            try
            {
                connection.Open();

                using (qry = new SqlCommand("ServerHeaderUpdate", connection))
                {
                    qry.CommandType = CommandType.StoredProcedure;
                    qry.Parameters.AddWithValue("@ServerGUID", server.ServerID);
                    qry.Parameters.AddWithValue("@ServerName", server.ServerName);
                    qry.Parameters.AddWithValue("@OS", server.OS);
                    qry.Parameters.AddWithValue("@ServicePack", server.ServicePack);
                    qry.Parameters.AddWithValue("@LastBoot", server.BootTime);
                    qry.Parameters.AddWithValue("@TotalMemory", server.TotalMemory);

                    qry.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }


        }
    }
}
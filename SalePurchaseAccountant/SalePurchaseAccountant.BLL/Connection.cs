using SalePurchaseAccountant.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SalePurchaseAccountant.BLL
{
    public class Connection
    {
        public static void Initialize(string server, string database)
        {
            ConnectionGetway.Initialize(server, database);
        }
        public static void Initialize(string server, string database, string userId, string password)
        {
            ConnectionGetway.Initialize(server, database,userId,password);
        }
        public static SqlConnection GetConnection()
        {
            SqlConnection con = ConnectionGetway.GetConnection();
            if(con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            return con;
        }
        public static bool IsConnected()
        {
            return GetConnection().State == ConnectionState.Open;
        }
    }
}

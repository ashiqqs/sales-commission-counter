using Dapper;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SalePurchaseAccountant.DAL
{
    public class ConnectionGetway
    {
        private static string _connectionString;
        private static readonly SqlConnection _sqlConnection;
        public static void Initialize(string server, string database)
        {
            _connectionString = $"Server={server};Database={database}; Integrated Security=true";
        }
        public static void Initialize(string server, string database, string userId, string password)
        {
            _connectionString = $"Server={server};Database={database}; Integrated Security=false; UserId={userId}; Password={password}";
        }
        public static SqlConnection GetConnection()
        {
            //_sqlConnection = new SqlConnection(_connectionString);
            return _sqlConnection?? new SqlConnection(_connectionString);
        }
    }
}

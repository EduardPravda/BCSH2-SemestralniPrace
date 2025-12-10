using System;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace OrdinaceApp1.DataAccess
{
    public class Database
    {
        private string _connectionString;

        public Database()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
        }

        public OracleConnection GetConnection()
        {
            var conn = new OracleConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}
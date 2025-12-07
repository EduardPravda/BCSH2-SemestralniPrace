using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class SystemRepository
    {
        private readonly Database _database;

        public SystemRepository()
        {
            _database = new Database();
        }

        public List<DbObject> GetDatabaseObjects()
        {
            var list = new List<DbObject>();
            using (var conn = _database.GetConnection())
            {
                string sql = @"SELECT object_name, object_type, status 
                               FROM user_objects 
                               WHERE object_type IN ('TABLE', 'VIEW', 'PROCEDURE', 'FUNCTION', 'TRIGGER', 'SEQUENCE')
                               ORDER BY object_type, object_name";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new DbObject
                            {
                                Nazev = reader["object_name"].ToString(),
                                Typ = reader["object_type"].ToString(),
                                Status = reader["status"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}
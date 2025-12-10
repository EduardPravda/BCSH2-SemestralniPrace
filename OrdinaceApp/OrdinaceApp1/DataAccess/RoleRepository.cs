using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class RoleRepository
    {
        private readonly Database _database;

        public RoleRepository()
        {
            _database = new Database();
        }

        public List<Role> GetVsechnyRole()
        {
            var list = new List<Role>();
            using (var conn = _database.GetConnection())
            {
                string sql = "SELECT id_role, nazev_role FROM ROLE ORDER BY id_role";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Role
                            {
                                IdRole = Convert.ToInt32(reader["id_role"]),
                                Nazev = reader["nazev_role"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}
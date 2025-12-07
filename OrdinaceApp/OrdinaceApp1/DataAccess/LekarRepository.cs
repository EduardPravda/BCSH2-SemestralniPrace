using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class LekarRepository
    {
        private readonly Database _database;

        public LekarRepository()
        {
            _database = new Database();
        }

        public List<Lekar> GetHierarchielekaru()
        {
            var list = new List<Lekar>();
            using (var conn = _database.GetConnection())
            {
                string sql = @"
                    SELECT LEVEL as Uroven, jmeno, prijmeni, specializace 
                    FROM LEKAR
                    START WITH ID_Vedouci_Lekar IS NULL
                    CONNECT BY PRIOR ID_Lekar = ID_Vedouci_Lekar
                    ORDER SIBLINGS BY prijmeni";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Lekar
                            {
                                Uroven = Convert.ToInt32(reader["Uroven"]),
                                Jmeno = reader["jmeno"].ToString(),
                                Prijmeni = reader["prijmeni"].ToString(),
                                Specializace = reader["specializace"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}
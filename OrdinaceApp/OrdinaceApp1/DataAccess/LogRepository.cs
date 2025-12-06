using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class LogRepository
    {
        private readonly Database _database;

        public LogRepository()
        {
            _database = new Database();
        }

        public List<LogPolozka> GetLogs()
        {
            var logs = new List<LogPolozka>();

            using (var conn = _database.GetConnection())
            {
                // Seřadíme od nejnovějšího
                string sql = "SELECT * FROM HISTORY_LOG ORDER BY cas_zmeny DESC";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var log = new LogPolozka
                            {
                                IdLog = Convert.ToInt32(reader["id_log"]),
                                Tabulka = reader["tabulka"].ToString(),
                                TypOperace = reader["typ_operace"].ToString(),
                                Uzivatel = reader["uzivatel"].ToString(),
                                CasZmeny = Convert.ToDateTime(reader["cas_zmeny"]),
                                // Ošetření null hodnot pro Stará/Nová hodnota
                                StaraHodnota = reader["stara_hodnota"] == DBNull.Value ? "" : reader["stara_hodnota"].ToString(),
                                NovaHodnota = reader["nova_hodnota"] == DBNull.Value ? "" : reader["nova_hodnota"].ToString()
                            };
                            logs.Add(log);
                        }
                    }
                }
            }
            return logs;
        }
    }
}
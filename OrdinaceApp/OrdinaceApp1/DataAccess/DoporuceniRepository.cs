using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class DoporuceniRepository
    {
        private readonly Database _database;

        public DoporuceniRepository()
        {
            _database = new Database();
        }

        // 1. SEZNAM DOPORUČENÍ
        public List<Doporuceni> GetVsechnaDoporuceni()
        {
            var list = new List<Doporuceni>();
            using (var conn = _database.GetConnection())
            {
                string sql = @"
                    SELECT d.ID_doporuceni, d.duvodDoporuceni, d.datumVydani,
                           d.PACIENT_ID_Pacient, d.LEKAR_ID_Lekar,
                           p.prijmeni || ' ' || p.jmeno AS pacient_cele,
                           l.prijmeni || ' ' || l.jmeno AS lekar_cele
                    FROM DOPORUCENI d
                    JOIN PACIENT p ON d.PACIENT_ID_Pacient = p.ID_Pacient
                    JOIN LEKAR l ON d.LEKAR_ID_Lekar = l.ID_Lekar
                    ORDER BY d.datumVydani DESC";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Doporuceni
                            {
                                IdDoporuceni = Convert.ToInt32(reader["ID_doporuceni"]),

                                Duvod = reader["duvodDoporuceni"].ToString(),
                                Datum = Convert.ToDateTime(reader["datumVydani"]),

                                IdPacient = Convert.ToInt32(reader["PACIENT_ID_Pacient"]),
                                PacientJmeno = reader["pacient_cele"].ToString(),
                                IdLekar = Convert.ToInt32(reader["LEKAR_ID_Lekar"]),
                                LekarJmeno = reader["lekar_cele"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        // 2. PŘIDAT DOPORUČENÍ
        public void PridatDoporuceni(Doporuceni d)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = @"INSERT INTO DOPORUCENI (duvodDoporuceni, datumVydani, PACIENT_ID_Pacient, LEKAR_ID_Lekar)
                               VALUES (:duvod, :datum, :idPac, :idLek)";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("duvod", d.Duvod);
                    cmd.Parameters.Add("datum", d.Datum);
                    cmd.Parameters.Add("idPac", d.IdPacient);
                    cmd.Parameters.Add("idLek", d.IdLekar);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 3. SMAZAT
        public void SmazatDoporuceni(int id)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "DELETE FROM DOPORUCENI WHERE ID_doporuceni = :id";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

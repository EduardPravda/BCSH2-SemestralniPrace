using System;
using System.Collections.Generic;
using System.Data;
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

        public List<Doporuceni> GetVsechnaDoporuceni()
        {
            var list = new List<Doporuceni>();
            using (var conn = _database.GetConnection())
            {
                string sql = @"
                    SELECT d.ID_Doporuceni, d.datumVydani, d.duvodDoporuceni, d.odbornostKam, -- Pokud nemáš sloupec odbornostKam, můžeš ho vynechat
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
                            var dop = new Doporuceni
                            {
                                IdDoporuceni = Convert.ToInt32(reader["ID_Doporuceni"]),
                                Datum = Convert.ToDateTime(reader["datumVydani"]),
                                Duvod = reader["duvodDoporuceni"].ToString(),
                                IdPacient = Convert.ToInt32(reader["PACIENT_ID_Pacient"]),
                                IdLekar = Convert.ToInt32(reader["LEKAR_ID_Lekar"]),
                                PacientJmeno = reader["pacient_cele"].ToString(),
                                LekarJmeno = reader["lekar_cele"].ToString()
                            };

                            try { dop.OdbornostKam = reader["odbornostKam"].ToString(); } catch { }

                            list.Add(dop);
                        }
                    }
                }
            }
            return list;
        }

        public void VystavitDoporuceni(Doporuceni d)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = @"INSERT INTO DOPORUCENI (datumVydani, duvodDoporuceni, PACIENT_ID_Pacient, LEKAR_ID_Lekar)
                               VALUES (:datum, :duvod, :idPac, :idLek)";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("datum", d.Datum);
                    cmd.Parameters.Add("duvod", d.Duvod);
                    cmd.Parameters.Add("idPac", d.IdPacient);
                    cmd.Parameters.Add("idLek", d.IdLekar);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SmazatDoporuceni(int id)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "DELETE FROM DOPORUCENI WHERE ID_Doporuceni = :id";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
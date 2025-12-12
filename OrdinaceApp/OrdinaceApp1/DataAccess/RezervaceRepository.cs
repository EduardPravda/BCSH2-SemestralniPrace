using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class RezervaceRepository
    {
        private readonly Database _database;

        public RezervaceRepository()
        {
            _database = new Database();
        }

        // 1. SEZNAM (S JOINEM NA LÉKAŘE)
        public List<Rezervace> GetVsechnyRezervace()
        {
            var list = new List<Rezervace>();
            using (var conn = _database.GetConnection())
            {
                // SQL PŘÍKAZ S LÉKAŘEM
                string sql = @"
                    SELECT r.ID_Rezervace, r.datumACas, 
                           r.PACIENT_ID_Pacient, r.LEKAR_ID_Lekar,
                           p.prijmeni || ' ' || p.jmeno AS pacient_cele,
                           l.prijmeni || ' ' || l.jmeno AS lekar_cele
                    FROM REZERVACE r
                    JOIN PACIENT p ON r.PACIENT_ID_Pacient = p.ID_Pacient
                    LEFT JOIN LEKAR l ON r.LEKAR_ID_Lekar = l.ID_Lekar
                    ORDER BY r.datumACas";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var r = new Rezervace
                            {
                                IdRezervace = Convert.ToInt32(reader["ID_Rezervace"]),
                                Datum = Convert.ToDateTime(reader["datumACas"]),
                                IdPacient = Convert.ToInt32(reader["PACIENT_ID_Pacient"]),
                                PacientJmeno = reader["pacient_cele"].ToString()
                            };

                            // Načtení lékaře (pokud není NULL)
                            if (reader["LEKAR_ID_Lekar"] != DBNull.Value)
                            {
                                r.IdLekar = Convert.ToInt32(reader["LEKAR_ID_Lekar"]);
                                r.LekarJmeno = reader["lekar_cele"].ToString();
                            }
                            else r.LekarJmeno = "---";

                            list.Add(r);
                        }
                    }
                }
            }
            return list;
        }

        // 1. Získat rezervace konkrétního pacienta
        public List<Rezervace> GetRezervacePacienta(int idPacient)
        {
            var list = new List<Rezervace>();
            using (var conn = _database.GetConnection())
            {
                string sql = @"
                    SELECT r.ID_Rezervace, r.datumACas, 
                           r.PACIENT_ID_Pacient, r.LEKAR_ID_Lekar,
                           p.prijmeni || ' ' || p.jmeno AS pacient_cele,
                           l.prijmeni || ' ' || l.jmeno AS lekar_cele
                    FROM REZERVACE r
                    JOIN PACIENT p ON r.PACIENT_ID_Pacient = p.ID_Pacient
                    LEFT JOIN LEKAR l ON r.LEKAR_ID_Lekar = l.ID_Lekar
                    WHERE r.PACIENT_ID_Pacient = :id
                    ORDER BY r.datumACas";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", idPacient);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var r = new Rezervace
                            {
                                IdRezervace = Convert.ToInt32(reader["ID_Rezervace"]),
                                Datum = Convert.ToDateTime(reader["datumACas"]),
                                IdPacient = Convert.ToInt32(reader["PACIENT_ID_Pacient"]),
                                PacientJmeno = reader["pacient_cele"].ToString()
                            };
                            if (reader["LEKAR_ID_Lekar"] != DBNull.Value)
                            {
                                r.IdLekar = Convert.ToInt32(reader["LEKAR_ID_Lekar"]);
                                r.LekarJmeno = reader["lekar_cele"].ToString();
                            }
                            else r.LekarJmeno = "---";
                            list.Add(r);
                        }
                    }
                }
            }
            return list;
        }

        // 2. Získat obsazené časy lékaře v daný den (pro výpočet volných bloků)
        public List<DateTime> GetObsazeneTerminy(int idLekar, DateTime datum)
        {
            var list = new List<DateTime>();
            using (var conn = _database.GetConnection())
            {
                // Vybereme rezervace pro lékaře v ten den
                string sql = @"SELECT datumACas FROM REZERVACE 
                               WHERE LEKAR_ID_Lekar = :lid 
                               AND TRUNC(datumACas) = TRUNC(:datum)";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("lid", idLekar);
                    cmd.Parameters.Add("datum", datum);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(Convert.ToDateTime(reader["datumACas"]));
                        }
                    }
                }
            }
            return list;
        }

        // 2. PŘIDAT (S LÉKAŘEM)
        public void PridatRezervaci(Rezervace r)
        {
            using (var conn = _database.GetConnection())
            {
                // INSERT VČETNĚ LÉKAŘE
                string sql = @"INSERT INTO REZERVACE (datumACas, PACIENT_ID_Pacient, LEKAR_ID_Lekar)
                               VALUES (:datum, :idPac, :idLek)";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("datum", r.Datum);
                    cmd.Parameters.Add("idPac", r.IdPacient);

                    if (r.IdLekar > 0) cmd.Parameters.Add("idLek", r.IdLekar);
                    else cmd.Parameters.Add("idLek", DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SmazatRezervaci(int id)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "DELETE FROM REZERVACE WHERE ID_Rezervace = :id";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
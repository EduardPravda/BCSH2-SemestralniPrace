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

        public List<Rezervace> GetVsechnyRezervace()
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
                    JOIN LEKAR l ON r.LEKAR_ID_Lekar = l.ID_Lekar
                    ORDER BY r.datumACas";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Rezervace
                            {
                                IdRezervace = Convert.ToInt32(reader["ID_Rezervace"]),
                                Datum = Convert.ToDateTime(reader["datumACas"]),
                                IdPacient = Convert.ToInt32(reader["PACIENT_ID_Pacient"]),
                                IdLekar = Convert.ToInt32(reader["LEKAR_ID_Lekar"]),
                                PacientJmeno = reader["pacient_cele"].ToString(),
                                LekarJmeno = reader["lekar_cele"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        
        public void PridatRezervaci(Rezervace r)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = @"INSERT INTO REZERVACE (datumACas, PACIENT_ID_Pacient, LEKAR_ID_Lekar)
                               VALUES (:datum, :idPac, :idLek)";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("datum", r.Datum);
                    cmd.Parameters.Add("idPac", r.IdPacient);
                    cmd.Parameters.Add("idLek", r.IdLekar);

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
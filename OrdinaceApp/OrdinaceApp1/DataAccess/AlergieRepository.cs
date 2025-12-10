using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class AlergieRepository
    {
        private readonly Database _database;

        public AlergieRepository()
        {
            _database = new Database();
        }
                public List<Alergie> GetVsechnyAlergie()
        {
            var list = new List<Alergie>();
            using (var conn = _database.GetConnection())
            {
                string sql = @"
                    SELECT a.ID_Alergie, a.nazevAlergie, a.PACIENT_ID_Pacient,
                           p.prijmeni || ' ' || p.jmeno AS pacient_cele
                    FROM ALERGIE a
                    JOIN PACIENT p ON a.PACIENT_ID_Pacient = p.ID_Pacient
                    ORDER BY p.prijmeni";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Alergie
                            {
                                IdAlergie = Convert.ToInt32(reader["ID_Alergie"]),
                                Nazev = reader["nazevAlergie"].ToString(),
                                IdPacient = Convert.ToInt32(reader["PACIENT_ID_Pacient"]),
                                PacientJmeno = reader["pacient_cele"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }
                public void PridatAlergii(Alergie a)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "INSERT INTO ALERGIE (nazevAlergie, PACIENT_ID_Pacient) VALUES (:nazev, :idPac)";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("nazev", a.Nazev);
                    cmd.Parameters.Add("idPac", a.IdPacient);
                    cmd.ExecuteNonQuery();
                }
            }
        }

       public void SmazatAlergii(int id)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "DELETE FROM ALERGIE WHERE ID_Alergie = :id";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
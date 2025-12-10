using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class AnamnezaRepository
    {
        private readonly Database _database;

        public AnamnezaRepository()
        {
            _database = new Database();
        }

        public List<Anamneza> GetVsechnyAnamnezy()
        {
            var list = new List<Anamneza>();
            using (var conn = _database.GetConnection())
            {
                string sql = @"
                    SELECT a.ID_Anamneza, a.datumZaznamu, a.typ_anamnezy, a.poznamky, a.PACIENT_ID_Pacient,
                           p.prijmeni || ' ' || p.jmeno AS pacient_cele
                    FROM ANAMNEZA a
                    JOIN PACIENT p ON a.PACIENT_ID_Pacient = p.ID_Pacient
                    ORDER BY p.prijmeni, a.datumZaznamu DESC";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Anamneza
                            {
                                IdAnamneza = Convert.ToInt32(reader["ID_Anamneza"]),
                                Datum = Convert.ToDateTime(reader["datumZaznamu"]),
                                Typ = reader["typ_anamnezy"].ToString(),
                                Poznamky = reader["poznamky"].ToString(),
                                IdPacient = Convert.ToInt32(reader["PACIENT_ID_Pacient"]),
                                PacientJmeno = reader["pacient_cele"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public void PridatAnamnezu(Anamneza a)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = @"INSERT INTO ANAMNEZA (datumZaznamu, typ_anamnezy, poznamky, PACIENT_ID_Pacient)
                               VALUES (:datum, :typ, :pozn, :idPac)";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("datum", a.Datum);
                    cmd.Parameters.Add("typ", a.Typ);
                    cmd.Parameters.Add("pozn", a.Poznamky);
                    cmd.Parameters.Add("idPac", a.IdPacient);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SmazatAnamnezu(int id)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "DELETE FROM ANAMNEZA WHERE ID_Anamneza = :id";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class VysetreniRepository
    {
        private readonly Database _database;

        public VysetreniRepository()
        {
            _database = new Database();
        }

        public List<Vysetreni> GetVsechnaVysetreni()
        {
            var list = new List<Vysetreni>();
            using (var conn = _database.GetConnection())
            {
                // Spojíme tabulky, abychom viděli jména, ne jen čísla
                string sql = @"
                    SELECT v.ID_Vysetreni, v.datumVysetreni, v.typVysetreni, v.poznamka, 
                           v.PACIENT_ID_Pacient, v.LEKAR_ID_Lekar,
                           p.prijmeni || ' ' || p.jmeno AS pacient_cele,
                           l.prijmeni || ' ' || l.jmeno AS lekar_cele
                    FROM VYSETRENI v
                    JOIN PACIENT p ON v.PACIENT_ID_Pacient = p.ID_Pacient
                    JOIN LEKAR l ON v.LEKAR_ID_Lekar = l.ID_Lekar
                    ORDER BY v.datumVysetreni DESC";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Vysetreni
                            {
                                IdVysetreni = Convert.ToInt32(reader["ID_Vysetreni"]),
                                Datum = Convert.ToDateTime(reader["datumVysetreni"]),
                                Typ = reader["typVysetreni"].ToString(),
                                Poznamka = reader["poznamka"].ToString(),
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

        public void PridatVysetreni(Vysetreni v)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = @"INSERT INTO VYSETRENI (typVysetreni, datumVysetreni, poznamka, PACIENT_ID_Pacient, LEKAR_ID_Lekar)
                               VALUES (:typ, :datum, :pozn, :idPac, :idLek)";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("typ", v.Typ);
                    cmd.Parameters.Add("datum", v.Datum);
                    cmd.Parameters.Add("pozn", OracleDbType.Clob).Value = v.Poznamka;
                    cmd.Parameters.Add("idPac", v.IdPacient);
                    cmd.Parameters.Add("idLek", v.IdLekar);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SmazatVysetreni(int id)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "DELETE FROM VYSETRENI WHERE ID_Vysetreni = :id";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
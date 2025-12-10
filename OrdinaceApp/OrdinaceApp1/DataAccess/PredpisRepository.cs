using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class PredpisRepository
    {
        private readonly Database _database;

        public PredpisRepository()
        {
            _database = new Database();
        }

        public List<Predpis> GetVsechnyPredpisy()
        {
            var list = new List<Predpis>();
            using (var conn = _database.GetConnection())
            {
                string sql = @"
                    SELECT p.ID_Predpis, p.datumVydani, p.davkovani, p.delkaLecby, p.popisLeku,
                           p.PACIENT_ID_Pacient, p.LEK_ID_Lek, p.LEKAR_ID_Lekar,
                           pac.prijmeni || ' ' || pac.jmeno AS pacient_cele,
                           l.nazevLeku,
                           doc.prijmeni || ' ' || doc.jmeno AS lekar_cele
                    FROM LEKARSKY_PREDPIS p
                    JOIN PACIENT pac ON p.PACIENT_ID_Pacient = pac.ID_Pacient
                    JOIN LEK l ON p.LEK_ID_Lek = l.ID_Lek
                    JOIN LEKAR doc ON p.LEKAR_ID_Lekar = doc.ID_Lekar
                    ORDER BY p.datumVydani DESC";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Predpis
                            {
                                IdPredpis = Convert.ToInt32(reader["ID_Predpis"]),
                                DatumVydani = Convert.ToDateTime(reader["datumVydani"]),
                                Davkovani = reader["davkovani"].ToString(),
                                DelkaLecby = reader["delkaLecby"].ToString(),
                                Poznamka = reader["popisLeku"].ToString(),
                                IdPacient = Convert.ToInt32(reader["PACIENT_ID_Pacient"]),
                                IdLek = Convert.ToInt32(reader["LEK_ID_Lek"]),
                                IdLekar = Convert.ToInt32(reader["LEKAR_ID_Lekar"]),

                                PacientJmeno = reader["pacient_cele"].ToString(),
                                LekNazev = reader["nazevLeku"].ToString(),
                                LekarJmeno = reader["lekar_cele"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public void PridatPredpis(Predpis p)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = @"INSERT INTO LEKARSKY_PREDPIS 
                               (datumVydani, davkovani, delkaLecby, popisLeku, PACIENT_ID_Pacient, LEK_ID_Lek, LEKAR_ID_Lekar)
                               VALUES (:datum, :davka, :delka, :popis, :idPac, :idLek, :idDoc)";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("datum", p.DatumVydani);
                    cmd.Parameters.Add("davka", p.Davkovani);
                    cmd.Parameters.Add("delka", p.DelkaLecby);
                    cmd.Parameters.Add("popis", p.Poznamka);
                    cmd.Parameters.Add("idPac", p.IdPacient);
                    cmd.Parameters.Add("idLek", p.IdLek);
                    cmd.Parameters.Add("idDoc", p.IdLekar);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SmazatPredpis(int id)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "DELETE FROM LEKARSKY_PREDPIS WHERE ID_Predpis = :id";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
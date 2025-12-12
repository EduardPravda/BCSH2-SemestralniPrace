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
                    SELECT 
                        lp.ID_PREDPIS,
                        lp.DATUMVYDANI,   
                        lp.POPISLEKU,     
                        lp.DAVKOVANI,
                        lp.DELKALECBY,
                        p.Prijmeni || ' ' || p.Jmeno AS PacientJmeno,
                        l.Prijmeni || ' ' || l.Jmeno AS LekarJmeno
                    FROM LEKARSKY_PREDPIS lp
                    JOIN VYSETRENI v ON lp.VYSETRENI_ID_VYSETRENI = v.ID_VYSETRENI
                    JOIN PACIENT p ON v.PACIENT_ID_Pacient = p.ID_Pacient
                    JOIN LEKAR l ON lp.LEKAR_ID_LEKAR = l.ID_Lekar
                    ORDER BY lp.DATUMVYDANI DESC";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Predpis
                            {
                                IdPredpis = Convert.ToInt32(reader["ID_PREDPIS"]),
                                DatumVydani = Convert.ToDateTime(reader["DATUMVYDANI"]),

                                LekNazev = reader["POPISLEKU"].ToString(),

                                Davkovani = reader["DAVKOVANI"].ToString(),
                                DelkaLecby = reader["DELKALECBY"] != DBNull.Value ? reader["DELKALECBY"].ToString() : "",
                                PacientJmeno = reader["PacientJmeno"].ToString(),
                                LekarJmeno = reader["LekarJmeno"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public void PridatPredpis(Predpis p, int idPacient, int idLekar)
        {
            using (var conn = _database.GetConnection())
            {
                int idVysetreni = 0;

                string sqlGetVysetreni = "SELECT ID_VYSETRENI FROM VYSETRENI WHERE PACIENT_ID_PACIENT = :idPac ORDER BY DATUMVYSETRENI DESC FETCH FIRST 1 ROWS ONLY";

                using (var cmdCheck = new OracleCommand(sqlGetVysetreni, conn))
                {
                    cmdCheck.Parameters.Add("idPac", idPacient);
                    var result = cmdCheck.ExecuteScalar();

                    if (result != null)
                    {
                        idVysetreni = Convert.ToInt32(result);
                    }
                    else
                    {
                        throw new Exception("Chyba: Pacient nemá žádné vyšetření! Recept musí být vázán na vyšetření.");
                    }
                }

                string sql = @"INSERT INTO LEKARSKY_PREDPIS 
                       (DATUMVYDANI, POPISLEKU, DAVKOVANI, DELKALECBY, LEKAR_ID_LEKAR, VYSETRENI_ID_VYSETRENI) 
                       VALUES (:datum, :popis, :davka, :delka, :idLekar, :idVysetreni)";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("datum", p.DatumVydani);
                    cmd.Parameters.Add("popis", p.LekNazev);
                    cmd.Parameters.Add("davka", p.Davkovani);
                    cmd.Parameters.Add("delka", p.DelkaLecby);
                    cmd.Parameters.Add("idLekar", idLekar);
                    cmd.Parameters.Add("idVysetreni", idVysetreni);

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

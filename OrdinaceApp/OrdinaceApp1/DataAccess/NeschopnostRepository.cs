using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class NeschopnostRepository
    {
        private readonly Database _database;

        public NeschopnostRepository()
        {
            _database = new Database();
        }

        // 1. SEZNAM (S pacientem a lékařem)
        public List<Neschopnost> GetVsechnyNeschopnosti()
        {
            var list = new List<Neschopnost>();
            using (var conn = _database.GetConnection())
            {
                string sql = @"
                    SELECT n.ID_Neschopnost, n.datumOd, n.datumDo, n.duvod,
                           n.PACIENT_ID_Pacient, n.LEKAR_ID_Lekar,
                           p.prijmeni || ' ' || p.jmeno AS pacient_cele,
                           l.prijmeni || ' ' || l.jmeno AS lekar_cele
                    FROM PRACOVNI_NESCHOPNOST n
                    JOIN PACIENT p ON n.PACIENT_ID_Pacient = p.ID_Pacient
                    JOIN LEKAR l ON n.LEKAR_ID_Lekar = l.ID_Lekar
                    ORDER BY n.datumOd DESC";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var n = new Neschopnost
                            {
                                IdNeschopnost = Convert.ToInt32(reader["ID_Neschopnost"]),
                                DatumOd = Convert.ToDateTime(reader["datumOd"]),
                                Duvod = reader["duvod"].ToString(),
                                IdPacient = Convert.ToInt32(reader["PACIENT_ID_Pacient"]),
                                PacientJmeno = reader["pacient_cele"].ToString(),
                                IdLekar = Convert.ToInt32(reader["LEKAR_ID_Lekar"]),
                                LekarJmeno = reader["lekar_cele"].ToString()
                            };

                            // DatumDo může být NULL (nemoc stále trvá)
                            if (reader["datumDo"] != DBNull.Value)
                            {
                                n.DatumDo = Convert.ToDateTime(reader["datumDo"]);
                            }

                            list.Add(n);
                        }
                    }
                }
            }
            return list;
        }

        // 2. VYTVOŘIT NESCHOPENKU
        public void PridatNeschopnost(Neschopnost n)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = @"INSERT INTO PRACOVNI_NESCHOPNOST (datumOd, datumDo, duvod, PACIENT_ID_Pacient, LEKAR_ID_Lekar, VYSETRENI_ID_Vysetreni)
                                VALUES (:od, :do, :duv, :idPac, :idLek, 1)";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("od", n.DatumOd);

                    if (n.DatumDo.HasValue)
                        cmd.Parameters.Add("do", n.DatumDo.Value);
                    else
                        cmd.Parameters.Add("do", DBNull.Value);

                    cmd.Parameters.Add("duv", n.Duvod);
                    cmd.Parameters.Add("idPac", n.IdPacient);
                    cmd.Parameters.Add("idLek", n.IdLekar);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SmazatNeschopnost(int id)
        {
            using (var conn = _database.GetConnection())
            {
                // SQL příkaz pro smazání podle ID
                string sql = "DELETE FROM PRACOVNI_NESCHOPNOST WHERE ID_Neschopnost = :id";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
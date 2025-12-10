using System;
using System.Collections.Generic;
using System.Data;
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

        public List<Neschopnost> GetVsechnyNeschopnosti()
        {
            var list = new List<Neschopnost>();
            using (var conn = _database.GetConnection())
            {
                string sql = @"
                    SELECT n.ID_Neschopnost, n.zacatekNeschopnosti, n.konecNeschopnosti, n.duvod,
                           n.PACIENT_ID_Pacient, n.LEKAR_ID_Lekar,
                           p.prijmeni || ' ' || p.jmeno AS pacient_cele,
                           l.prijmeni || ' ' || l.jmeno AS lekar_cele
                    FROM PRACOVNI_NESCHOPNOST n
                    JOIN PACIENT p ON n.PACIENT_ID_Pacient = p.ID_Pacient
                    JOIN LEKAR l ON n.LEKAR_ID_Lekar = l.ID_Lekar
                    ORDER BY n.zacatekNeschopnosti DESC";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pn = new Neschopnost
                            {
                                IdNeschopnost = Convert.ToInt32(reader["ID_Neschopnost"]),
                                Zacatek = Convert.ToDateTime(reader["zacatekNeschopnosti"]),
                                Duvod = reader["duvod"].ToString(),
                                IdPacient = Convert.ToInt32(reader["PACIENT_ID_Pacient"]),
                                IdLekar = Convert.ToInt32(reader["LEKAR_ID_Lekar"]),
                                PacientJmeno = reader["pacient_cele"].ToString(),
                                LekarJmeno = reader["lekar_cele"].ToString()
                            };

                            if (reader["konecNeschopnosti"] != DBNull.Value)
                                pn.Konec = Convert.ToDateTime(reader["konecNeschopnosti"]);

                            list.Add(pn);
                        }
                    }
                }
            }
            return list;
        }

        public void VystavitNeschopnost(Neschopnost n)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = @"INSERT INTO PRACOVNI_NESCHOPNOST (zacatekNeschopnosti, konecNeschopnosti, duvod, PACIENT_ID_Pacient, LEKAR_ID_Lekar)
                               VALUES (:zacatek, :konec, :duvod, :idPac, :idLek)";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("zacatek", n.Zacatek);

                    if (n.Konec.HasValue)
                        cmd.Parameters.Add("konec", n.Konec.Value);
                    else
                        cmd.Parameters.Add("konec", DBNull.Value);

                    cmd.Parameters.Add("duvod", n.Duvod);
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
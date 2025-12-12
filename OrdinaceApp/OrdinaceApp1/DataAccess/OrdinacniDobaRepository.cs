using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class OrdinacniDobaRepository
    {
        private readonly Database _database;

        public OrdinacniDobaRepository()
        {
            _database = new Database();
        }

        public List<OrdinacniDoba> GetVsechnyCasy()
        {
            var list = new List<OrdinacniDoba>();
            using (var conn = _database.GetConnection())
            {
                string sql = @"
            SELECT o.ID_Ordinacni_Doby, o.den, o.zacatek, o.konec, o.LEKAR_ID_Lekar,
                   l.prijmeni || ' ' || l.jmeno AS lekar_jmeno
            FROM ORDINACNI_DOBA o
            JOIN LEKAR l ON o.LEKAR_ID_Lekar = l.ID_Lekar
            ORDER BY l.prijmeni, o.den";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new OrdinacniDoba
                            {

                                IdOrdinacniDoby = Convert.ToInt32(reader["ID_ORDINACNI_DOBY"]),
                                Den = reader["DEN"].ToString(),

                                Zacatek = Convert.ToDateTime(reader["ZACATEK"]),
                                Konec = Convert.ToDateTime(reader["KONEC"]),

                                IdLekar = Convert.ToInt32(reader["LEKAR_ID_LEKAR"]),

                                LekarJmeno = reader["LEKAR_JMENO"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public void PridatDobu(OrdinacniDoba o)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "INSERT INTO ORDINACNI_DOBA (den, zacatek, konec, LEKAR_ID_Lekar) VALUES (:den, :zac, :kon, :idLek)";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("den", o.Den);

                    cmd.Parameters.Add("zac", o.Zacatek);
                    cmd.Parameters.Add("kon", o.Konec);

                    cmd.Parameters.Add("idLek", o.IdLekar);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SmazatDobu(int id)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "DELETE FROM ORDINACNI_DOBA WHERE ID_Ordinacni_Doby = :id";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

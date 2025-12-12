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
                // 1. V SQL použijeme jednoduchý alias bez uvozovek: AS lekar_jmeno
                // Oracle si to interně převede na LEKAR_JMENO
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
                                // Pozor na velká/malá písmena v názvech sloupců!
                                // U Oracle je nejbezpečnější používat VŽDY VELKÁ PÍSMENA.

                                IdOrdinacniDoby = Convert.ToInt32(reader["ID_ORDINACNI_DOBY"]),
                                Den = reader["DEN"].ToString(),

                                // Načítáme jako DateTime
                                Zacatek = Convert.ToDateTime(reader["ZACATEK"]),
                                Konec = Convert.ToDateTime(reader["KONEC"]),

                                IdLekar = Convert.ToInt32(reader["LEKAR_ID_LEKAR"]),

                                // 2. Tady čteme sloupec VELKÝMI PÍSMENY: LEKAR_JMENO
                                // Protože v SQL jsme napsali 'AS lekar_jmeno'
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
                // SQL zůstává stejné, ale parametry níže budou mít jiný typ
                string sql = "INSERT INTO ORDINACNI_DOBA (den, zacatek, konec, LEKAR_ID_Lekar) VALUES (:den, :zac, :kon, :idLek)";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("den", o.Den);

                    // OPRAVA: Zde se odesílá objekt DateTime. 
                    // Ovladač Oracle to automaticky převede na správný formát DATE.
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
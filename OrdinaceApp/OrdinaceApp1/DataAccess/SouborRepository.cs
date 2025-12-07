using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class SouborRepository
    {
        private readonly Database _database;

        public SouborRepository()
        {
            _database = new Database();
        }

        public List<Soubor> GetSeznamSouboru()
        {
            var list = new List<Soubor>();
            using (var conn = _database.GetConnection())
            {
                string sql = "SELECT id_soubor, nazev_souboru, typ_souboru, pripona, datum_nahrani, popis, UZIVATEL_id_uzivatel FROM SOUBOR ORDER BY datum_nahrani DESC";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Soubor
                            {
                                IdSoubor = Convert.ToInt32(reader["id_soubor"]),
                                Nazev = reader["nazev_souboru"].ToString(),
                                Typ = reader["typ_souboru"].ToString(),
                                Pripona = reader["pripona"].ToString(),
                                DatumNahrani = Convert.ToDateTime(reader["datum_nahrani"]),
                                Popis = reader["popis"].ToString(),
                                IdUzivatel = Convert.ToInt32(reader["UZIVATEL_id_uzivatel"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        public void NahratSoubor(string nazev, string typ, string pripona, byte[] data, string popis, int idUzivatel)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "INSERT INTO SOUBOR (nazev_souboru, typ_souboru, pripona, binarni_data, datum_nahrani, popis, UZIVATEL_id_uzivatel) VALUES (:nazev, :typ, :pripona, :blob, SYSDATE, :popis, :userid)";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("nazev", nazev);
                    cmd.Parameters.Add("typ", typ);
                    cmd.Parameters.Add("pripona", pripona);

                    var blobParam = new OracleParameter("blob", OracleDbType.Blob);
                    blobParam.Value = data;
                    cmd.Parameters.Add(blobParam);

                    cmd.Parameters.Add("popis", popis);
                    cmd.Parameters.Add("userid", idUzivatel);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
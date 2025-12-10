using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class LekRepository
    {
        private readonly Database _database;

        public LekRepository()
        {
            _database = new Database();
        }

        public List<Lek> GetVsechnyLeky()
        {
            var list = new List<Lek>();
            using (var conn = _database.GetConnection())
            {
                string sql = "SELECT ID_Lek, nazevLeku, ucinnaLatka, doporuceneDavkovani, vedlejsiUcinky, cena FROM LEK ORDER BY nazevLeku";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Lek
                            {
                                IdLek = Convert.ToInt32(reader["ID_Lek"]),
                                Nazev = reader["nazevLeku"].ToString(),
                                UcinnaLatka = reader["ucinnaLatka"].ToString(),
                                Davkovani = reader["doporuceneDavkovani"].ToString(),
                                VedlejsiUcinky = reader["vedlejsiUcinky"].ToString(),
                                Cena = Convert.ToDecimal(reader["cena"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        public void PridatLek(Lek lek)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = @"INSERT INTO LEK (nazevLeku, ucinnaLatka, doporuceneDavkovani, vedlejsiUcinky, cena) 
                               VALUES (:nazev, :latka, :davkovani, :ucinky, :cena)";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("nazev", lek.Nazev);
                    cmd.Parameters.Add("latka", lek.UcinnaLatka);
                    cmd.Parameters.Add("davkovani", OracleDbType.Clob).Value = lek.Davkovani;
                    cmd.Parameters.Add("ucinky", OracleDbType.Clob).Value = lek.VedlejsiUcinky;
                    cmd.Parameters.Add("cena", lek.Cena);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpravitLek(Lek lek)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = @"UPDATE LEK SET 
                               nazevLeku = :nazev, 
                               ucinnaLatka = :latka, 
                               doporuceneDavkovani = :davkovani, 
                               vedlejsiUcinky = :ucinky, 
                               cena = :cena 
                               WHERE ID_Lek = :id";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("nazev", lek.Nazev);
                    cmd.Parameters.Add("latka", lek.UcinnaLatka);
                    cmd.Parameters.Add("davkovani", OracleDbType.Clob).Value = lek.Davkovani;
                    cmd.Parameters.Add("ucinky", OracleDbType.Clob).Value = lek.VedlejsiUcinky;
                    cmd.Parameters.Add("cena", lek.Cena);
                    cmd.Parameters.Add("id", lek.IdLek);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SmazatLek(int idLek)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "DELETE FROM LEK WHERE ID_Lek = :id";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", idLek);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
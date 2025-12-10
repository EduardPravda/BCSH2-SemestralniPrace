using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class NemocRepository
    {
        private readonly Database _database;

        public NemocRepository()
        {
            _database = new Database();
        }

        public List<Nemoc> GetVsechnyNemoci()
        {
            var list = new List<Nemoc>();
            using (var conn = _database.GetConnection())
            {
                string sql = "SELECT * FROM NEMOC ORDER BY nazevNemoci";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var n = new Nemoc
                            {
                                IdNemoc = Convert.ToInt32(reader["ID_Nemoc"]),
                                Nazev = reader["nazevNemoci"].ToString()
                            };

                            if (reader["stavNemoci"] != DBNull.Value)
                            {
                                n.Popis = reader["stavNemoci"].ToString();
                            }

                            if (reader["datumDiagnozy"] != DBNull.Value)
                            {
                                n.Kod = reader["datumDiagnozy"].ToString();
                            }

                            list.Add(n);
                        }
                    }
                }
            }
            return list;
        }

        public void PridatNemoc(Nemoc n)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "INSERT INTO NEMOC (nazevNemoci, stavNemoci, datumDiagnozy, ANAMNEZA_ID_ANAMNEZA) VALUES (:nazev, :stav, :datum, :idAnam)";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("nazev", n.Nazev);

                    if (!string.IsNullOrEmpty(n.Popis))
                        cmd.Parameters.Add("stav", n.Popis);
                    else
                        cmd.Parameters.Add("stav", DBNull.Value);

                    cmd.Parameters.Add("datum", DateTime.Now);

                    cmd.Parameters.Add("idAnam", 1);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpravitNemoc(Nemoc n)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "UPDATE NEMOC SET nazevNemoci = :nazev, stavNemoci = :stav WHERE ID_Nemoc = :id";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("nazev", n.Nazev);

                    if (!string.IsNullOrEmpty(n.Popis))
                        cmd.Parameters.Add("stav", n.Popis);
                    else
                        cmd.Parameters.Add("stav", DBNull.Value);

                    cmd.Parameters.Add("id", n.IdNemoc);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        public void SmazatNemoc(int id)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "DELETE FROM NEMOC WHERE ID_Nemoc = :id";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
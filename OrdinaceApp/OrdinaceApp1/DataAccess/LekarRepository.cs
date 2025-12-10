using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class LekarRepository
    {
        private readonly Database _database;

        public LekarRepository()
        {
            _database = new Database();
        }

        public List<Lekar> GetHierarchielekaru()
        {
            var list = new List<Lekar>();
            using (var conn = _database.GetConnection())
            {
                string sql = @"SELECT LEVEL as Uroven, jmeno, prijmeni, specializace 
                               FROM LEKAR
                               START WITH ID_Vedouci_Lekar IS NULL
                               CONNECT BY PRIOR ID_Lekar = ID_Vedouci_Lekar
                               ORDER SIBLINGS BY prijmeni";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Lekar
                            {
                                Uroven = Convert.ToInt32(reader["Uroven"]),
                                Jmeno = reader["jmeno"].ToString(),
                                Prijmeni = reader["prijmeni"].ToString(),
                                Specializace = reader["specializace"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }
        public List<Lekar> GetVsechnyLekare()
        {
            var list = new List<Lekar>();
            using (var conn = _database.GetConnection())
            {
                string sql = "SELECT * FROM LEKAR ORDER BY prijmeni";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var l = new Lekar
                            {
                                IdLekar = Convert.ToInt32(reader["ID_Lekar"]),
                                Jmeno = reader["jmeno"].ToString(),
                                Prijmeni = reader["prijmeni"].ToString(),
                                Specializace = reader["specializace"].ToString(),
                                Telefon = reader["telefon"].ToString(),
                                Email = reader["email"].ToString(),
                                IdUzivatel = Convert.ToInt32(reader["UZIVATEL_id_uzivatel"])
                            };

                            if (reader["ID_Vedouci_Lekar"] != DBNull.Value)
                            {
                                l.IdVedouci = Convert.ToInt32(reader["ID_Vedouci_Lekar"]);
                            }

                            list.Add(l);
                        }
                    }
                }
            }
            return list;
        }
        
        public void PridatLekare(Lekar l)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = @"INSERT INTO LEKAR (jmeno, prijmeni, specializace, telefon, email, UZIVATEL_id_uzivatel, ID_Vedouci_Lekar)
                               VALUES (:jmeno, :prijmeni, :spec, :tel, :email, :uid, :vedouci)";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("jmeno", l.Jmeno);
                    cmd.Parameters.Add("prijmeni", l.Prijmeni);
                    cmd.Parameters.Add("spec", l.Specializace);
                    cmd.Parameters.Add("tel", l.Telefon);
                    cmd.Parameters.Add("email", l.Email);
                    cmd.Parameters.Add("uid", l.IdUzivatel);

                    if (l.IdVedouci.HasValue && l.IdVedouci.Value > 0)
                        cmd.Parameters.Add("vedouci", l.IdVedouci.Value);
                    else
                        cmd.Parameters.Add("vedouci", DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpravitLekare(Lekar l)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = @"UPDATE LEKAR SET 
                               jmeno = :jmeno, 
                               prijmeni = :prijmeni, 
                               specializace = :spec, 
                               telefon = :tel, 
                               email = :email, 
                               ID_Vedouci_Lekar = :vedouci
                               WHERE ID_Lekar = :id";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("jmeno", l.Jmeno);
                    cmd.Parameters.Add("prijmeni", l.Prijmeni);
                    cmd.Parameters.Add("spec", l.Specializace);
                    cmd.Parameters.Add("tel", l.Telefon);
                    cmd.Parameters.Add("email", l.Email);

                    if (l.IdVedouci.HasValue && l.IdVedouci.Value > 0)
                        cmd.Parameters.Add("vedouci", l.IdVedouci.Value);
                    else
                        cmd.Parameters.Add("vedouci", DBNull.Value);

                    cmd.Parameters.Add("id", l.IdLekar);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SmazatLekare(int id)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "DELETE FROM LEKAR WHERE ID_Lekar = :id";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
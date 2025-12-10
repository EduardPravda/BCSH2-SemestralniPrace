using System;
using System.Collections.Generic; 
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class UzivatelRepository
    {
        private readonly Database _database;

        public UzivatelRepository()
        {
            _database = new Database();
        }

        public Uzivatel OveritUzivatele(string login, string heslo)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = @"SELECT id_uzivatel, prihlasovaci_jmeno, jmeno, prijmeni, role_id_role, aktivni 
                               FROM UZIVATEL 
                               WHERE prihlasovaci_jmeno = :login 
                               AND heslo = :heslo 
                               AND aktivni = 'A'";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("login", login));
                    cmd.Parameters.Add(new OracleParameter("heslo", heslo));

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Uzivatel
                            {
                                IdUzivatel = Convert.ToInt32(reader["id_uzivatel"]),
                                PrihlasovaciJmeno = reader["prihlasovaci_jmeno"].ToString(),
                                Jmeno = reader["jmeno"].ToString(),
                                Prijmeni = reader["prijmeni"].ToString(),
                                RoleId = Convert.ToInt32(reader["role_id_role"]),
                                Aktivni = reader["aktivni"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }
       public List<Uzivatel> GetVsechnyUzivatele()
        {
            var list = new List<Uzivatel>();
            using (var conn = _database.GetConnection())
            {
                string sql = "SELECT id_uzivatel, prihlasovaci_jmeno, jmeno, prijmeni, role_id_role, aktivni FROM UZIVATEL ORDER BY prihlasovaci_jmeno";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Uzivatel
                            {
                                IdUzivatel = Convert.ToInt32(reader["id_uzivatel"]),
                                PrihlasovaciJmeno = reader["prihlasovaci_jmeno"].ToString(),
                                Jmeno = reader["jmeno"] != DBNull.Value ? reader["jmeno"].ToString() : "",
                                Prijmeni = reader["prijmeni"] != DBNull.Value ? reader["prijmeni"].ToString() : "",
                                RoleId = Convert.ToInt32(reader["role_id_role"]),
                                Aktivni = reader["aktivni"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }


        public void PridatUzivatele(Uzivatel u, string heslo)
        {
            using (var conn = _database.GetConnection())
            {
                // Poznámka: Heslo by se mělo hashovat, pro školní projekt ho ukládáme jako text
                string sql = @"INSERT INTO UZIVATEL (prihlasovaci_jmeno, heslo, jmeno, prijmeni, role_id_role, aktivni, datum_registrace, email)
                               VALUES (:login, :heslo, :jmeno, :prijmeni, :role, :aktivni, SYSDATE, :email)";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("login", u.PrihlasovaciJmeno);
                    cmd.Parameters.Add("heslo", heslo); 
                    cmd.Parameters.Add("jmeno", u.Jmeno);
                    cmd.Parameters.Add("prijmeni", u.Prijmeni);
                    cmd.Parameters.Add("role", u.RoleId);
                    cmd.Parameters.Add("aktivni", u.Aktivni); 
                    cmd.Parameters.Add("email", u.PrihlasovaciJmeno + "@nemocnice.cz"); 

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpravitUzivatele(Uzivatel u, string noveHeslo)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = @"UPDATE UZIVATEL SET 
                               prihlasovaci_jmeno = :login, 
                               jmeno = :jmeno, 
                               prijmeni = :prijmeni, 
                               role_id_role = :role, 
                               aktivni = :aktivni";

                if (!string.IsNullOrEmpty(noveHeslo))
                {
                    sql += ", heslo = :heslo";
                }

                sql += " WHERE id_uzivatel = :id";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("login", u.PrihlasovaciJmeno);
                    cmd.Parameters.Add("jmeno", u.Jmeno);
                    cmd.Parameters.Add("prijmeni", u.Prijmeni);
                    cmd.Parameters.Add("role", u.RoleId);
                    cmd.Parameters.Add("aktivni", u.Aktivni);

                    if (!string.IsNullOrEmpty(noveHeslo))
                    {
                        cmd.Parameters.Add("heslo", noveHeslo);
                    }

                    cmd.Parameters.Add("id", u.IdUzivatel);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SmazatUzivatele(int id)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "DELETE FROM UZIVATEL WHERE id_uzivatel = :id";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
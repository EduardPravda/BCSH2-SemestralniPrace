using System;
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
    }
}
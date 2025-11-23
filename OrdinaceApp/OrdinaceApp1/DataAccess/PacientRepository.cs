using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.DataAccess
{
    public class PacientRepository
    {
        private readonly Database _database;

        public PacientRepository()
        {
            _database = new Database();
        }

        public List<Pacient> GetPacientiPublic()
        {
            var pacienti = new List<Pacient>();

            // 1. Získat spojení
            using (var conn = _database.GetConnection())
            {
                // 2. Připravit SQL příkaz (Použijeme tvůj View z DB)
                string sql = "SELECT jmeno, prijmeni, telefon_mask, mesto FROM V_PACIENT_PUBLIC";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    // 3. Spustit příkaz a získat "čtečku" (Reader)
                    using (var reader = cmd.ExecuteReader())
                    {
                        // 4. Číst řádek po řádku
                        while (reader.Read())
                        {
                            var p = new Pacient();

                            // Pozor: Názvy sloupců v readeru musí sedět na SQL (nejsou case-sensitive)
                            p.Jmeno = reader["jmeno"].ToString();
                            p.Prijmeni = reader["prijmeni"].ToString();
                            p.Telefon = reader["telefon_mask"].ToString();
                            p.Mesto = reader["mesto"].ToString();

                            pacienti.Add(p);
                        }
                    }
                }
            }
            return pacienti;
        }
    }
}

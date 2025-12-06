using System;
using System.Collections.Generic;
using System.Data; // Důležité pro CommandType
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

        // 1. Metoda pro načtení seznamu pacientů (READ)
        public List<Pacient> GetPacientiPublic()
        {
            var pacienti = new List<Pacient>();

            using (var conn = _database.GetConnection())
            {
                // Používáme tvůj View z DB
                string sql = "SELECT jmeno, prijmeni, telefon_mask, mesto FROM V_PACIENT_PUBLIC";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var p = new Pacient();
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

        // 2. NOVÁ METODA: Vložení pacienta přes uloženou proceduru (CREATE)
        public void PridatPacienta(string jmeno, string prijmeni, DateTime datumNarozeni, string ulice, string mesto, string psc, int userId)
        {
            using (var conn = _database.GetConnection())
            {
                // Voláme PL/SQL proceduru SP_Novy_Pacient definovanou v DB
                using (var cmd = new OracleCommand("SP_Novy_Pacient", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parametry procedury (názvy musí sedět přesně na definici v SQL)
                    cmd.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = jmeno;
                    cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = prijmeni;
                    cmd.Parameters.Add("p_dat_nar", OracleDbType.Date).Value = datumNarozeni;
                    cmd.Parameters.Add("p_ulice", OracleDbType.Varchar2).Value = ulice;
                    cmd.Parameters.Add("p_mesto", OracleDbType.Varchar2).Value = mesto;
                    cmd.Parameters.Add("p_psc", OracleDbType.Char).Value = psc;
                    cmd.Parameters.Add("p_user_id", OracleDbType.Int32).Value = userId;

                    // Spustit proceduru (commit je uvnitř procedury v DB)
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
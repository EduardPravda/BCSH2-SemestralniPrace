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
            using (var conn = _database.GetConnection())
            {
                string sql = "SELECT jmeno, prijmeni, telefon_mask, mesto FROM V_PACIENT_PUBLIC";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pacienti.Add(new Pacient
                            {
                                Jmeno = reader["jmeno"].ToString(),
                                Prijmeni = reader["prijmeni"].ToString(),
                                Telefon = reader["telefon_mask"].ToString(),
                                Mesto = reader["mesto"].ToString()
                            });
                        }
                    }
                }
            }
            return pacienti;
        }

        public void PridatPacienta(string jmeno, string prijmeni, DateTime datumNarozeni, string ulice, string mesto, string psc, int userId)
        {
            using (var conn = _database.GetConnection())
            {
                using (var cmd = new OracleCommand("SP_Novy_Pacient", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = jmeno;
                    cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = prijmeni;
                    cmd.Parameters.Add("p_dat_nar", OracleDbType.Date).Value = datumNarozeni;
                    cmd.Parameters.Add("p_ulice", OracleDbType.Varchar2).Value = ulice;
                    cmd.Parameters.Add("p_mesto", OracleDbType.Varchar2).Value = mesto;
                    cmd.Parameters.Add("p_psc", OracleDbType.Char).Value = psc;
                    cmd.Parameters.Add("p_user_id", OracleDbType.Int32).Value = userId;

                    cmd.ExecuteNonQuery();
                }
            }
        }

       public string GenerovatReportAlergii()
        {
            string vysledek = "";

            using (var conn = _database.GetConnection())
            {
                using (var cmd = new OracleCommand("SP_Report_Alergiku", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var outParam = new OracleParameter("p_vystup", OracleDbType.Clob);
                    outParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outParam);

                    cmd.ExecuteNonQuery();

                    if (outParam.Value != DBNull.Value)
                    {
                        vysledek = outParam.Value.ToString();
                    }
                }
            }
            return vysledek;
        }
    }
}
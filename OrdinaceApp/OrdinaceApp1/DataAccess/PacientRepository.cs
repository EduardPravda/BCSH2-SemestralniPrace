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

        /*K tabulce VYSETRENI */
        public List<Pacient> GetPacientiProCombo()
        {
            var list = new List<Pacient>();
            using (var conn = _database.GetConnection())
            {
                string sql = "SELECT ID_Pacient, jmeno, prijmeni FROM PACIENT ORDER BY prijmeni";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Pacient
                            {
                                IdPacient = Convert.ToInt32(reader["ID_Pacient"]),
                                Jmeno = reader["jmeno"].ToString(),
                                Prijmeni = reader["prijmeni"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
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

        public Pacient GetPacientDetail(int idPacient)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "SELECT * FROM PACIENT WHERE ID_Pacient = :id";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", idPacient);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Pacient
                            {
                                IdPacient = Convert.ToInt32(reader["ID_Pacient"]),
                                Jmeno = reader["jmeno"].ToString(),
                                Prijmeni = reader["prijmeni"].ToString(),
                                Mesto = reader["mesto"].ToString(),
                                Telefon = reader["telefon"].ToString(),
                                // Ulice = reader["ulice"].ToString(),
                                // PSC = reader["psc"].ToString(),
                                // IdUzivatel = Convert.ToInt32(reader["UZIVATEL_id_uzivatel"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void UpravitPacienta(Pacient p, int idUzivatel)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = @"UPDATE PACIENT SET 
                               jmeno = :jmeno,
                               prijmeni = :prijmeni,
                               ulice = :ulice,
                               mesto = :mesto,
                               psc = :psc,
                               UZIVATEL_id_uzivatel = :uid
                               WHERE ID_Pacient = :id";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("jmeno", p.Jmeno);
                    cmd.Parameters.Add("prijmeni", p.Prijmeni);
                    cmd.Parameters.Add("ulice", "Neznámá");
                    cmd.Parameters.Add("mesto", p.Mesto);
                    cmd.Parameters.Add("psc", "00000"); 
                    cmd.Parameters.Add("uid", idUzivatel);
                    cmd.Parameters.Add("id", p.IdPacient);

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
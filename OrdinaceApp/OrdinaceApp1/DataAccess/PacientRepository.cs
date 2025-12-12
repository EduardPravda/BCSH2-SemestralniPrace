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
                string sql = "SELECT * FROM V_PACIENT_PUBLIC ORDER BY prijmeni";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var p = new Pacient
                            {
                                IdPacient = Convert.ToInt32(reader["ID_Pacient"]),
                                Jmeno = reader["jmeno"].ToString(),
                                Prijmeni = reader["prijmeni"].ToString(),
                                Telefon = reader["telefon_mask"].ToString(),
                                Mesto = reader["mesto"].ToString(),

                                Vek = reader["vek"] != DBNull.Value ? Convert.ToInt32(reader["vek"]) : 0
                            };

                            // Převod 1/0 na text
                            int neschop = reader["ma_neschopenku"] != DBNull.Value ? Convert.ToInt32(reader["ma_neschopenku"]) : 0;
                            p.StavNeschopenky = (neschop == 1) ? "V pracovní neschopnosti" : "Zdravý";

                            pacienti.Add(p);
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
                string sql = @"
                    SELECT p.ID_Pacient, p.jmeno, p.prijmeni, p.telefon, p.email, p.datumNarozeni,
                           p.ADRESA_ID__Adresa, p.UZIVATEL_id_uzivatel,
                           a.ulice, a.mesto, a.psc
                    FROM PACIENT p
                    JOIN ADRESA a ON p.ADRESA_ID__Adresa = a.ID__Adresa
                    WHERE p.ID_Pacient = :id";

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
                                Telefon = reader["telefon"] != DBNull.Value ? reader["telefon"].ToString() : "",
                                Mesto = reader["mesto"].ToString(),
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<Pacient> GetPacientiLekare(int idLekar)
        {
            var pacienti = new List<Pacient>();
            using (var conn = _database.GetConnection())
            {
                // Vybereme pacienty, kteří mají u lékaře VYŠETŘENÍ nebo REZERVACI
                string sql = @"
                    SELECT DISTINCT p.ID_Pacient, p.jmeno, p.prijmeni, 
                           '***-***-' || SUBSTR(p.telefon, -3) AS telefon_mask, 
                           a.mesto,
                           F_Vypocet_Veku(p.ID_Pacient) AS vek,
                           F_Ma_Neschopenku(p.ID_Pacient) AS ma_neschopenku
                    FROM PACIENT p
                    JOIN ADRESA a ON p.ADRESA_ID__Adresa = a.ID__Adresa
                    WHERE p.ID_Pacient IN (
                        SELECT PACIENT_ID_Pacient FROM VYSETRENI WHERE LEKAR_ID_Lekar = :lid
                        UNION
                        SELECT PACIENT_ID_Pacient FROM REZERVACE WHERE LEKAR_ID_Lekar = :lid
                    )
                    ORDER BY p.prijmeni";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add("lid", idLekar);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var p = new Pacient
                            {
                                IdPacient = Convert.ToInt32(reader["ID_Pacient"]),
                                Jmeno = reader["jmeno"].ToString(),
                                Prijmeni = reader["prijmeni"].ToString(),
                                Telefon = reader["telefon_mask"].ToString(),
                                Mesto = reader["mesto"].ToString(),
                                Vek = reader["vek"] != DBNull.Value ? Convert.ToInt32(reader["vek"]) : 0
                            };

                            int neschop = reader["ma_neschopenku"] != DBNull.Value ? Convert.ToInt32(reader["ma_neschopenku"]) : 0;
                            p.StavNeschopenky = (neschop == 1) ? "V pracovní neschopnosti" : "Zdravý";

                            pacienti.Add(p);
                        }
                    }
                }
            }
            return pacienti;
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

        public int? GetIdPacientaByUzivatel(int idUzivatel)
        {
            using (var conn = _database.GetConnection())
            {
                string sql = "SELECT ID_Pacient FROM PACIENT WHERE UZIVATEL_id_uzivatel = :userid AND ROWNUM = 1";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add("userid", idUzivatel);

                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            return null;
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
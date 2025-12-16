using System;

namespace OrdinaceApp1.Models
{
    public class Uzivatel
    {
        public int IdUzivatel { get; set; }
        public string PrihlasovaciJmeno { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public int RoleId { get; set; }
        public string Aktivni { get; set; }

        public string Telefon { get; set; }

        public string CeleJmeno => $"{Jmeno} {Prijmeni}";
    }
}
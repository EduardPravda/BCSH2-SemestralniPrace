using System;
using System.Data;

namespace OrdinaceApp.Models
{
    public class Uzivatel
    {
        public int IdUzivatel { get; set; }
        public string PrihlasovaciJmeno { get; set; } = string.Empty;
        public string Heslo { get; set; } = string.Empty;
        public string Jmeno { get; set; } = string.Empty;
        public string Prijmeni { get; set; } = string.Empty;
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public DateTime DatumRegistrace { get; set; }
        public string Aktivni { get; set; } = "A"; // A = aktivní, I = neaktivní
    }
}

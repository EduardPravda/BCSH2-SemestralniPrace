using System;
using System.Collections.Generic;

namespace OrdinaceApp.Models
{
    public class Lekar
    {
        public int IdLekar { get; set; }
        public string Jmeno { get; set; } = string.Empty;
        public string Prijmeni { get; set; } = string.Empty;
        public string Specializace { get; set; } = string.Empty; // číselník
        public string? Telefon { get; set; }
        public string? Email { get; set; }

        public ICollection<Rezervace>? Rezervace { get; set; }
        public ICollection<Vysetreni>? Vysetreni { get; set; }
        public ICollection<LekarskyPredpis>? Predpisy { get; set; }
        public ICollection<OrdinacniDoba>? OrdinacniDoby { get; set; }

        public override string ToString() => $"{Jmeno} {Prijmeni} – {Specializace}";
    }
}

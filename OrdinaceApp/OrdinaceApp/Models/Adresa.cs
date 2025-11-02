using System.Collections.Generic;

namespace OrdinaceApp.Models
{
    public class Adresa
    {
        public int IdAdresa { get; set; }
        public string Ulice { get; set; } = string.Empty;
        public int CisloPopisne { get; set; }
        public string Mesto { get; set; } = string.Empty;
        public string PSC { get; set; } = string.Empty;

        public ICollection<Pacient>? Pacienti { get; set; }

        public override string ToString() => $"{Ulice} {CisloPopisne}, {Mesto} {PSC}";
    }
}

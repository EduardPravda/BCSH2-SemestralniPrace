using System;

namespace OrdinaceApp.Models
{
    public class Soubor
    {
        public int IdSoubor { get; set; }
        public string NazevSouboru { get; set; } = string.Empty;
        public string? TypSouboru { get; set; }
        public string? Pripona { get; set; }
        public byte[]? BinarniData { get; set; }
        public DateTime DatumNahrani { get; set; }
        public DateTime? DatumModifikace { get; set; }
        public int UzivatelId { get; set; }
        public string? Popis { get; set; }
    }
}

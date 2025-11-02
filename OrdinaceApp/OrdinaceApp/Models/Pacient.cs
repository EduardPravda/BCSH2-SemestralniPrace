using System;
using System.Collections.Generic;

namespace OrdinaceApp.Models
{
    public class Pacient
    {
        public int IdPacient { get; set; }
        public string Jmeno { get; set; } = string.Empty;
        public string Prijmeni { get; set; } = string.Empty;
        public DateTime DatumNarozeni { get; set; }
        public string? Telefon { get; set; }
        public string? Email { get; set; }

        public int AdresaId { get; set; }
        public Adresa? Adresa { get; set; }

        public ICollection<Rezervace>? Rezervace { get; set; }
        public ICollection<Vysetreni>? Vysetreni { get; set; }
        public ICollection<Alergie>? Alergie { get; set; }
        public ZdravotniKarta? ZdravotniKarta { get; set; }

        public override string ToString() => $"{Jmeno} {Prijmeni} ({DatumNarozeni:dd.MM.yyyy})";
    }
}

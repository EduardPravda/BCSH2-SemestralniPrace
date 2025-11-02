using System;

namespace OrdinaceApp.Models
{
    public class Rezervace
    {
        public int IdRezervace { get; set; }
        public DateTime DatumCas { get; set; }

        public int PacientId { get; set; }
        public Pacient? Pacient { get; set; }

        public int LekarId { get; set; }
        public Lekar? Lekar { get; set; }

        public override string ToString() => $"{DatumCas:dd.MM.yyyy HH:mm}";
    }
}

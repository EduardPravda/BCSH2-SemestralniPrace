using System;

namespace OrdinaceApp.Models
{
    public class Alergie
    {
        public int Id { get; set; }
        public string Nazev { get; set; } = string.Empty;
        public string? Poznamka { get; set; }

        public int PacientId { get; set; }
        public Pacient? Pacient { get; set; }
    }
}

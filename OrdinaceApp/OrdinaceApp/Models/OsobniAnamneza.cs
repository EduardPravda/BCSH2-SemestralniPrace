using System;

namespace OrdinaceApp.Models
{
    public class OsobniAnamneza
    {
        public int Id { get; set; }
        public string? Popis { get; set; }

        public int AnamnezaId { get; set; }
        public Anamneza? Anamneza { get; set; }
    }
}

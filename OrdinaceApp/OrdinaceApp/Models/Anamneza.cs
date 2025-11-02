using System;

namespace OrdinaceApp.Models
{
    public class Anamneza
    {
        public int IdAnamneza { get; set; }
        public DateTime DatumZaznamu { get; set; }
        public string? Poznamky { get; set; }
        public string TypAnamnezy { get; set; } = "O"; // O = osobní, R = rodinná

        public int PacientId { get; set; }
        public Pacient? Pacient { get; set; }

        public int LekarId { get; set; }
        public Lekar? Lekar { get; set; }

        public int DoporuceniId { get; set; }
        public Doporuceni? Doporuceni { get; set; }

        public OsobniAnamneza? OsobniAnamneza { get; set; }
        public RodinnaAnamneza? RodinnaAnamneza { get; set; }
    }
}

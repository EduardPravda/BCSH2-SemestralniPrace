using System;

namespace OrdinaceApp1.Models
{
    public class Anamneza
    {
        public int IdAnamneza { get; set; }
        public int IdDoporuceni { get; set; }
        public DateTime Datum { get; set; }
        public string Typ { get; set; }
        public string Poznamky { get; set; }
        public int IdPacient { get; set; }
        public string PacientJmeno { get; set; }
    }
}
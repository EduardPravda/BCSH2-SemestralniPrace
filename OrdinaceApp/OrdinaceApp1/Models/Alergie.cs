using System;

namespace OrdinaceApp1.Models
{
    public class Alergie
    {
        public int IdAlergie { get; set; }
        public string Nazev { get; set; } 

        public int IdPacient { get; set; }

        public string PacientJmeno { get; set; }
    }
}
using System;

namespace OrdinaceApp1.Models
{
    public class Doporuceni
    {
        public int IdDoporuceni { get; set; }
        public DateTime Datum { get; set; }
        public string Duvod { get; set; }
        // public string OdbornostKam { get; set; } 
        public int IdPacient { get; set; }
        public int IdLekar { get; set; } 

        public string PacientJmeno { get; set; }
        public string LekarJmeno { get; set; }
    }
}
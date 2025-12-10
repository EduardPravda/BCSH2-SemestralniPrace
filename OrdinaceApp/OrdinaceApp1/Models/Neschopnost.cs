using System;

namespace OrdinaceApp1.Models
{
    public class Neschopnost
    {
        public int IdNeschopnost { get; set; }
        public DateTime Zacatek { get; set; }
        public DateTime? Konec { get; set; } 
        public string Duvod { get; set; }
        public string MistoPobytu { get; set; } 

        public int IdPacient { get; set; }
        public int IdLekar { get; set; }
        public string PacientJmeno { get; set; }
        public string LekarJmeno { get; set; }
    }
}
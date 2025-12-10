using System;

namespace OrdinaceApp1.Models
{
    public class Predpis
    {
        public int IdPredpis { get; set; }
        public DateTime DatumVydani { get; set; }
        public string Davkovani { get; set; }
        public string DelkaLecby { get; set; }
        public string Poznamka { get; set; } 

        public int IdPacient { get; set; }
        public int IdLek { get; set; }
        public int IdLekar { get; set; } 

        public string PacientJmeno { get; set; }
        public string LekNazev { get; set; }
        public string LekarJmeno { get; set; }
    }
}
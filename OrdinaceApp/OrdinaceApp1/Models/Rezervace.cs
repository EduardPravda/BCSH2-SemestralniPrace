using System;

namespace OrdinaceApp1.Models
{
    public class Rezervace
    {
        public int IdRezervace { get; set; }
        public DateTime Datum { get; set; }

        public int IdPacient { get; set; }
        public int IdLekar { get; set; }
        public string PacientJmeno { get; set; }
        public string LekarJmeno { get; set; }

        public string DatumFormat => Datum.ToString("dd.MM.yyyy HH:mm");
    }
}
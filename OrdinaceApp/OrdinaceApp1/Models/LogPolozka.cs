using System;

namespace OrdinaceApp1.Models
{
    public class LogPolozka
    {
        public int IdLog { get; set; }
        public string Tabulka { get; set; }
        public string TypOperace { get; set; }
        public string Uzivatel { get; set; }
        public DateTime CasZmeny { get; set; }
        public string StaraHodnota { get; set; }
        public string NovaHodnota { get; set; }
    }
}
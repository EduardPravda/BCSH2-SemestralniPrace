using System;

namespace OrdinaceApp1.Models
{
    public class OrdinacniDoba
    {
        public int IdOrdinacniDoby { get; set; }
        public string Den { get; set; }       
        public DateTime Zacatek { get; set; }
        public DateTime Konec { get; set; }   
        public int IdLekar { get; set; }

        public string LekarJmeno { get; set; }
        public string ZacatekCas => Zacatek.ToString("HH:mm");
        public string KonecCas => Konec.ToString("HH:mm");
    }
}
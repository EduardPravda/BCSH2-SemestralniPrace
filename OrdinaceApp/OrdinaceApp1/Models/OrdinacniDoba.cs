using System;

namespace OrdinaceApp1.Models
{
    public class OrdinacniDoba
    {
        public int IdOrdinacniDoby { get; set; }
        public string Den { get; set; } 
        public string Zacatek { get; set; } 
        public string Konec { get; set; }   

        public int IdLekar { get; set; }
        public string LekarJmeno { get; set; } 
    }
}
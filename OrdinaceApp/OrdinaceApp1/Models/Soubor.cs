using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdinaceApp1.Models
{
    public class Soubor
    {
        public int IdSoubor { get; set; }
        public string Nazev { get; set; }
        public string Typ { get; set; }
        public string Pripona { get; set; }
        public DateTime DatumNahrani { get; set; }
        public string Popis { get; set; }
        public int IdUzivatel { get; set; }
    }
}
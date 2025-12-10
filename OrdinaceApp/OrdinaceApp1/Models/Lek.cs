using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdinaceApp1.Models
{
    public class Lek
    {
        public int IdLek { get; set; }
        public string Nazev { get; set; }
        public string UcinnaLatka { get; set; }
        public string Davkovani { get; set; }    
        public string VedlejsiUcinky { get; set; } 
        public decimal Cena { get; set; }           
    }
}

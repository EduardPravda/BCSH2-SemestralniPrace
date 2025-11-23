using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdinaceApp1.Models
{
    public class Pacient
    {
        // Vlastnosti odpovídají sloupcům v databázi (resp. ve View V_PACIENT_PUBLIC)
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Telefon { get; set; } // Ve view se to jmenuje 'telefon_mask'
        public string Mesto { get; set; }

        // Můžeme přidat pomocnou vlastnost pro zobrazení celého jména
        public string CeleJmeno => $"{Prijmeni} {Jmeno}";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdinaceApp1.Models
{
    public class Pacient
    {
        public int IdPacient { get; set; }  /*K tabulce VYSETRENI */
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Telefon { get; set; }
        public string Mesto { get; set; }

        public string CeleJmeno => $"{Prijmeni} {Jmeno}";
    }
}

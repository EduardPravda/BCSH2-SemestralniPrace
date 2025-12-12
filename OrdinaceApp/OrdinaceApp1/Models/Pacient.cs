using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdinaceApp1.Models
{
    public class Pacient
    {
        public int IdPacient { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Telefon { get; set; }
        public string Mesto { get; set; }
        public string Ulice { get; set; }
        public string Psc { get; set; }
        public DateTime DatumNarozeni { get; set; }
        public string Email { get; set; }
        public int IdUzivatel { get; set; }

        public int Vek { get; set; }
        public string StavNeschopenky { get; set; }

        public string CeleJmeno => $"{Prijmeni} {Jmeno}";
    }
}

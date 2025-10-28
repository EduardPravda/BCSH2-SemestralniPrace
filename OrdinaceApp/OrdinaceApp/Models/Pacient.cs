using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdinaceApp.Models
{
    public class Pacient
    {
        public int IdPacient { get; set; }
        public string Jmeno { get; set; } = string.Empty;
        public string Prijmeni { get; set; } = string.Empty;
        public DateTime DatumNarozeni { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public int IdAdresa { get; set; }

        public override string ToString()
        {
            return $"{Jmeno} {Prijmeni} ({DatumNarozeni:dd.MM.yyyy})";
        }
    }
}

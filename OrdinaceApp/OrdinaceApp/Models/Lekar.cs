using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdinaceApp.Models
{
    public class Lekar
    {
        public int IdLekar { get; set; }
        public string Jmeno { get; set; } = string.Empty;
        public string Prijmeni { get; set; } = string.Empty;
        public string Specializace { get; set; } = string.Empty;
        public string Telefon { get; set; }
        public string Email { get; set; }

        public override string ToString() => $"{Jmeno} {Prijmeni} – {Specializace}";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdinaceApp1.Models
{
    public class Vysetreni
    {
        public int IdVysetreni { get; set; }
        public DateTime Datum { get; set; }
        public string Typ { get; set; }
        public string Poznamka { get; set; }

        public int IdPacient { get; set; }
        public int IdLekar { get; set; }

        public string PacientJmeno { get; set; }
        public string LekarJmeno { get; set; }
    }
}

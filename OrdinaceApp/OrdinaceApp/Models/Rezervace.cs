using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdinaceApp.Models
{
    public class Rezervace
    {
        public int IdRezervace { get; set; }
        public DateTime DatumCas { get; set; }
        public int IdPacient { get; set; }

        public override string ToString() => $"{DatumCas:dd.MM.yyyy HH:mm}";
    }
}

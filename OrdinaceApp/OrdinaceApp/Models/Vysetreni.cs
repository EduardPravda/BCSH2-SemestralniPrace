using System;
using System.Collections.Generic;

namespace OrdinaceApp.Models
{
    public class Vysetreni
    {
        public int IdVysetreni { get; set; }
        public string TypVysetreni { get; set; } = string.Empty;
        public DateTime DatumVysetreni { get; set; }
        public string? Poznamka { get; set; }

        public int PacientId { get; set; }
        public Pacient? Pacient { get; set; }

        public int LekarId { get; set; }
        public Lekar? Lekar { get; set; }

        public ICollection<PracovniNeschopnost>? Neschopnosti { get; set; }
        public ICollection<LekarskyPredpis>? Predpisy { get; set; }
        public ICollection<Anamneza>? Anamnezy { get; set; }
    }
}

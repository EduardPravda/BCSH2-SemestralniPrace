using System;

namespace OrdinaceApp.Models
{
    public class PracovniNeschopnost
    {
        public int Id { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
        public string? Poznamka { get; set; }

        public int VysetreniId { get; set; }
        public Vysetreni? Vysetreni { get; set; }
    }
}

using System;

namespace OrdinaceApp.Models
{
    public class LekarskyPredpis
    {
        public int IdPredpis { get; set; }
        public DateTime DatumVydani { get; set; }
        public string PopisLek { get; set; } = string.Empty;
        public string Davkovani { get; set; } = string.Empty;
        public string? DelkaLecby { get; set; }

        public int LekarId { get; set; }
        public Lekar? Lekar { get; set; }

        public int VysetreniId { get; set; }
        public Vysetreni? Vysetreni { get; set; }
    }
}

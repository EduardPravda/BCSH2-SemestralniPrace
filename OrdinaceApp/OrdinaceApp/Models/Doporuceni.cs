using System;

namespace OrdinaceApp.Models
{
    public class Doporuceni
    {
        public int IdDoporuceni { get; set; }
        public DateTime DatumVydani { get; set; }
        public string DuvodDoporuceni { get; set; } = string.Empty;
    }
}

namespace OrdinaceApp.Models
{
    public class ZdravotniKarta
    {
        public int PacientId { get; set; }
        public string Alergie { get; set; } = string.Empty;
        public string? ChronickaOnemocneni { get; set; }
        public string? Ockovani { get; set; }

        public Pacient? Pacient { get; set; }
    }
}

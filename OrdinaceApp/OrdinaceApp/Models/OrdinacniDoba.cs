using System;

namespace OrdinaceApp.Models
{
    public class OrdinacniDoba
    {
        public int Id { get; set; }
        public DayOfWeek Den { get; set; }
        public TimeSpan CasOd { get; set; }
        public TimeSpan CasDo { get; set; }

        public int LekarId { get; set; }
        public Lekar? Lekar { get; set; }
    }
}

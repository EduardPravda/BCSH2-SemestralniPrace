using System;

namespace OrdinaceApp1.Models
{
    public class Nemoc
    {
        public int IdNemoc { get; set; }
        public string Nazev { get; set; }
        public string Kod { get; set; } // Např. MKN-10 kód (A01.1)
        public string Popis { get; set; }
    }
}
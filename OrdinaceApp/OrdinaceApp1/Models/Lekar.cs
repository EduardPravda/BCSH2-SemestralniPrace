using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdinaceApp1.Models
{
    public class Lekar
    {
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Specializace { get; set; }
        public int Uroven { get; set; }

        public string Odsazeni => new string(' ', Uroven * 4); 
        public string CeleJmenoHierarchie => $"{Odsazeni}└── {Prijmeni} {Jmeno} ({Specializace})";
    }
}
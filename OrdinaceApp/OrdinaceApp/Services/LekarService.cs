using OrdinaceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdinaceApp.Services
{
    public class LekarService
    {
        private readonly List<Lekar> _lekari = new List<Lekar>();

        public IEnumerable<Lekar> GetAll() => _lekari;

        public void Add(Lekar l) => _lekari.Add(l);

        public void Remove(int id) => _lekari.RemoveAll(l => l.IdLekar == id);

        public Lekar? FindById(int id) => _lekari.FirstOrDefault(l => l.IdLekar == id);
    }
}

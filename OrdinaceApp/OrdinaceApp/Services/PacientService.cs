using OrdinaceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdinaceApp.Services
{
    public class PacientService
    {
        private readonly List<Pacient> _pacienti = new List<Pacient>();

        public IEnumerable<Pacient> GetAll() => _pacienti;

        public void Add(Pacient p) => _pacienti.Add(p);

        public void Remove(int id) => _pacienti.RemoveAll(p => p.IdPacient == id);

        public Pacient? FindById(int id) => _pacienti.FirstOrDefault(p => p.IdPacient == id);
    }
}

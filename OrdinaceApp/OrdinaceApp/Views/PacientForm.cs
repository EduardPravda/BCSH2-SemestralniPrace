using OrdinaceApp.Models;
using OrdinaceApp.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OrdinaceApp.Views
{
    public partial class PacientForm : Form
    {
        private readonly PacientService _service = new();
        private ListBox lstPacienti;
        private Button btnPridat;
        private Button btnOdebrat;

        public PacientForm()
        {
            InitializeComponent();
            NactiVzorovaData();
            AktualizujSeznam();
        }

        private void InitializeComponent()
        {
            Text = "Pacienti";
            Size = new Size(400, 300);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;

            lstPacienti = new ListBox { Dock = DockStyle.Top, Height = 180 };
            Controls.Add(lstPacienti);

            btnPridat = new Button { Text = "➕ Přidat pacienta", Dock = DockStyle.Left, Width = 150 };
            btnOdebrat = new Button { Text = "❌ Odebrat", Dock = DockStyle.Right, Width = 150 };
            btnPridat.Click += (s, e) => PridatPacienta();
            btnOdebrat.Click += (s, e) => OdebratPacienta();

            Controls.Add(btnPridat);
            Controls.Add(btnOdebrat);
        }

        private void NactiVzorovaData()
        {
            _service.Add(new Pacient { IdPacient = 1, Jmeno = "Jan", Prijmeni = "Novák", DatumNarozeni = new DateTime(1980, 5, 12) });
            _service.Add(new Pacient { IdPacient = 2, Jmeno = "Petr", Prijmeni = "Horák", DatumNarozeni = new DateTime(1995, 8, 3) });
        }

        private void AktualizujSeznam()
        {
            lstPacienti.DataSource = null;
            lstPacienti.DataSource = _service.GetAll().ToList();
        }

        private void PridatPacienta()
        {
            var novy = new Pacient { IdPacient = _service.GetAll().Count() + 1, Jmeno = "Nový", Prijmeni = "Pacient", DatumNarozeni = DateTime.Now };
            _service.Add(novy);
            AktualizujSeznam();
        }

        private void OdebratPacienta()
        {
            if (lstPacienti.SelectedItem is Pacient vybrany)
            {
                _service.Remove(vybrany.IdPacient);
                AktualizujSeznam();
            }
        }
    }
}

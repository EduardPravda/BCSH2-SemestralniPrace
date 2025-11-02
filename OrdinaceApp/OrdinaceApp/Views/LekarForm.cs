using OrdinaceApp.Models;
using OrdinaceApp.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OrdinaceApp.Views
{
    public partial class LekarForm : Form
    {
        private readonly LekarService _service = new();
        private ListBox lstLekari;
        private Button btnPridat;
        private Button btnOdebrat;

        public LekarForm()
        {
            InitializeComponent();
            NactiVzorovaData();
            AktualizujSeznam();
        }

        private void InitializeComponent()
        {
            Text = "Lékaři";
            Size = new Size(400, 300);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;

            lstLekari = new ListBox { Dock = DockStyle.Top, Height = 180 };
            Controls.Add(lstLekari);

            btnPridat = new Button { Text = "➕ Přidat lékaře", Dock = DockStyle.Left, Width = 150 };
            btnOdebrat = new Button { Text = "❌ Odebrat", Dock = DockStyle.Right, Width = 150 };
            btnPridat.Click += (s, e) => PridatLekare();
            btnOdebrat.Click += (s, e) => OdebratLekare();

            Controls.Add(btnPridat);
            Controls.Add(btnOdebrat);
        }

        private void NactiVzorovaData()
        {
            _service.Add(new Lekar { IdLekar = 1, Jmeno = "MUDr. Eva", Prijmeni = "Černá", Specializace = "Praktický lékař" });
            _service.Add(new Lekar { IdLekar = 2, Jmeno = "MUDr. Karel", Prijmeni = "Bílý", Specializace = "Internista" });
        }

        private void AktualizujSeznam()
        {
            lstLekari.DataSource = null;
            lstLekari.DataSource = _service.GetAll().ToList();
        }

        private void PridatLekare()
        {
            var novy = new Lekar { IdLekar = _service.GetAll().Count() + 1, Jmeno = "Nový", Prijmeni = "Lékař", Specializace = "Obecný" };
            _service.Add(novy);
            AktualizujSeznam();
        }

        private void OdebratLekare()
        {
            if (lstLekari.SelectedItem is Lekar vybrany)
            {
                _service.Remove(vybrany.IdLekar);
                AktualizujSeznam();
            }
        }
    }
}

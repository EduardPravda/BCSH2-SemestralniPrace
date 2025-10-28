using OrdinaceApp.Models;
using OrdinaceApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;

namespace OrdinaceApp.Views
{
    public partial class MainForm : Form
    {
        private readonly PacientService _pacientService = new();
        private readonly LekarService _lekarService = new();

        public MainForm()
        {
            InitializeComponent();
            Text = "Informační systém ordinace praktického lékaře";

            // demo data
            _pacientService.Add(new Pacient { IdPacient = 1, Jmeno = "Jan", Prijmeni = "Novák", DatumNarozeni = new DateTime(1985, 5, 10) });
            _lekarService.Add(new Lekar { IdLekar = 1, Jmeno = "Petr", Prijmeni = "Horák", Specializace = "Praktický lékař" });

            // zobraz základní info
            lblInfo.Text = $"Načteno: {_pacientService.GetAll().Count()} pacient(ů), {_lekarService.GetAll().Count()} lékař(ů)";
        }
    }
}

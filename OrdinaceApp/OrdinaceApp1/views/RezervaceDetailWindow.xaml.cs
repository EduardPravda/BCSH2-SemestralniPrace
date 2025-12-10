using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class RezervaceDetailWindow : Window
    {
        public RezervaceDetailWindow()
        {
            InitializeComponent();
            DpDatum.SelectedDate = DateTime.Now.AddDays(1); 
            NacistCiselniky();
        }

        private void NacistCiselniky()
        {
            var repoP = new PacientRepository();
            CmbPacient.ItemsSource = repoP.GetPacientiProCombo();

            var repoL = new LekarRepository();
            CmbLekar.ItemsSource = repoL.GetVsechnyLekare();
        }

        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            if (CmbPacient.SelectedValue == null || CmbLekar.SelectedValue == null || DpDatum.SelectedDate == null)
            {
                MessageBox.Show("Vyplňte všechny údaje.");
                return;
            }

            try
            {
                DateTime datum = DpDatum.SelectedDate.Value;
                if (TimeSpan.TryParse(TxtCas.Text, out TimeSpan cas))
                {
                    datum = datum.Add(cas);
                }
                else
                {
                    MessageBox.Show("Špatný formát času (použijte např. 14:30).");
                    return;
                }

                var r = new Rezervace
                {
                    IdPacient = (int)CmbPacient.SelectedValue,
                    IdLekar = (int)CmbLekar.SelectedValue,
                    Datum = datum
                };

                var repo = new RezervaceRepository();
                repo.PridatRezervaci(r);

                MessageBox.Show("Rezervace uložena.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }
    }
}
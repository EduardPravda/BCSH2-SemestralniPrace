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

            NacistSeznamy();
        }

        private void NacistSeznamy()
        {
            try
            {
                var pacRepo = new PacientRepository();
                CmbPacient.ItemsSource = pacRepo.GetPacientiProCombo();

                // 1. NAČTENÍ LÉKAŘŮ DO COMBOBOXU
                // (Pokud nemáš LekarRepository, použij UzivatelRepository nebo jiný existující)
                var lekRepo = new LekarRepository();
                CmbLekar.ItemsSource = lekRepo.GetVsechnyLekare();
            }
            catch { }
        }

        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            // 2. KONTROLA, ZDA JE VYBRÁN I LÉKAŘ
            if (CmbPacient.SelectedValue == null || CmbLekar.SelectedValue == null || DpDatum.SelectedDate == null)
            {
                MessageBox.Show("Vyberte pacienta, lékaře a datum.");
                return;
            }

            try
            {
                DateTime datum = DpDatum.SelectedDate.Value;
                if (TimeSpan.TryParse(TxtCas.Text, out TimeSpan cas))
                {
                    datum = datum.Add(cas);
                }

                var r = new Rezervace
                {
                    IdPacient = (int)CmbPacient.SelectedValue,
                    IdLekar = (int)CmbLekar.SelectedValue, // 3. ULOŽENÍ ID LÉKAŘE
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
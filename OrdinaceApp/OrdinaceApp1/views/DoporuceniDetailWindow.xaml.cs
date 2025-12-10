using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class DoporuceniDetailWindow : Window
    {
        public DoporuceniDetailWindow()
        {
            InitializeComponent();
            DpDatum.SelectedDate = DateTime.Now;
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
            if (CmbPacient.SelectedValue == null || CmbLekar.SelectedValue == null)
            {
                MessageBox.Show("Vyberte pacienta a lékaře.");
                return;
            }

            try
            {
                var d = new Doporuceni
                {
                    IdPacient = (int)CmbPacient.SelectedValue,
                    IdLekar = (int)CmbLekar.SelectedValue,
                    Datum = DpDatum.SelectedDate ?? DateTime.Now,
                    Duvod = TxtDuvod.Text
                };

                var repo = new DoporuceniRepository();
                repo.VystavitDoporuceni(d);

                MessageBox.Show("Doporučení vystaveno.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }
    }
}
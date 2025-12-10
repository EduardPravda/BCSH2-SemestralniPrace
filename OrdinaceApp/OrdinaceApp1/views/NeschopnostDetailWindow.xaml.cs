using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class NeschopnostDetailWindow : Window
    {
        public NeschopnostDetailWindow()
        {
            InitializeComponent();
            DpZacatek.SelectedDate = DateTime.Now;
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
                var n = new Neschopnost
                {
                    IdPacient = (int)CmbPacient.SelectedValue,
                    IdLekar = (int)CmbLekar.SelectedValue,
                    Zacatek = DpZacatek.SelectedDate ?? DateTime.Now,
                    Konec = DpKonec.SelectedDate,
                    Duvod = TxtDuvod.Text
                };

                var repo = new NeschopnostRepository();
                repo.VystavitNeschopnost(n);

                MessageBox.Show("Neschopenka vystavena.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }
    }
}
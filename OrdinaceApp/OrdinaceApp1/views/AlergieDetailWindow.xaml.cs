using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class AlergieDetailWindow : Window
    {
        public AlergieDetailWindow()
        {
            InitializeComponent();
            NacistPacienty();
        }

        private void NacistPacienty()
        {
            var repo = new PacientRepository();
            CmbPacient.ItemsSource = repo.GetPacientiProCombo();
        }

        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            if (CmbPacient.SelectedValue == null || string.IsNullOrWhiteSpace(TxtNazev.Text))
            {
                MessageBox.Show("Vyberte pacienta a zadejte název alergie.");
                return;
            }

            try
            {
                var a = new Alergie
                {
                    IdPacient = (int)CmbPacient.SelectedValue,
                    Nazev = TxtNazev.Text
                };

                var repo = new AlergieRepository();
                repo.PridatAlergii(a);

                MessageBox.Show("Uloženo.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }
    }
}
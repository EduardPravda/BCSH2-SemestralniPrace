using System;
using System.Windows;
using System.Windows.Controls;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class AnamnezaDetailWindow : Window
    {
        public AnamnezaDetailWindow()
        {
            InitializeComponent();
            DpDatum.SelectedDate = DateTime.Now;
            NacistPacienty();
        }

        private void NacistPacienty()
        {
            var repo = new PacientRepository();
            CmbPacient.ItemsSource = repo.GetPacientiProCombo();
        }

        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            if (CmbPacient.SelectedValue == null || cmbTyp.SelectedItem == null)
            {
                MessageBox.Show("Vyberte pacienta a typ anamnézy.");
                return;
            }

            try
            {
                var typItem = (ComboBoxItem)cmbTyp.SelectedItem;

                var a = new Anamneza
                {
                    IdPacient = (int)CmbPacient.SelectedValue,
                    Datum = DpDatum.SelectedDate ?? DateTime.Now,
                    Typ = typItem.Content.ToString(),
                    Poznamky = TxtPoznamky.Text
                };

                var repo = new AnamnezaRepository();
                repo.PridatAnamnezu(a);

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
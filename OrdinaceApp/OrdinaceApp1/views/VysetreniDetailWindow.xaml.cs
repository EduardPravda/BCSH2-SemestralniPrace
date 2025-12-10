using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class VysetreniDetailWindow : Window
    {
        public VysetreniDetailWindow()
        {
            InitializeComponent();

            DpDatum.SelectedDate = DateTime.Now;

            NacistCiselniky();
        }

        private void NacistCiselniky()
        {
            try
            {
                var repoPacient = new PacientRepository();
                CmbPacient.ItemsSource = repoPacient.GetPacientiProCombo();

                var repoLekar = new LekarRepository();
                CmbLekar.ItemsSource = repoLekar.GetVsechnyLekare();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání seznamů: " + ex.Message);
            }
        }

        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            if (CmbPacient.SelectedValue == null || CmbLekar.SelectedValue == null)
            {
                MessageBox.Show("Musíte vybrat Pacienta i Lékaře!");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtTyp.Text))
            {
                MessageBox.Show("Zadejte typ vyšetření.");
                return;
            }

            try
            {
                var v = new Vysetreni
                {
                    IdPacient = (int)CmbPacient.SelectedValue,
                    IdLekar = (int)CmbLekar.SelectedValue,
                    Datum = DpDatum.SelectedDate ?? DateTime.Now,
                    Typ = TxtTyp.Text,
                    Poznamka = TxtPoznamka.Text
                };

                var repo = new VysetreniRepository();
                repo.PridatVysetreni(v);

                MessageBox.Show("Vyšetření uloženo.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při ukládání: " + ex.Message);
            }
        }

        private void BtnZrusit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
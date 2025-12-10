using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class PredpisDetailWindow : Window
    {
        public PredpisDetailWindow()
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

                var repoLek = new LekRepository();
                CmbLek.ItemsSource = repoLek.GetVsechnyLeky();

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
            if (CmbPacient.SelectedValue == null || CmbLek.SelectedValue == null || CmbLekar.SelectedValue == null)
            {
                MessageBox.Show("Musíte vybrat Pacienta, Lék i Lékaře!");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtDavkovani.Text))
            {
                MessageBox.Show("Vyplňte prosím dávkování.");
                return;
            }

            try
            {
                var p = new Predpis
                {
                    IdPacient = (int)CmbPacient.SelectedValue,
                    IdLek = (int)CmbLek.SelectedValue,
                    IdLekar = (int)CmbLekar.SelectedValue,

                    DatumVydani = DpDatum.SelectedDate ?? DateTime.Now,
                    Davkovani = TxtDavkovani.Text,
                    DelkaLecby = TxtDelka.Text,
                    Poznamka = TxtPoznamka.Text
                };

                var repo = new PredpisRepository();
                repo.PridatPredpis(p);

                MessageBox.Show("Recept byl úspěšně vydán.");
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
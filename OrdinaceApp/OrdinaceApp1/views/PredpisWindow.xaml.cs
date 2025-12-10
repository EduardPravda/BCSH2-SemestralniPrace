using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class PredpisyWindow : Window
    {
        public PredpisyWindow()
        {
            InitializeComponent();
            NacistData();
        }

        private void NacistData()
        {
            try
            {
                var repo = new PredpisRepository();
                DgPredpisy.ItemsSource = repo.GetVsechnyPredpisy();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }

        private void BtnPridat_Click(object sender, RoutedEventArgs e)
        {
            var okno = new PredpisDetailWindow();
            okno.ShowDialog();
            NacistData();
        }

        private void BtnSmazat_Click(object sender, RoutedEventArgs e)
        {
            if (DgPredpisy.SelectedItem is Predpis p)
            {
                if (MessageBox.Show("Opravdu smazat tento recept?", "Dotaz", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var repo = new PredpisRepository();
                        repo.SmazatPredpis(p.IdPredpis);
                        NacistData();
                    }
                    catch (Exception ex) { MessageBox.Show("Chyba: " + ex.Message); }
                }
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
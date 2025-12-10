using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class LekyWindow : Window
    {
        public LekyWindow()
        {
            InitializeComponent();
            NacistData();
        }

        private void NacistData()
        {
            try
            {
                var repo = new LekRepository();
                DgLeky.ItemsSource = repo.GetVsechnyLeky();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání: " + ex.Message);
            }
        }

        private void BtnPridat_Click(object sender, RoutedEventArgs e)
        {
            var okno = new LekDetailWindow();
            okno.ShowDialog();
            NacistData(); 
        }

        private void BtnUpravit_Click(object sender, RoutedEventArgs e)
        {
            if (DgLeky.SelectedItem is Lek vybranyLek)
            {
                var okno = new LekDetailWindow(vybranyLek);
                okno.ShowDialog();
                NacistData();
            }
            else
            {
                MessageBox.Show("Vyberte lék k úpravě.");
            }
        }

        private void BtnSmazat_Click(object sender, RoutedEventArgs e)
        {
            if (DgLeky.SelectedItem is Lek vybranyLek)
            {
                var result = MessageBox.Show($"Opravdu chcete smazat lék '{vybranyLek.Nazev}'?", "Potvrzení", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var repo = new LekRepository();
                        repo.SmazatLek(vybranyLek.IdLek);
                        NacistData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Nelze smazat (lék je pravděpodobně používán v receptech).\n\nDetaily: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vyberte lék ke smazání.");
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
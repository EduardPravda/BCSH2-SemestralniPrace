using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class SpravaLekaruWindow : Window
    {
        public SpravaLekaruWindow()
        {
            InitializeComponent();
            NacistData();
        }

        private void NacistData()
        {
            try
            {
                var repo = new LekarRepository();
                DgLekari.ItemsSource = repo.GetVsechnyLekare();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }

        private void BtnPridat_Click(object sender, RoutedEventArgs e)
        {
            var okno = new LekarDetailWindow(); 
            okno.ShowDialog();
            NacistData();
        }

        private void BtnUpravit_Click(object sender, RoutedEventArgs e)
        {
            if (DgLekari.SelectedItem is Lekar vybrany)
            {
                var okno = new LekarDetailWindow(vybrany);
                okno.ShowDialog();
                NacistData();
            }
            else MessageBox.Show("Vyberte lékaře.");
        }

        private void BtnSmazat_Click(object sender, RoutedEventArgs e)
        {
            if (DgLekari.SelectedItem is Lekar vybrany)
            {
                if (MessageBox.Show($"Smazat lékaře {vybrany.Prijmeni}?", "Dotaz", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var repo = new LekarRepository();
                        repo.SmazatLekare(vybrany.IdLekar);
                        NacistData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Nelze smazat (lékař má asi pacienty/vyšetření).\n" + ex.Message);
                    }
                }
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
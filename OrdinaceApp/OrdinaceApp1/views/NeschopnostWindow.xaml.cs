using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class NeschopnostWindow : Window
    {
        public NeschopnostWindow()
        {
            InitializeComponent();
            NacistData();
        }

        private void NacistData()
        {
            try
            {
                var repo = new NeschopnostRepository();
                DgNeschopnosti.ItemsSource = repo.GetVsechnyNeschopnosti();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }

        private void BtnPridat_Click(object sender, RoutedEventArgs e)
        {
            var okno = new NeschopnostDetailWindow();
            okno.ShowDialog();
            NacistData();
        }

        private void BtnSmazat_Click(object sender, RoutedEventArgs e)
        {
            if (DgNeschopnosti.SelectedItem is Neschopnost n)
            {
                if (MessageBox.Show("Smazat neschopenku?", "Dotaz", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var repo = new NeschopnostRepository();
                        repo.SmazatNeschopnost(n.IdNeschopnost);
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
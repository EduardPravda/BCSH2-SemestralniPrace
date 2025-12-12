using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class OrdinacniDobaWindow : Window
    {
        private int _idRole;

        public OrdinacniDobaWindow(int idRole)
        {
            InitializeComponent();
            _idRole = idRole;

            NastavitOpravneni();
            NacistData();
        }

        private void NacistData()
        {
            try
            {
                var repo = new OrdinacniDobaRepository();
                DgDoby.ItemsSource = repo.GetVsechnyCasy();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }

        private void BtnPridat_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinacniDobaDetailWindow();
            okno.ShowDialog();
            NacistData();
        }

        private void BtnSmazat_Click(object sender, RoutedEventArgs e)
        {
            if (DgDoby.SelectedItem is OrdinacniDoba o)
            {
                if (MessageBox.Show("Smazat tento čas?", "Dotaz", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var repo = new OrdinacniDobaRepository();
                        repo.SmazatDobu(o.IdOrdinacniDoby);
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

        private void NastavitOpravneni()
        {
            // Povolení úprav pouze pro Admina (1) a Lékaře (2).
            // Host/Pacient (Role ID 3, 4, atd.) úpravy nevidí.
            if (_idRole > 2)
            {
                if (BtnPridatCas != null) BtnPridatCas.Visibility = Visibility.Collapsed;
                if (BtnSmazatZaznam != null) BtnSmazatZaznam.Visibility = Visibility.Collapsed;
            }
        }

    }
}
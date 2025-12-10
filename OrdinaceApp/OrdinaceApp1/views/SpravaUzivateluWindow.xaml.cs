using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class SpravaUzivateluWindow : Window
    {
        public SpravaUzivateluWindow()
        {
            InitializeComponent();
            NacistData();
        }

        private void NacistData()
        {
            try
            {
                var repo = new UzivatelRepository();
                DgUzivatele.ItemsSource = repo.GetVsechnyUzivatele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }

        private void BtnPridat_Click(object sender, RoutedEventArgs e)
        {
            var okno = new UzivatelDetailWindow();
            okno.ShowDialog();
            NacistData();
        }

        private void BtnUpravit_Click(object sender, RoutedEventArgs e)
        {
            if (DgUzivatele.SelectedItem is Uzivatel u)
            {
                var okno = new UzivatelDetailWindow(u);
                okno.ShowDialog();
                NacistData();
            }
            else MessageBox.Show("Vyberte uživatele.");
        }

        private void BtnSmazat_Click(object sender, RoutedEventArgs e)
        {
            if (DgUzivatele.SelectedItem is Uzivatel u)
            {
                if (MessageBox.Show($"Smazat uživatele {u.PrihlasovaciJmeno}?", "Pozor", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var repo = new UzivatelRepository();
                        repo.SmazatUzivatele(u.IdUzivatel);
                        NacistData();
                    }
                    catch (Exception ex) { MessageBox.Show("Nelze smazat (uživatel je asi navázán na lékaře/pacienta).\n" + ex.Message); }
                }
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
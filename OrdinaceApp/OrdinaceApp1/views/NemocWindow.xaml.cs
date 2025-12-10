using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class NemocWindow : Window
    {
        public NemocWindow()
        {
            InitializeComponent();
            NacistData();
        }

        private void NacistData()
        {
            try
            {
                var repo = new NemocRepository();
                DgNemoci.ItemsSource = repo.GetVsechnyNemoci();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }

        private void BtnPridat_Click(object sender, RoutedEventArgs e)
        {
            var okno = new NemocDetailWindow();
            okno.ShowDialog();
            NacistData();
        }

        private void BtnUpravit_Click(object sender, RoutedEventArgs e)
        {
            if (DgNemoci.SelectedItem is Nemoc n)
            {
                var okno = new NemocDetailWindow(n);
                okno.ShowDialog();
                NacistData();
            }
        }

        private void BtnSmazat_Click(object sender, RoutedEventArgs e)
        {
            if (DgNemoci.SelectedItem is Nemoc n)
            {
                if (MessageBox.Show("Smazat diagnózu?", "Dotaz", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var repo = new NemocRepository();
                        repo.SmazatNemoc(n.IdNemoc);
                        NacistData();
                    }
                    catch (Exception ex) { MessageBox.Show("Nelze smazat.\n" + ex.Message); }
                }
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
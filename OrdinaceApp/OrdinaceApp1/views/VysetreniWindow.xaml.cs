using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class VysetreniWindow : Window
    {
        public VysetreniWindow()
        {
            InitializeComponent();
            NacistData();
        }

        private void NacistData()
        {
            try
            {
                var repo = new VysetreniRepository();
                DgVysetreni.ItemsSource = repo.GetVsechnaVysetreni();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }

        private void BtnPridat_Click(object sender, RoutedEventArgs e)
        {
            var okno = new VysetreniDetailWindow();
            okno.ShowDialog();
            NacistData();
        }

        private void BtnSmazat_Click(object sender, RoutedEventArgs e)
        {
            if (DgVysetreni.SelectedItem is Vysetreni v)
            {
                if (MessageBox.Show("Opravdu smazat?", "Potvrzení", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var repo = new VysetreniRepository();
                        repo.SmazatVysetreni(v.IdVysetreni);
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
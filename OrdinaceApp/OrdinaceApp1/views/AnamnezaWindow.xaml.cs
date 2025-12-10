using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class AnamnezaWindow : Window
    {
        public AnamnezaWindow()
        {
            InitializeComponent();
            NacistData();
        }

        private void NacistData()
        {
            try
            {
                var repo = new AnamnezaRepository();
                DgAnamneza.ItemsSource = repo.GetVsechnyAnamnezy();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }

        private void BtnPridat_Click(object sender, RoutedEventArgs e)
        {
            var okno = new AnamnezaDetailWindow();
            okno.ShowDialog();
            NacistData();
        }

        private void BtnSmazat_Click(object sender, RoutedEventArgs e)
        {
            if (DgAnamneza.SelectedItem is Anamneza a)
            {
                if (MessageBox.Show("Smazat tento záznam?", "Dotaz", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var repo = new AnamnezaRepository();
                        repo.SmazatAnamnezu(a.IdAnamneza);
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
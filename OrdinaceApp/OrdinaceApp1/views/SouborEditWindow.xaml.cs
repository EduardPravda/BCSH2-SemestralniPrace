using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class SouborEditWindow : Window
    {
        private int _idSoubor;

        public SouborEditWindow(Soubor s)
        {
            InitializeComponent();
            _idSoubor = s.IdSoubor;
            TxtNazev.Text = s.Nazev;
            TxtPopis.Text = s.Popis;
        }

        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNazev.Text))
            {
                MessageBox.Show("Název souboru nesmí být prázdný.");
                return;
            }

            try
            {
                var repo = new SouborRepository();
                repo.UpravitSoubor(_idSoubor, TxtNazev.Text, TxtPopis.Text);

                MessageBox.Show("Soubor byl aktualizován.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }
    }
}
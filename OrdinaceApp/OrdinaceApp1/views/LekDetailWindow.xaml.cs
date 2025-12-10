using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class LekDetailWindow : Window
    {
        private int _idLeku = 0; 
        public LekDetailWindow()
        {
            InitializeComponent();
            TxtNazev.Focus();
        }

        public LekDetailWindow(Lek existujiciLek)
        {
            InitializeComponent();

            _idLeku = existujiciLek.IdLek;
            TxtNazev.Text = existujiciLek.Nazev;
            TxtUcinnaLatka.Text = existujiciLek.UcinnaLatka;
            TxtCena.Text = existujiciLek.Cena.ToString();
            TxtDavkovani.Text = existujiciLek.Davkovani;
            TxtUcinky.Text = existujiciLek.VedlejsiUcinky;

            this.Title = $"Editace léku: {existujiciLek.Nazev}";
        }

        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNazev.Text) || string.IsNullOrWhiteSpace(TxtCena.Text))
            {
                MessageBox.Show("Název a cena jsou povinné údaje.");
                return;
            }

            if (!decimal.TryParse(TxtCena.Text, out decimal cena))
            {
                MessageBox.Show("Cena musí být číslo (např. 150 nebo 150,50).");
                return;
            }

            try
            {
                var lek = new Lek
                {
                    IdLek = _idLeku, 
                    Nazev = TxtNazev.Text,
                    UcinnaLatka = TxtUcinnaLatka.Text,
                    Cena = cena,
                    Davkovani = TxtDavkovani.Text,
                    VedlejsiUcinky = TxtUcinky.Text
                };

                var repo = new LekRepository();

                if (_idLeku == 0)
                {
                    repo.PridatLek(lek);
                }
                else
                {
                    repo.UpravitLek(lek);
                }

                MessageBox.Show("Uloženo.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při ukládání: " + ex.Message);
            }
        }

        private void BtnStorno_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
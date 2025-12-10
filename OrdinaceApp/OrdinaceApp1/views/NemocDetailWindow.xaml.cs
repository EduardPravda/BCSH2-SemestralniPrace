using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class NemocDetailWindow : Window
    {
        private int _id = 0;

        public NemocDetailWindow()
        {
            InitializeComponent();
        }

        public NemocDetailWindow(Nemoc n)
        {
            InitializeComponent();
            _id = n.IdNemoc;
            TxtNazev.Text = n.Nazev;
            TxtPopis.Text = n.Popis;
        }

        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNazev.Text))
            {
                MessageBox.Show("Vyplňte název.");
                return;
            }

            try
            {
                var n = new Nemoc
                {
                    IdNemoc = _id,
                    Nazev = TxtNazev.Text,
                    Popis = TxtPopis.Text
                };

                var repo = new NemocRepository();
                if (_id == 0) repo.PridatNemoc(n);
                else repo.UpravitNemoc(n);

                MessageBox.Show("Uloženo.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }
    }
}
using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class LekarDetailWindow : Window
    {
        private int _idLekar = 0;

        public LekarDetailWindow()
        {
            InitializeComponent();
            NacistUzivatele();
        }

        public LekarDetailWindow(Lekar l)
        {
            InitializeComponent();
            NacistUzivatele();

            _idLekar = l.IdLekar;
            TxtJmeno.Text = l.Jmeno;
            TxtPrijmeni.Text = l.Prijmeni;
            TxtSpec.Text = l.Specializace;
            TxtTel.Text = l.Telefon;
            TxtEmail.Text = l.Email;

            CmbUzivatel.SelectedValue = l.IdUzivatel;
            CmbUzivatel.IsEnabled = false; 
        }

        private void NacistUzivatele()
        {
            var repo = new UzivatelRepository();
            CmbUzivatel.ItemsSource = repo.GetVsechnyUzivatele();
        }

        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            if (CmbUzivatel.SelectedValue == null)
            {
                MessageBox.Show("Musíte vybrat uživatele!");
                return;
            }

            try
            {
                var l = new Lekar
                {
                    IdLekar = _idLekar,
                    Jmeno = TxtJmeno.Text,
                    Prijmeni = TxtPrijmeni.Text,
                    Specializace = TxtSpec.Text,
                    Telefon = TxtTel.Text,
                    Email = TxtEmail.Text,
                    IdUzivatel = (int)CmbUzivatel.SelectedValue
                };

                var repo = new LekarRepository();
                if (_idLekar == 0) repo.PridatLekare(l);
                else repo.UpravitLekare(l);

                MessageBox.Show("Uloženo.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba (asi je tento uživatel už zabraný):\n" + ex.Message);
            }
        }
    }
}
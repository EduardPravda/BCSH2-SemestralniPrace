using System;
using System.Windows;
using System.Windows.Controls;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class UzivatelDetailWindow : Window
    {
        private int _idUzivatel = 0;
        private bool _isRegistrationMode = false;

        public UzivatelDetailWindow()
        {
            InitializeComponent();
        }

        public UzivatelDetailWindow(bool isRegistration) : this()
        {
            _isRegistrationMode = isRegistration;

            if (_isRegistrationMode)
            {
                this.Title = "Registrace nového pacienta";
                // Skryjeme prvky, které si běžný uživatel nemůže nastavit
                CmbRole.Visibility = Visibility.Collapsed;
                ChkAktivni.Visibility = Visibility.Collapsed;

                // Přednastavíme, že je účet aktivní
                ChkAktivni.IsChecked = true;
            }
        }

        public UzivatelDetailWindow(Uzivatel u)
        {
            InitializeComponent();
            _idUzivatel = u.IdUzivatel;
            TxtLogin.Text = u.PrihlasovaciJmeno;
            TxtJmeno.Text = u.Jmeno;
            TxtPrijmeni.Text = u.Prijmeni;

            TxtTelefon.Text = u.Telefon;

            ChkAktivni.IsChecked = (u.Aktivni == "A");

            foreach (ComboBoxItem item in CmbRole.Items)
            {
                if (item.Tag.ToString() == u.RoleId.ToString())
                {
                    CmbRole.SelectedItem = item;
                    break;
                }
            }
        }

        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validace Role (pokud nejsme v registraci)
            if (!_isRegistrationMode && CmbRole.SelectedItem == null)
            {
                MessageBox.Show("Vyberte roli!");
                return;
            }

            // 2. Získání a kontrola hesel (z PasswordBoxů)
            string noveHeslo = PbHeslo.Password;
            string kontrolaHesla = PbHesloKontrola.Password;

            if (_idUzivatel == 0 && string.IsNullOrWhiteSpace(noveHeslo))
            {
                MessageBox.Show("Musíte zadat heslo!");
                return;
            }

            if (!string.IsNullOrEmpty(noveHeslo))
            {
                if (noveHeslo != kontrolaHesla)
                {
                    MessageBox.Show("Hesla se neshodují! Zadejte je znovu.");
                    PbHeslo.Clear();
                    PbHesloKontrola.Clear();
                    return;
                }
            }

            try
            {
                int roleId;
                string aktivni;

                if (_isRegistrationMode)
                {
                    roleId = 3; // Pacient
                    aktivni = "A";
                }
                else
                {
                    var roleItem = (ComboBoxItem)CmbRole.SelectedItem;
                    roleId = int.Parse(roleItem.Tag.ToString());
                    aktivni = (ChkAktivni.IsChecked == true) ? "A" : "N";
                }

                var u = new Uzivatel
                {
                    IdUzivatel = _idUzivatel,
                    PrihlasovaciJmeno = TxtLogin.Text,
                    Jmeno = TxtJmeno.Text,
                    Prijmeni = TxtPrijmeni.Text,
                    RoleId = roleId,
                    Aktivni = aktivni,
                    Telefon = TxtTelefon.Text
                };

                var repo = new UzivatelRepository();

                if (_idUzivatel == 0)
                {
                    repo.PridatUzivatele(u, noveHeslo);
                }
                else
                {
                    repo.UpravitUzivatele(u, noveHeslo);
                }

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
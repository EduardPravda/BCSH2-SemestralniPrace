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
            ChkAktivni.IsChecked = (u.Aktivni == "A");

            foreach (ComboBoxItem item in CmbRole.Items)
            {
                if (item.Tag.ToString() == u.RoleId.ToString())
                {
                    CmbRole.SelectedItem = item;
                    break;
                }
            }

            TxtHeslo.ToolTip = "Zadejte nové heslo jen pokud ho chcete změnit.";
        }

        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            // Kontrola role jen pokud NEJSME v režimu registrace
            if (!_isRegistrationMode && CmbRole.SelectedItem == null)
            {
                MessageBox.Show("Vyberte roli!");
                return;
            }

            try
            {
                int roleId;
                string aktivni;

                if (_isRegistrationMode)
                {
                    // Při registraci je to vždy Pacient (ID 3) a Aktivní (A)
                    roleId = 3;
                    aktivni = "A";
                }
                else
                {
                    // Při editaci adminem bereme hodnoty z formuláře
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
                    Aktivni = aktivni
                };

                var repo = new UzivatelRepository();

                if (_idUzivatel == 0) // Nový záznam
                {
                    if (string.IsNullOrWhiteSpace(TxtHeslo.Text))
                    {
                        MessageBox.Show("Musíte zadat heslo!");
                        return;
                    }
                    repo.PridatUzivatele(u, TxtHeslo.Text);
                }
                else // Editace
                {
                    repo.UpravitUzivatele(u, TxtHeslo.Text);
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
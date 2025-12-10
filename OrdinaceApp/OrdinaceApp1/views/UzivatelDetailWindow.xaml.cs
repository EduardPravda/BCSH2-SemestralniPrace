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

        public UzivatelDetailWindow()
        {
            InitializeComponent();
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
            if (CmbRole.SelectedItem == null)
            {
                MessageBox.Show("Vyberte roli!");
                return;
            }

            try
            {
                var roleItem = (ComboBoxItem)CmbRole.SelectedItem;
                int roleId = int.Parse(roleItem.Tag.ToString());

                var u = new Uzivatel
                {
                    IdUzivatel = _idUzivatel,
                    PrihlasovaciJmeno = TxtLogin.Text,
                    Jmeno = TxtJmeno.Text,
                    Prijmeni = TxtPrijmeni.Text,
                    RoleId = roleId,
                    Aktivni = (ChkAktivni.IsChecked == true) ? "A" : "N"
                };

                var repo = new UzivatelRepository();

                if (_idUzivatel == 0)
                {
                    if (string.IsNullOrWhiteSpace(TxtHeslo.Text))
                    {
                        MessageBox.Show("U nového uživatele musíte zadat heslo!");
                        return;
                    }
                    repo.PridatUzivatele(u, TxtHeslo.Text);
                }
                else
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
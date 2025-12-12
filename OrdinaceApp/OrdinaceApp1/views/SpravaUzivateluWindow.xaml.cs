using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class SpravaUzivateluWindow : Window
    {
        public SpravaUzivateluWindow()
        {
            InitializeComponent();
            NacistData();
        }

        private void NacistData()
        {
            try
            {
                var repo = new UzivatelRepository();
                DgUzivatele.ItemsSource = repo.GetVsechnyUzivatele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }

        private void BtnPridat_Click(object sender, RoutedEventArgs e)
        {
            var okno = new UzivatelDetailWindow();
            okno.ShowDialog();
            NacistData();
        }

        private void BtnUpravit_Click(object sender, RoutedEventArgs e)
        {
            if (DgUzivatele.SelectedItem is Uzivatel u)
            {
                var okno = new UzivatelDetailWindow(u);
                okno.ShowDialog();
                NacistData();
            }
            else MessageBox.Show("Vyberte uživatele.");
        }

        private void BtnEmulovat_Click(object sender, RoutedEventArgs e)
        {
            if (DgUzivatele.SelectedItem is Uzivatel vybrany)
            {
                var result = MessageBox.Show($"Chcete se přihlásit do systému jako uživatel '{vybrany.PrihlasovaciJmeno}'?",
                                             "Emulace uživatele", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // 1. Vytvoříme nové hlavní okno s identitou vybraného uživatele
                    var newMainWindow = new OrdinaceApp1.MainWindow(vybrany);
                    newMainWindow.Show();

                    // 2. Zavřeme aktuální okno správy
                    this.Close();

                    // 3. Zavřeme původní hlavní okno (musíme ho najít v aplikaci)
                    foreach (Window w in Application.Current.Windows)
                    {
                        // Zavřeme všechna okna kromě toho nového, co jsme právě otevřeli
                        if (w != newMainWindow)
                        {
                            w.Close();
                        }
                    }

                    // Nastavíme nové okno jako hlavní
                    Application.Current.MainWindow = newMainWindow;
                }
            }
            else
            {
                MessageBox.Show("Vyberte uživatele ze seznamu.");
            }
        }

        private void BtnSmazat_Click(object sender, RoutedEventArgs e)
        {
            if (DgUzivatele.SelectedItem is Uzivatel u)
            {
                if (MessageBox.Show($"Smazat uživatele {u.PrihlasovaciJmeno}?", "Pozor", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var repo = new UzivatelRepository();
                        repo.SmazatUzivatele(u.IdUzivatel);
                        NacistData();
                    }
                    catch (Exception ex) { MessageBox.Show("Nelze smazat (uživatel je asi navázán na lékaře/pacienta).\n" + ex.Message); }
                }
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
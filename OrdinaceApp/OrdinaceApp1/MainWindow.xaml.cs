using System;
using System.Windows;
using OrdinaceApp1.Models;
using OrdinaceApp1.DataAccess;

namespace OrdinaceApp1
{
    public partial class MainWindow : Window
    {
        // Vlastnost, kam si uložíme, kdo je přihlášený
        public Uzivatel PrihlasenyUzivatel { get; private set; }

        // Upravený konstruktor - vyžaduje uživatele!
        public MainWindow(Uzivatel uzivatel)
        {
            InitializeComponent();

            PrihlasenyUzivatel = uzivatel;

            // Zobrazíme jméno v titulku okna
            this.Title = $"IS Nemocnice - Přihlášen: {uzivatel.CeleJmeno} (Role ID: {uzivatel.RoleId})";

            // Nastavíme práva (skryjeme tlačítka pro ne-adminy)
            NastavitPrava();
        }

        private void NastavitPrava()
        {
            // Příklad: Role 1 je Admin. Pokud není admin, skryjeme menu Administrace.
            if (PrihlasenyUzivatel.RoleId != 1)
            {
                MenuAdmin.Visibility = Visibility.Collapsed;
            }
        }

        // --- OBSLUHA MENU ---

        private void MenuOdhlasit_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new Views.LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void MenuUkoncit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuPacientiSeznam_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var repo = new PacientRepository();
                var data = repo.GetPacientiPublic();
                MainDataGrid.ItemsSource = data;
                TxtStatus.Text = $"Načteno {data.Count} pacientů.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání: " + ex.Message);
            }
        }

        private void MenuPacientNovy_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Zde bude formulář pro vložení pacienta.");
        }

        private void MenuLog_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Zde bude historie logů (jen pro admina).");
        }
    }
}
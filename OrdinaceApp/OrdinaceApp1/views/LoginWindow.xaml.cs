using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            try
            {
                var db = new OrdinaceApp1.DataAccess.Database();
                using (var conn = db.GetConnection())
                {
                    // Zkusíme přidat sloupec. Pokud už existuje, spadne to do catch (což nevadí)
                    using (var cmd = new Oracle.ManagedDataAccess.Client.OracleCommand(
                        "ALTER TABLE REZERVACE ADD LEKAR_ID_Lekar INTEGER", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Databáze byla úspěšně aktualizována! Sloupec pro lékaře přidán.");
            }
            catch { /* Ignorujeme chybu, pokud sloupec už existuje */ }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtLogin.Text;
            string heslo = TxtHeslo.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(heslo))
            {
                TxtChyba.Text = "Zadejte jméno a heslo.";
                return;
            }

            try
            {
                var repo = new UzivatelRepository();
                var uzivatel = repo.OveritUzivatele(login, heslo);

                if (uzivatel != null)
                {
                    var main = new OrdinaceApp1.MainWindow(uzivatel);
                    main.Show();
                    this.Close();
                }
                else
                {
                    TxtChyba.Text = "Příhlášení se nezdařilo. Chybné jméno nebo heslo.";
                }
            }
            catch (Exception ex)
            {
                TxtChyba.Text = "Chyba databáze: " + ex.Message;
            }
        }

        private void BtnRegistrace_Click(object sender, RoutedEventArgs e)
        {
            // Otevřeme okno v režimu registrace (true)
            var regWindow = new UzivatelDetailWindow(true);
            regWindow.ShowDialog();
            // Po zavření může uživatel zadat své nové údaje do loginu
        }

        private void BtnHost_Click(object sender, RoutedEventArgs e)
        {
            var host = new Uzivatel
            {
                IdUzivatel = 0,
                Jmeno = "Neregistrovaný",
                Prijmeni = "Návštěvník",
                RoleId = 4, // 4 = Host
                PrihlasovaciJmeno = "host"
            };

            var main = new MainWindow(host);
            main.Show();
            this.Close();
        }

    }
}
using System;
using System.Windows;
using OrdinaceApp1.DataAccess; // Přístup k databázi
using OrdinaceApp1.Models;     // Přístup k modelu Uzivatel

namespace OrdinaceApp1.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
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
                Uzivatel prihlasenyUzivatel = repo.OveritUzivatele(login, heslo);

                if (prihlasenyUzivatel != null)
                {
                    // ÚSPĚCH: Otevřeme hlavní okno a POŠLEME MU UŽIVATELE
                    MainWindow main = new MainWindow(prihlasenyUzivatel);
                    main.Show();
                    this.Close();
                }
                else
                {
                    TxtChyba.Text = "Chybné přihlašovací jméno nebo heslo.";
                }
            }
            catch (Exception ex)
            {
                TxtChyba.Text = "Chyba databáze: " + ex.Message;
            }
        }
    }
}
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
                    TxtChyba.Text = "Chybné jméno nebo heslo.";
                }
            }
            catch (Exception ex)
            {
                TxtChyba.Text = "Chyba databáze: " + ex.Message;
            }
        }
    }
}
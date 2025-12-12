using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class RezervaceWindow : Window
    {
        private Uzivatel _prihlaseny;

        public RezervaceWindow(Uzivatel u)
        {
            InitializeComponent();
            _prihlaseny = u;
            NacistData();
        }

        private void NacistData()
        {
            try
            {
                var repo = new RezervaceRepository();

                if (_prihlaseny.RoleId == 3) // Pacient
                {
                    // Musíme zjistit ID pacienta
                    var pacRepo = new PacientRepository();
                    int? idPac = pacRepo.GetIdPacientaByUzivatel(_prihlaseny.IdUzivatel);

                    if (idPac.HasValue)
                        DgRezervace.ItemsSource = repo.GetRezervacePacienta(idPac.Value);
                    else
                        MessageBox.Show("K vašemu účtu není přiřazena karta pacienta.");
                }
                else // Admin a Lékař vidí vše
                {
                    DgRezervace.ItemsSource = repo.GetVsechnyRezervace();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnPridat_Click(object sender, RoutedEventArgs e)
        {
            var okno = new RezervaceDetailWindow(_prihlaseny);
            okno.ShowDialog();
            NacistData();
        }

        private void BtnSmazat_Click(object sender, RoutedEventArgs e)
        {
            if (DgRezervace.SelectedItem is Rezervace r)
            {
                if (MessageBox.Show("Zrušit tuto rezervaci?", "Dotaz", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var repo = new RezervaceRepository();
                        repo.SmazatRezervaci(r.IdRezervace);
                        NacistData();
                    }
                    catch (Exception ex) { MessageBox.Show("Chyba: " + ex.Message); }
                }
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
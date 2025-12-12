using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class NovyPacientWindow : Window
    {
        private int _idPacientEdit = 0; 

        public NovyPacientWindow()
        {
            InitializeComponent();
            NacistUzivatele();
            this.Title = "Přidat nového pacienta";
        }

        public NovyPacientWindow(int idPacient)
        {
            InitializeComponent();
            _idPacientEdit = idPacient;
            this.Title = "Upravit pacienta";

            NacistUzivatele();
            NacistDataPacienta(idPacient);
        }

        private void NacistUzivatele()
        {
            try
            {
                var repo = new UzivatelRepository();
                CmbUzivatele.ItemsSource = repo.GetVsechnyUzivatele();
            }
            catch { }
        }

        private void NacistDataPacienta(int id)
        {
            try
            {
                var repo = new PacientRepository();
                var p = repo.GetPacientDetail(id);

                if (p != null)
                {
                    TxtJmeno.Text = p.Jmeno;
                    TxtPrijmeni.Text = p.Prijmeni;
                    TxtMesto.Text = p.Mesto;
                    TxtUlice.Text = p.Ulice;
                    TxtPsc.Text = p.Psc;
                    DpDatumNarozeni.SelectedDate = p.DatumNarozeni;
                    CmbUzivatele.SelectedValue = p.IdUzivatel;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání dat: " + ex.Message);
            }
        }

        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtJmeno.Text) || CmbUzivatele.SelectedValue == null)
            {
                MessageBox.Show("Vyplňte jméno a uživatele.");
                return;
            }

            int idUzivatel = (int)CmbUzivatele.SelectedValue;

            try
            {
                var repo = new PacientRepository();

                if (_idPacientEdit == 0)
                {
                    repo.PridatPacienta(
                        TxtJmeno.Text,
                        TxtPrijmeni.Text,
                        DpDatumNarozeni.SelectedDate ?? DateTime.Now,
                        TxtUlice.Text,
                        TxtMesto.Text,
                        TxtPsc.Text,
                        idUzivatel
                    );
                }
                else
                {
                    var p = new Pacient
                    {
                        IdPacient = _idPacientEdit,
                        Jmeno = TxtJmeno.Text,
                        Prijmeni = TxtPrijmeni.Text,
                        Mesto = TxtMesto.Text
                    };

                    repo.UpravitPacienta(p, idUzivatel);
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
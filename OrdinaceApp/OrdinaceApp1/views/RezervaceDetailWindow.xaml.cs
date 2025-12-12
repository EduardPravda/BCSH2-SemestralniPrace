using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class RezervaceDetailWindow : Window
    {
        private Uzivatel _prihlasenyUzivatel;
        private int? _idPacientProRezervaci;

        // konstruktor - přijímá uživatele
        public RezervaceDetailWindow(Uzivatel u)
        {
            InitializeComponent();
            _prihlasenyUzivatel = u;
            DpDatum.SelectedDate = DateTime.Now.AddDays(1);

            NacistSeznamy();
            NastavitRezimPacienta();

            // Připojíme události ručně (pokud nejsou v XAML)
            CmbLekar.SelectionChanged += (s, e) => NacistVolneTerminy();
            DpDatum.SelectedDateChanged += (s, e) => NacistVolneTerminy();
        }

        private void NacistSeznamy()
        {
            try
            {
                var pacRepo = new PacientRepository();
                CmbPacient.ItemsSource = pacRepo.GetPacientiProCombo();

                var lekRepo = new LekarRepository();
                CmbLekar.ItemsSource = lekRepo.GetVsechnyLekare();
            }
            catch { }
        }

        private void NastavitRezimPacienta()
        {
            // Pokud je to pacient, najdeme jeho ID a zamkneme výběr
            if (_prihlasenyUzivatel.RoleId == 3)
            {
                var pacRepo = new PacientRepository();
                _idPacientProRezervaci = pacRepo.GetIdPacientaByUzivatel(_prihlasenyUzivatel.IdUzivatel);

                if (_idPacientProRezervaci.HasValue)
                {
                    CmbPacient.SelectedValue = _idPacientProRezervaci.Value;
                    CmbPacient.IsEnabled = false; // Zamčeno
                }
            }
        }

        private void NacistVolneTerminy()
        {
            if (CmbLekar.SelectedValue == null || DpDatum.SelectedDate == null) return;

            int idLekar = (int)CmbLekar.SelectedValue;
            DateTime datum = DpDatum.SelectedDate.Value;

            // 1. Pracovní doba 8:00 - 16:00
            TimeSpan start = new TimeSpan(8, 0, 0);
            TimeSpan end = new TimeSpan(16, 0, 0);
            TimeSpan interval = TimeSpan.FromMinutes(30);

            // 2. Zjistíme obsazené časy z DB
            var repo = new RezervaceRepository();
            var obsazene = repo.GetObsazeneTerminy(idLekar, datum);

            // 3. Vygenerujeme volné sloty
            CmbCas.Items.Clear();
            for (TimeSpan cas = start; cas < end; cas = cas.Add(interval))
            {
                bool kolize = false;
                foreach (var obs in obsazene)
                {
                    // Porovnáme hodinu a minutu
                    if (obs.Hour == cas.Hours && obs.Minute == cas.Minutes)
                    {
                        kolize = true;
                        break;
                    }
                }

                if (!kolize)
                {
                    CmbCas.Items.Add(cas.ToString(@"hh\:mm"));
                }
            }

            if (CmbCas.Items.Count == 0) CmbCas.Items.Add("Žádné volné termíny");
            CmbCas.SelectedIndex = 0;
        }

        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            if (CmbPacient.SelectedValue == null || CmbLekar.SelectedValue == null || DpDatum.SelectedDate == null || CmbCas.SelectedItem == null)
            {
                MessageBox.Show("Vyplňte všechna pole.");
                return;
            }

            if (CmbCas.SelectedItem.ToString() == "Žádné volné termíny")
            {
                MessageBox.Show("V tento den není volný termín.");
                return;
            }

            try
            {
                // Sestavení data a času
                DateTime datum = DpDatum.SelectedDate.Value;
                TimeSpan cas = TimeSpan.Parse(CmbCas.SelectedItem.ToString());
                DateTime vyslednyDatumCas = datum.Date + cas;

                var r = new Rezervace
                {
                    IdPacient = (int)CmbPacient.SelectedValue,
                    IdLekar = (int)CmbLekar.SelectedValue,
                    Datum = vyslednyDatumCas
                };

                var repo = new RezervaceRepository();
                repo.PridatRezervaci(r);

                MessageBox.Show("Rezervace uložena.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }
    }
}
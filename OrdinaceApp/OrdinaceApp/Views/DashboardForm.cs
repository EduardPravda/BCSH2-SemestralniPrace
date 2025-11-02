using System;
using System.Drawing;
using System.Windows.Forms;

namespace OrdinaceApp.Views
{
    public partial class DashboardForm : Form
    {
        private Button btnPacienti;
        private Button btnLekari;
        private Label lblNadpis;
        private Panel topPanel;

        public DashboardForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Informační systém ordinace";
            Size = new Size(600, 400);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.WhiteSmoke;

            // Horní panel s nadpisem
            topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(0, 122, 204)
            };
            Controls.Add(topPanel);

            lblNadpis = new Label
            {
                Text = "🩺 Ordinace – Hlavní přehled",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            topPanel.Controls.Add(lblNadpis);

            // Tlačítka
            btnPacienti = new Button
            {
                Text = "👤 Pacienti",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Size = new Size(200, 60),
                Location = new Point(60, 150),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnPacienti.FlatAppearance.BorderColor = Color.Silver;
            btnPacienti.Click += (s, e) => new PacientForm().ShowDialog();
            Controls.Add(btnPacienti);

            btnLekari = new Button
            {
                Text = "🧑‍⚕️ Lékaři",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Size = new Size(200, 60),
                Location = new Point(320, 150),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLekari.FlatAppearance.BorderColor = Color.Silver;
            btnLekari.Click += (s, e) => new LekarForm().ShowDialog();
            Controls.Add(btnLekari);
        }
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.Forms.Admin
{
    public partial class FrmAdminDashboard : Form
    {
        public FrmAdminDashboard()
        {
            InitializeComponent();
            SetupDashboardUi();
        }

        private void SetupDashboardUi()
        {
            this.Text = "CineLuxe Admin Console - Dashboard";
            this.Size = new Size(1366, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(16, 20, 24);
            
            Panel pnlSidebar = new Panel();
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Width = 256;
            pnlSidebar.BackColor = Color.FromArgb(11, 15, 18);

            Label lblLogo = new Label();
            lblLogo.Text = "CineLuxe Admin";
            lblLogo.ForeColor = Color.White;
            lblLogo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblLogo.Location = new Point(20, 20);
            lblLogo.AutoSize = true;
            pnlSidebar.Controls.Add(lblLogo);

            Button btnMovies = new Button();
            btnMovies.Text = "Quản lý danh mục phim";
            btnMovies.ForeColor = Color.FromArgb(148, 163, 184);
            btnMovies.BackColor = Color.Transparent;
            btnMovies.FlatStyle = FlatStyle.Flat;
            btnMovies.FlatAppearance.BorderSize = 0;
            btnMovies.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            btnMovies.Size = new Size(220, 40);
            btnMovies.Location = new Point(15, 120);
            btnMovies.TextAlign = ContentAlignment.MiddleLeft;
            btnMovies.Cursor = Cursors.Hand;
            btnMovies.Click += BtnMovies_Click;
            pnlSidebar.Controls.Add(btnMovies);

            this.Controls.Add(pnlSidebar);
            
            Panel pnlContent = new Panel();
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.BackColor = Color.FromArgb(16, 20, 24);

            Label lblHeader = new Label();
            lblHeader.Text = "Bảng điều khiển tổng quan hệ thống";
            lblHeader.ForeColor = Color.White;
            lblHeader.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblHeader.Location = new Point(40, 30);
            lblHeader.AutoSize = true;
            pnlContent.Controls.Add(lblHeader);

            this.Controls.Add(pnlContent);
        }

        private void BtnMovies_Click(object? sender, EventArgs? e)
        {
            FrmAdminMovies frmMovies = new FrmAdminMovies();
            this.Hide();
            frmMovies.ShowDialog();
            this.Show();
        }
    }
}
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.Forms.Kiosk;

partial class FrmKioskHome
{
    private System.ComponentModel.IContainer components = null;

    private FlowLayoutPanel pnlMovieList;
    private Label lblHeader;
    private Panel pnlHeader;
    private Panel pnlFilterBar;
    private Label lblClock;
    private Label lblStep1;
    private Label lblStep2;
    private Button btnHelp;
    private Button btnExit;
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }
    private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "CineLuxe Kiosk - Trang chủ";
            this.Size = new Size(1920, 1080);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = ColorTranslator.FromHtml("#101418");

            pnlHeader = new Panel
            {
                Size = new Size(1920, 80),
                Location = new Point(0, 0),
                BackColor = ColorTranslator.FromHtml("#181c20")
            };

            lblHeader = new Label
            {
                Text = "CINELUXE KIOSK",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
                Location = new Point(24, 24),
                AutoSize = true
            };

            lblStep1 = new Label
            {
                Text = "1. Chọn Phim",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#1a5276"),
                Padding = new Padding(16, 8, 16, 8),
                Location = new Point(600, 20),
                AutoSize = true
            };

            lblStep2 = new Label
            {
                Text = "2. Chọn Ghế",
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#8b9198"),
                Location = new Point(780, 28),
                AutoSize = true
            };

            lblClock = new Label
            {
                Text = DateTime.Now.ToString("HH:mm:ss"),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                Location = new Point(1540, 26),
                AutoSize = true
            };

            btnHelp = new Button
            {
                Text = "Trợ giúp",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                BackColor = ColorTranslator.FromHtml("#6f4400"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 36),
                Location = new Point(1670, 22),
                Cursor = Cursors.Hand
            };
            btnHelp.FlatAppearance.BorderSize = 0;

            btnExit = new Button
            {
                Text = "✕ THOÁT",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#ef4444"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 36),
                Location = new Point(1795, 22),
                Cursor = Cursors.Hand
            };
            btnExit.FlatAppearance.BorderSize = 0;

            pnlHeader.Controls.Add(lblHeader);
            pnlHeader.Controls.Add(lblStep1);
            pnlHeader.Controls.Add(lblStep2);
            pnlHeader.Controls.Add(lblClock);
            pnlHeader.Controls.Add(btnHelp);
            pnlHeader.Controls.Add(btnExit);

            pnlFilterBar = new Panel
            {
                Size = new Size(1920, 56),
                Location = new Point(0, 80),
                BackColor = ColorTranslator.FromHtml("#101418")
            };

            pnlMovieList = new FlowLayoutPanel
            {
                Size = new Size(1872, 870),
                Location = new Point(24, 150),
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(0),
                BackColor = ColorTranslator.FromHtml("#101418")
            };

            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlFilterBar);
            this.Controls.Add(pnlMovieList);
            this.ResumeLayout(false);
        }
}

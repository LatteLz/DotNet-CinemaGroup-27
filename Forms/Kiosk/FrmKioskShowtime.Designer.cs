using System;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.Forms.Kiosk;

partial class FrmKioskShowtime
{
    private System.ComponentModel.IContainer components = null;

    private FlowLayoutPanel pnlShowtimes;
    private Label lblMovieTitle;
    private Button btnBack;
    private Button btnContinue;
    private Panel pnlHeader;
    private Panel pnlLeftMovieInfo;
    private Panel pnlRightShowtimes;
    private Panel pnlBottomBar;
    private FlowLayoutPanel pnlDates;
    private Label lblSelectedShowtimeSummary;
    private Label lblClock;
    private Label lblLogo;
    private Label lblStep1;
    private Label lblStep2;
    private Button btnExit;
    protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "CineLuxe Kiosk - Chọn suất chiếu";
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

            lblLogo = new Label
            {
                Text = "CINELUXE KIOSK",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
                Location = new Point(24, 26),
                AutoSize = true
            };

            lblStep1 = new Label
            {
                Text = "1. Chọn Phim",
                Font = new Font("Segoe UI", 11F),
                ForeColor = ColorTranslator.FromHtml("#8b9198"),
                Location = new Point(600, 28),
                AutoSize = true
            };

            lblStep2 = new Label
            {
                Text = "2. Chọn Suất Chiếu",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#1a5276"),
                Padding = new Padding(16, 8, 16, 8),
                Location = new Point(760, 20),
                AutoSize = true
            };

            lblClock = new Label
            {
                Text = DateTime.Now.ToString("HH:mm:ss"),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                Location = new Point(1530, 26),
                AutoSize = true
            };

            btnBack = new Button
            {
                Text = "← Quay lại ",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#94A3B8"),
                BackColor = ColorTranslator.FromHtml("#262a2f"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 38),
                Location = new Point(1660, 21),
                Cursor = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderSize = 0;

            btnExit = new Button
            {
                Text = "✕ THOÁT",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#ef4444"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 38),
                Location = new Point(1780, 21),
                Cursor = Cursors.Hand
            };
            btnExit.FlatAppearance.BorderSize = 0;

            pnlHeader.Controls.Add(lblLogo);
            pnlHeader.Controls.Add(lblStep1);
            pnlHeader.Controls.Add(lblStep2);
            pnlHeader.Controls.Add(lblClock);
            pnlHeader.Controls.Add(btnBack);
            pnlHeader.Controls.Add(btnExit);

            pnlLeftMovieInfo = new Panel
            {
                Size = new Size(400, 976),
                Location = new Point(24, 96),
                BackColor = ColorTranslator.FromHtml("#1c2024")
            };

            lblMovieTitle = new Label
            {
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#F59E0B"),
                Location = new Point(24, 24),
                AutoSize = true,
                MaximumSize = new Size(350, 0)
            };
            pnlLeftMovieInfo.Controls.Add(lblMovieTitle);

            pnlRightShowtimes = new Panel
            {
                Size = new Size(1444, 876),
                Location = new Point(452, 96),
                BackColor = ColorTranslator.FromHtml("#101418")
            };

            pnlDates = new FlowLayoutPanel
            {
                Size = new Size(1444, 76),
                Location = new Point(0, 0),
                BackColor = ColorTranslator.FromHtml("#101418")
            };

            pnlShowtimes = new FlowLayoutPanel
            {
                Location = new Point(0, 92),
                Size = new Size(1444, 784),
                AutoScroll = true,
                Padding = new Padding(24),
                BackColor = ColorTranslator.FromHtml("#1c2024")
            };

            pnlRightShowtimes.Controls.Add(pnlDates);
            pnlRightShowtimes.Controls.Add(pnlShowtimes);

            pnlBottomBar = new Panel
            {
                Size = new Size(1444, 84),
                Location = new Point(452, 988),
                BackColor = ColorTranslator.FromHtml("#0b0f12")
            };

            lblSelectedShowtimeSummary = new Label
            {
                Text = "SUẤT CHIẾU ĐANG CHỌN: (Chưa chọn suất chiếu)",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
                Location = new Point(24, 30),
                AutoSize = true
            };

            btnContinue = new Button
            {
                Text = "TIẾP TỤC CHỌN GHẾ →",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#101418"),
                BackColor = ColorTranslator.FromHtml("#282C31"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(260, 52),
                Location = new Point(1160, 16),
                Enabled = false,
                Cursor = Cursors.Hand
            };
            btnContinue.FlatAppearance.BorderSize = 0;

            pnlBottomBar.Controls.Add(lblSelectedShowtimeSummary);
            pnlBottomBar.Controls.Add(btnContinue);

            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlLeftMovieInfo);
            this.Controls.Add(pnlRightShowtimes);
            this.Controls.Add(pnlBottomBar);
            this.ResumeLayout(false);
        }

        #endregion
    }
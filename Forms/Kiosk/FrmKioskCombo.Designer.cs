using System;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.Forms.Kiosk;

partial class FrmKioskCombo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblLogo;
        private Label lblClock;
        private Button btnBack;
        private Button btnExit;
        private Label lblTitle;
        private Panel pnlBottomBar;
        private Label lblComboSummary;
        private Label lblTotalPrice;
        private Button btnContinuePayment;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.Text = "CineLuxe Kiosk - Bắp Nước & Uống";
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
                Text = "← Quay lại",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#94A3B8"),
                BackColor = ColorTranslator.FromHtml("#262a2f"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 38),
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
            pnlHeader.Controls.Add(lblClock);
            pnlHeader.Controls.Add(btnBack);
            pnlHeader.Controls.Add(btnExit);

            lblTitle = new Label
            {
                Text = "CHỌN COMBO BẮP NƯỚC UỐNG KÈM",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                Location = new Point(40, 110),
                AutoSize = true
            };

            pnlBottomBar = new Panel
            {
                Size = new Size(1920, 84),
                Location = new Point(0, 988),
                BackColor = ColorTranslator.FromHtml("#0b0f12")
            };

            lblComboSummary = new Label
            {
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#e0e3e8"),
                Location = new Point(40, 30),
                AutoSize = true
            };

            lblTotalPrice = new Label
            {
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#ffb961"),
                Location = new Point(1100, 26),
                AutoSize = true
            };

            btnContinuePayment = new Button
            {
                Text = "SANG THANH TOÁN →",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#101418"),
                BackColor = ColorTranslator.FromHtml("#ffb961"),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(240, 52),
                Location = new Point(1640, 16),
                Cursor = Cursors.Hand
            };
            btnContinuePayment.FlatAppearance.BorderSize = 0;

            pnlBottomBar.Controls.Add(lblComboSummary);
            pnlBottomBar.Controls.Add(lblTotalPrice);
            pnlBottomBar.Controls.Add(btnContinuePayment);

            this.Controls.Add(pnlHeader);
            this.Controls.Add(lblTitle);
            this.Controls.Add(pnlBottomBar);
            this.ResumeLayout(false);
        }

        #endregion
    }
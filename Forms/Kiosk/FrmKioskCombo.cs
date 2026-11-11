using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CinemaManagement.Data;

namespace CinemaManagement.Forms.Kiosk;

public class FrmKioskCombo : Form
{
    private int _showtimeId;
    private List<string> _selectedSeats;
    private decimal _seatTotal;
    private int _qtyComboSingle = 0;
    private int _qtyComboDouble = 0;
    private decimal _priceSingle = 89000m;
    private decimal _priceDouble = 119000m;

    private Label lblClock;
    private Label lblComboSummary;
    private Label lblTotalPrice;
    private Button btnContinuePayment;
    private System.Windows.Forms.Timer _timer;
    
    public FrmKioskCombo(int showtimeId, List<string> selectedSeats, decimal seatTotal)
    {
        _showtimeId = showtimeId;
        _selectedSeats = selectedSeats;
        _seatTotal = seatTotal;
        InitializeComponent();
        UpdateSummary();
        }
    private void InitializeComponent()
    {
        this.SuspendLayout();
        this.Text = "CineLuxe Kiosk - Bắp Nước & Uống";
        this.Size = new Size(1920, 1080);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.None;
        this.BackColor = ColorTranslator.FromHtml("#101418");
        Panel pnlHeader = new Panel
        {
            Size = new Size(1920, 80),
            Location = new Point(0, 0),
            BackColor = ColorTranslator.FromHtml("#181c20")
        };

        Label lblLogo = new Label
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

        Button btnBack = new Button
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
        btnBack.Click += (s, e) => this.Close();
        Button btnExit = new Button
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
        btnExit.Click += (s, e) =>
        {
            Form mainForm = Application.OpenForms.Cast<Form>().FirstOrDefault(f => f.Name == "FrmMain" || f.Name == "FrmRoleSelection" || f.Tag?.ToString() == "Main");
            mainForm?.Show();
            this.Close();
        };
        pnlHeader.Controls.Add(lblLogo);
        pnlHeader.Controls.Add(lblClock);
        pnlHeader.Controls.Add(btnBack);
        pnlHeader.Controls.Add(btnExit);
        Label lblTitle = new Label
        {
            Text = "CHỌN COMBO BẮP NƯỚC UỐNG DÈM",
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#ffb961"),
            Location = new Point(40, 110),
            AutoSize = true
        };
        Panel cardSingle = CreateComboCard("COMBO SOLO 1 NGUỜI", "1 Bắp Ngọt (L) + 1 Nước Có Gas (L)", _priceSingle, 40, 170, (qty) => {
            _qtyComboSingle = qty;
            UpdateSummary();
        });
        Panel cardDouble = CreateComboCard("COMBO COUPLE 2 NGUỜI", "1 Bắp Ngọt (XLL) + 2 Nước Có Gas (L)", _priceDouble, 500, 170, (qty) => {
            _qtyComboDouble = qty;
            UpdateSummary();
        });
        Panel pnlBottomBar = new Panel
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
        btnContinuePayment.Click += (s, e) =>
        {
            decimal totalAll = _seatTotal + (_qtyComboSingle * _priceSingle) + (_qtyComboDouble * _priceDouble);
            this.Hide();
        };
        pnlBottomBar.Controls.Add(lblComboSummary);
        pnlBottomBar.Controls.Add(lblTotalPrice);
        pnlBottomBar.Controls.Add(btnContinuePayment);

        this.Controls.Add(pnlHeader);
        this.Controls.Add(lblTitle);
        this.Controls.Add(cardSingle);
        this.Controls.Add(cardDouble);
        this.Controls.Add(pnlBottomBar);
        this.ResumeLayout(false);

        _timer = new System.Windows.Forms.Timer { Interval = 1000 };
        _timer.Tick += (s, e) => { if (lblClock != null && !lblClock.IsDisposed) lblClock.Text = DateTime.Now.ToString("HH:mm:ss"); };
        _timer.Start();
    }
    private Panel CreateComboCard(string name, string desc, decimal price, int x, int y, Action<int> onQtyChanged)
    {
        Panel pnl = new Panel
        {
            Size = new Size(420, 320),
            Location = new Point(x, y),
            BackColor = ColorTranslator.FromHtml("#1c2024")
        };

        Label lblName = new Label { Text = name, Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(20, 20), AutoSize = true };
        Label lblDesc = new Label { Text = desc, Font = new Font("Segoe UI", 10F), ForeColor = ColorTranslator.FromHtml("#94A3B8"), Location = new Point(20, 60), Size = new Size(380, 50) };
        Label lblPrice = new Label { Text = $"{price:N0} đ", Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#ffb961"), Location = new Point(20, 120), AutoSize = true };

        int currentQty = 0;
        Label lblQty = new Label { Text = "0", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(200, 230), AutoSize = true };

        Button btnMinus = new Button { Text = "-", Size = new Size(40, 40), Location = new Point(140, 222), BackColor = ColorTranslator.FromHtml("#334155"), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
        Button btnPlus = new Button { Text = "+", Size = new Size(40, 40), Location = new Point(240, 222), BackColor = ColorTranslator.FromHtml("#38bdf8"), ForeColor = ColorTranslator.FromHtml("#101418"), FlatStyle = FlatStyle.Flat };

        btnMinus.Click += (s, e) => {
            if (currentQty > 0) { currentQty--; lblQty.Text = currentQty.ToString(); onQtyChanged(currentQty); }
        };
        btnPlus.Click += (s, e) => {
            currentQty++; lblQty.Text = currentQty.ToString(); onQtyChanged(currentQty);
        };

        pnl.Controls.Add(lblName);
        pnl.Controls.Add(lblDesc);
        pnl.Controls.Add(lblPrice);
        pnl.Controls.Add(btnMinus);
        pnl.Controls.Add(lblQty);
        pnl.Controls.Add(btnPlus);
        return pnl;
    }

    private void UpdateSummary()
    {
        int totalCombo = _qtyComboSingle + _qtyComboDouble;
        lblComboSummary.Text = $"Ghế: {string.Join(", ", _selectedSeats)} | Bắp nước đã chọn: {totalCombo} phần";
        decimal totalAll = _seatTotal + (_qtyComboSingle * _priceSingle) + (_qtyComboDouble * _priceDouble);
        lblTotalPrice.Text = $"TỔNG TIỀN: {totalAll:N0} đ";
    }
}
    




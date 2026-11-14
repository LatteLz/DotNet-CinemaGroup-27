using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CinemaManagement.Data;

namespace CinemaManagement.Forms.Kiosk;

public partial class FrmKioskCombo : Form
    {
        private int _showtimeId;
        private List<string> _selectedSeats;
        private decimal _seatTotal;
        private int _qtyComboSingle = 0;
        private int _qtyComboDouble = 0;
        private decimal _priceSingle = 89000m;
        private decimal _priceDouble = 119000m;

        private System.Windows.Forms.Timer _timer;

        public FrmKioskCombo(int showtimeId, List<string> selectedSeats, decimal seatTotal)
        {
            _showtimeId = showtimeId;
            _selectedSeats = selectedSeats;
            _seatTotal = seatTotal;

            InitializeComponent();
            InitializeComboCards();
            RegisterEvents();
            UpdateSummary();
        }

        private void InitializeComboCards()
        {
            Panel cardSingle = CreateComboCard("COMBO SOLO 1 NGƯỜI", "1 Bắp Ngọt (L) + 1 Nước Có Gas (L)", _priceSingle, 40, 170, (qty) =>
            {
                _qtyComboSingle = qty;
                UpdateSummary();
            });

            Panel cardDouble = CreateComboCard("COMBO COUPLE 2 NGƯỜI", "1 Bắp Ngọt (XLL) + 2 Nước Có Gas (L)", _priceDouble, 500, 170, (qty) =>
            {
                _qtyComboDouble = qty;
                UpdateSummary();
            });

            this.Controls.Add(cardSingle);
            this.Controls.Add(cardDouble);
        }

        private void RegisterEvents()
        {
            btnBack.Click += (s, e) =>
            {
                _timer?.Stop();
                var seatForm = Application.OpenForms.OfType<FrmKioskSeat>().FirstOrDefault();
                if (seatForm != null)
                {
                    seatForm.TopMost = true;
                    seatForm.Show();
                    seatForm.Activate();
                }
                else
                {
                    var newSeatForm = new FrmKioskSeat(_showtimeId);
                    newSeatForm.TopMost = true;
                    newSeatForm.Show();
                }
                this.Close();
            };

            btnExit.Click += (s, e) =>
            {
                _timer?.Stop();
                Form? mainForm = Application.OpenForms.Cast<Form>()
                    .FirstOrDefault(f => f.Name == "FrmMain" || f.Name == "FrmRoleSelection" || f.Tag?.ToString() == "Main");
                mainForm?.Show();
                this.Close();
            };

            btnContinuePayment.Click += (s, e) =>
            {
                var selectedCombos = new List<KioskComboSelection>();

                if (_qtyComboSingle > 0)
                {
                    selectedCombos.Add(new KioskComboSelection
                    {
                        ComboId = 1,
                        ComboName = "Combo Single (1 Bắp + 1 Nước)",
                        Quantity = _qtyComboSingle,
                        UnitPrice = _priceSingle
                    });
                }

                if (_qtyComboDouble > 0)
                {
                    selectedCombos.Add(new KioskComboSelection
                    {
                        ComboId = 2,
                        ComboName = "Combo Double (1 Bắp + 2 Nước)",
                        Quantity = _qtyComboDouble,
                        UnitPrice = _priceDouble
                    });
                }

                var frmPayment = new FrmKioskPayment(
                    showtimeId: _showtimeId,
                    selectedSeats: _selectedSeats,
                    ticketTotal: _seatTotal,
                    selectedCombos: selectedCombos
                );
                frmPayment.TopMost = true;
                frmPayment.Show();
                this.Hide();
            };

            _timer = new System.Windows.Forms.Timer { Interval = 1000 };
            _timer.Tick += (s, e) =>
            {
                if (lblClock != null && !lblClock.IsDisposed)
                    lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
            };
            _timer.Start();

            this.FormClosed += (_, _) => _timer?.Dispose();
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

            Button btnMinus = new Button { Text = "-", Size = new Size(40, 40), Location = new Point(140, 222), BackColor = ColorTranslator.FromHtml("#334155"), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            Button btnPlus = new Button { Text = "+", Size = new Size(40, 40), Location = new Point(240, 222), BackColor = ColorTranslator.FromHtml("#38bdf8"), ForeColor = ColorTranslator.FromHtml("#101418"), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };

            btnMinus.Click += (s, e) =>
            {
                if (currentQty > 0)
                {
                    currentQty--;
                    lblQty.Text = currentQty.ToString();
                    onQtyChanged(currentQty);
                }
            };
            btnPlus.Click += (s, e) =>
            {
                currentQty++;
                lblQty.Text = currentQty.ToString();
                onQtyChanged(currentQty);
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
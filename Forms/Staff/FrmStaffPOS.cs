using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CinemaManagement.Models;
using CinemaManagement.Services;
using CinemaManagement.Helpers;

namespace CinemaManagement.Forms.Staff
{
    public partial class FrmStaffPOS : Form
    {
        private readonly Color COLOR_CANVAS = ColorTranslator.FromHtml("#101418");
        private readonly Color COLOR_PANEL_BG = ColorTranslator.FromHtml("#181C20");
        private readonly Color COLOR_CARD_BG = ColorTranslator.FromHtml("#22262B");
        private readonly Color COLOR_BORDER = ColorTranslator.FromHtml("#2C3238");
        
        private readonly Color COLOR_PRIMARY = ColorTranslator.FromHtml("#0284C7");
        private readonly Color COLOR_VIP = ColorTranslator.FromHtml("#F59E0B");
        private readonly Color COLOR_SWEETBOX = ColorTranslator.FromHtml("#06B6D4");
        private readonly Color COLOR_SOLD = ColorTranslator.FromHtml("#334155");
        private readonly Color COLOR_GOLD = ColorTranslator.FromHtml("#FBBF24");
        private readonly Color COLOR_TEXT_PRIMARY = ColorTranslator.FromHtml("#FFFFFF");
        private readonly Color COLOR_TEXT_SECONDARY = ColorTranslator.FromHtml("#94A3B8");
        private readonly Color COLOR_TEXT_MUTED = ColorTranslator.FromHtml("#64748B");

        private Panel pnlSidebar;
        private Panel pnlCenter;
        private Panel pnlRightPayment;

        private Label lblTotalPrice;
        private Label lblChangeDue;
        private TextBox txtCustomerCash;
        private FlowLayoutPanel flpSeatGrid;
        private FlowLayoutPanel flpComboGrid;
        private FlowLayoutPanel flpCashButtons;
        private DataGridView dgvCart;

        private decimal _totalAmount = 0m;
        private decimal _customerCash = 500000m;
        
        private List<SelectedSeatInfo> _selectedSeats = new List<SelectedSeatInfo>();
        private List<SelectedComboInfo> _selectedCombos = new List<SelectedComboInfo>();

        public class SelectedSeatInfo
        {
            public string SeatId { get; set; }
            public string SeatType { get; set; }
            public decimal Price { get; set; }
        }

        public class SelectedComboInfo
        {
            public string ComboName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }

        public FrmStaffPOS()
        {
            InitializeComponent();
            SetupFormLayout();
            BuildSidebar();
            BuildCenterArea();
            BuildRightPaymentPanel();
            RegisterShortcuts();
            UpdateCartDisplay();
        }

        private void SetupFormLayout()
        {
            pnlSidebar = new Panel
            {
                Width = 240,
                Dock = DockStyle.Left,
                BackColor = COLOR_PANEL_BG,
                Padding = new Padding(12)
            };

            pnlRightPayment = new Panel
            {
                Width = 400,
                Dock = DockStyle.Right,
                BackColor = COLOR_PANEL_BG,
                Padding = new Padding(16)
            };

            pnlCenter = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = COLOR_CANVAS,
                Padding = new Padding(16)
            };

            this.Controls.Add(pnlCenter);
            this.Controls.Add(pnlRightPayment);
            this.Controls.Add(pnlSidebar);
        }

        private void BuildSidebar()
        {
            Label lblBrand = new Label
            {
                Text = "CINELUXE\nEXCELLENCE CINEMA",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = COLOR_TEXT_PRIMARY,
                Dock = DockStyle.Top,
                Height = 50
            };

            Label lblSection = new Label
            {
                Text = "PHẦN HỆ VẬN HÀNH",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = COLOR_TEXT_MUTED,
                Dock = DockStyle.Top,
                Height = 30
            };

            Button btnNavPOS = CreateNavButton("Bán vé Quầy POS\nFrmStaffPOS", true);
    
            Button btnNavCombo = CreateNavButton("Quản lý Combo Bắp Nước\nFrmAdminCombos", false);
            btnNavCombo.Click += (s, e) => {
                FrmAdminCombos frm = new FrmAdminCombos();
                frm.ShowDialog();
            };

            Button btnNavReport = CreateNavButton("Tài chính & Doanh thu\nFrmAdminReports", false);
            btnNavReport.Click += (s, e) => {
               
                MessageBox.Show("Chức năng đang phát triển", "Thông báo");
            };

            pnlSidebar.Controls.Add(btnNavReport);
            pnlSidebar.Controls.Add(btnNavCombo);
            pnlSidebar.Controls.Add(btnNavPOS);
            pnlSidebar.Controls.Add(lblSection);
            pnlSidebar.Controls.Add(lblBrand);
        }

        private Button CreateNavButton(string text, bool isActive)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9.5f, isActive ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = isActive ? COLOR_TEXT_PRIMARY : COLOR_TEXT_SECONDARY,
                BackColor = isActive ? COLOR_PRIMARY : Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                Margin = new Padding(0, 0, 0, 5)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void BuildCenterArea()
        {
            Panel pnlMovieHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = COLOR_PANEL_BG,
                Padding = new Padding(12)
            };

            Label lblMovieTitle = new Label
            {
                Text = "T16 Dune: Part Two | 166 phút • IMAX Laser 4K",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = COLOR_TEXT_PRIMARY,
                Location = new Point(12, 10),
                AutoSize = true
            };

            Button btnShowtime1 = new Button
            {
                Text = "19:30 • P.01",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = COLOR_TEXT_PRIMARY,
                BackColor = COLOR_PRIMARY,
                Size = new Size(95, 32),
                Location = new Point(12, 38),
                FlatStyle = FlatStyle.Flat
            };
            btnShowtime1.FlatAppearance.BorderSize = 0;

            pnlMovieHeader.Controls.Add(btnShowtime1);
            pnlMovieHeader.Controls.Add(lblMovieTitle);

            Panel pnlComboArea = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = COLOR_PANEL_BG,
                Padding = new Padding(8),
                Margin = new Padding(0, 5, 0, 5)
            };

            flpComboGrid = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };
            GenerateInteractiveCombos();
            pnlComboArea.Controls.Add(flpComboGrid);

            Panel pnlScreenArea = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = COLOR_CARD_BG,
                Padding = new Padding(15)
            };

            Label lblScreen = new Label
            {
                Text = "MÀN HÌNH CHIẾU CONG CURVED LASER IMAX",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = COLOR_TEXT_MUTED,
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = COLOR_PANEL_BG
            };

            flpSeatGrid = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20, 10, 20, 10)
            };

            GenerateInteractiveSeats();

            pnlScreenArea.Controls.Add(flpSeatGrid);
            pnlScreenArea.Controls.Add(lblScreen);

            pnlCenter.Controls.Add(pnlScreenArea);
            pnlCenter.Controls.Add(pnlComboArea);
            pnlCenter.Controls.Add(pnlMovieHeader);
        }

        private void GenerateInteractiveCombos()
        {
            var combos = new[]
            {
                new { Name = "Combo Single", Price = 79000m },
                new { Name = "Combo Couple IMAX", Price = 129000m },
                new { Name = "Bắp Ngọt Lớn", Price = 55000m },
                new { Name = "Pepsi Tươi Lớn", Price = 35000m }
            };

            foreach (var cb in combos)
            {
                Button btnCombo = new Button
                {
                    Text = $"{cb.Name}\n{cb.Price:N0} đ",
                    Size = new Size(130, 42),
                    Margin = new Padding(4, 0, 4, 0),
                    Font = new Font("Segoe UI", 8, FontStyle.Bold),
                    ForeColor = COLOR_TEXT_PRIMARY,
                    BackColor = COLOR_CARD_BG,
                    FlatStyle = FlatStyle.Flat,
                    Tag = cb
                };
                btnCombo.FlatAppearance.BorderColor = COLOR_BORDER;
                btnCombo.Click += Combo_Click;
                flpComboGrid.Controls.Add(btnCombo);
            }
        }

        private void Combo_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null || btn.Tag == null) return;

            dynamic cbData = btn.Tag;
            string comboName = cbData.Name;
            decimal price = cbData.Price;

            var existing = _selectedCombos.FirstOrDefault(c => c.ComboName == comboName);
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                _selectedCombos.Add(new SelectedComboInfo { ComboName = comboName, Quantity = 1, Price = price });
            }

            UpdateCartDisplay();
        }

        private void GenerateInteractiveSeats()
        {
            string[] rows = { "A", "B", "C", "D", "G", "SW" };
            foreach (var row in rows)
            {
                for (int i = 1; i <= 10; i++)
                {
                    string seatId = $"{row}{i:D2}";
                    Button btnSeat = new Button
                    {
                        Text = seatId,
                        Size = row == "SW" ? new Size(65, 30) : new Size(36, 30),
                        Margin = new Padding(2),
                        Font = new Font("Segoe UI", 8, FontStyle.Bold),
                        FlatStyle = FlatStyle.Flat,
                        Tag = seatId
                    };
                    btnSeat.FlatAppearance.BorderSize = 1;
                    btnSeat.FlatAppearance.BorderColor = COLOR_BORDER;

                    if (row == "A" && i <= 3)
                    {
                        btnSeat.BackColor = COLOR_SOLD;
                        btnSeat.ForeColor = COLOR_TEXT_MUTED;
                        btnSeat.Enabled = false;
                    }
                    else if (row == "G" && (i >= 6 && i <= 8))
                    {
                        btnSeat.BackColor = COLOR_VIP;
                        btnSeat.ForeColor = Color.Black;
                        btnSeat.Click += Seat_Click;
                    }
                    else if (row == "SW")
                    {
                        btnSeat.BackColor = COLOR_SWEETBOX;
                        btnSeat.ForeColor = Color.Black;
                        btnSeat.Click += Seat_Click;
                    }
                    else
                    {
                        btnSeat.BackColor = COLOR_PANEL_BG;
                        btnSeat.ForeColor = COLOR_TEXT_PRIMARY;
                        btnSeat.Click += Seat_Click;
                    }

                    flpSeatGrid.Controls.Add(btnSeat);
                }
            }
        }

        private void Seat_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            string seatId = btn.Tag.ToString();
            string rowPrefix = seatId.Substring(0, 1);
            if (seatId.StartsWith("SW")) rowPrefix = "SW";

            decimal price = 90000m;
            string seatType = "Standard";

            if (rowPrefix == "G" && int.TryParse(seatId.Substring(1), out int num) && num >= 6 && num <= 8)
            {
                price = 110000m;
                seatType = "VIP";
            }
            else if (rowPrefix == "SW")
            {
                price = 220000m;
                seatType = "Sweetbox";
            }

            var existing = _selectedSeats.FirstOrDefault(s => s.SeatId == seatId);
            if (existing != null)
            {
                _selectedSeats.Remove(existing);
                
                if (seatType == "VIP") btn.BackColor = COLOR_VIP;
                else if (seatType == "Sweetbox") btn.BackColor = COLOR_SWEETBOX;
                else btn.BackColor = COLOR_PANEL_BG;
                
                btn.ForeColor = seatType == "Standard" ? COLOR_TEXT_PRIMARY : Color.Black;
            }
            else
            {
                _selectedSeats.Add(new SelectedSeatInfo { SeatId = seatId, SeatType = seatType, Price = price });
                
                btn.BackColor = COLOR_PRIMARY;
                btn.ForeColor = COLOR_TEXT_PRIMARY;
            }

            UpdateCartDisplay();
        }

        private void BuildRightPaymentPanel()
        {
            Label lblStaffInfo = new Label
            {
                Text = "Thu ngân: Julian Vance (Ca tối #2)\nKhách: 0987654321 • Trần Anh Tuấn",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = COLOR_TEXT_SECONDARY,
                Dock = DockStyle.Top,
                Height = 45
            };

            dgvCart = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 160,
                BackgroundColor = COLOR_CARD_BG,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            ConfigureCartGridColumns();

            Panel pnlTotalBox = new Panel
            {
                Dock = DockStyle.Top,
                Height = 200,
                BackColor = COLOR_CARD_BG,
                Padding = new Padding(10),
                Margin = new Padding(0, 5, 0, 5)
            };

            Label lblTotalTitle = new Label
            {
                Text = "TỔNG CỘNG THANH TOÁN (FINAL TOTAL):",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = COLOR_TEXT_SECONDARY,
                Location = new Point(10, 8),
                AutoSize = true
            };

            lblTotalPrice = new Label
            {
                Text = "0 đ",
                Font = new Font("JetBrains Mono", 22, FontStyle.Bold),
                ForeColor = COLOR_GOLD,
                Location = new Point(10, 26),
                AutoSize = true
            };

            Label lblCashTitle = new Label
            {
                Text = "Tiền khách đưa (CASH):",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = COLOR_TEXT_SECONDARY,
                Location = new Point(10, 72),
                AutoSize = true
            };

            txtCustomerCash = new TextBox
            {
                Text = "500000",
                Font = new Font("JetBrains Mono", 12, FontStyle.Bold),
                Location = new Point(10, 95),
                Size = new Size(355, 28),
                BackColor = COLOR_CANVAS,
                ForeColor = COLOR_TEXT_PRIMARY
            };
            txtCustomerCash.TextChanged += (s, e) =>
            {
                if (decimal.TryParse(txtCustomerCash.Text.Trim(), out decimal cash))
                {
                    _customerCash = cash;
                }
                else
                {
                    _customerCash = 0m;
                }
                UpdateChangeDueDisplay();
            };
            txtCustomerCash.Click += (s, e) =>
            {
                txtCustomerCash.SelectAll();
            };

            flpCashButtons = new FlowLayoutPanel
            {
                Location = new Point(10, 130),
                Size = new Size(355, 32),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            AddCashPresetButton("100K", 100000m);
            AddCashPresetButton("200K", 200000m);
            AddCashPresetButton("500K", 500000m);
            AddCashPresetButton("1.000K", 1000000m);
            AddCashPresetButton("Đủ tiền", -1m);

            pnlTotalBox.Controls.Add(flpCashButtons);
            pnlTotalBox.Controls.Add(txtCustomerCash);
            pnlTotalBox.Controls.Add(lblCashTitle);
            pnlTotalBox.Controls.Add(lblTotalPrice);
            pnlTotalBox.Controls.Add(lblTotalTitle);

            lblChangeDue = new Label
            {
                Text = "TIỀN THỐI LẠI (CHANGE DUE): 0 đ",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = COLOR_PRIMARY,
                Dock = DockStyle.Top,
                Height = 35,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Button btnCheckout = new Button
            {
                Text = "IN VÉ & HOÀN TẤT GIAO DỊCH [Enter]",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = COLOR_TEXT_PRIMARY,
                BackColor = COLOR_PRIMARY,
                Dock = DockStyle.Bottom,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCheckout.FlatAppearance.BorderSize = 0;
            btnCheckout.Click += (s, e) => ExecuteCheckout();

            pnlRightPayment.Controls.Add(btnCheckout);
            pnlRightPayment.Controls.Add(lblChangeDue);
            pnlRightPayment.Controls.Add(pnlTotalBox);
            pnlRightPayment.Controls.Add(dgvCart);
            pnlRightPayment.Controls.Add(lblStaffInfo);
        }

        private void AddCashPresetButton(string label, decimal amount)
        {
            Button btn = new Button
            {
                Text = label,
                Size = new Size(66, 28),
                Margin = new Padding(0, 0, 4, 0),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = COLOR_TEXT_PRIMARY,
                BackColor = COLOR_PANEL_BG,
                FlatStyle = FlatStyle.Flat
            };
            btn.FlatAppearance.BorderColor = COLOR_BORDER;
            btn.Click += (s, e) =>
            {
                if (amount == -1m)
                {
                    _customerCash = _totalAmount;
                }
                else
                {
                    _customerCash = amount;
                }
                txtCustomerCash.Text = _customerCash.ToString("0");
            };
            flpCashButtons.Controls.Add(btn);
        }

        private void ConfigureCartGridColumns()
        {
            dgvCart.Columns.Add("ItemName", "MẶT HÀNG");
            dgvCart.Columns.Add("Quantity", "SL");
            dgvCart.Columns.Add("TotalPrice", "THÀNH TIỀN");

            dgvCart.DefaultCellStyle.BackColor = COLOR_CARD_BG;
            dgvCart.DefaultCellStyle.ForeColor = COLOR_TEXT_PRIMARY;
            dgvCart.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvCart.ColumnHeadersDefaultCellStyle.BackColor = COLOR_PANEL_BG;
            dgvCart.ColumnHeadersDefaultCellStyle.ForeColor = COLOR_TEXT_MUTED;
            dgvCart.EnableHeadersVisualStyles = false;
        }

        private void UpdateCartDisplay()
        {
            dgvCart.Rows.Clear();
            _totalAmount = 0m;

            if (_selectedSeats.Count > 0)
            {
                var groupedSeats = _selectedSeats.GroupBy(s => s.SeatType);
                foreach (var group in groupedSeats)
                {
                    int count = group.Count();
                    decimal subTotal = group.Sum(s => s.Price);
                    _totalAmount += subTotal;

                    string seatNames = string.Join(", ", group.Select(s => s.SeatId));
                    dgvCart.Rows.Add($"Ghế {group.Key} ({seatNames})", $"{count:00}", $"{subTotal:N0} đ");
                }
            }

            foreach (var cb in _selectedCombos)
            {
                decimal subTotal = cb.Quantity * cb.Price;
                _totalAmount += subTotal;
                dgvCart.Rows.Add(cb.ComboName, $"{cb.Quantity:00}", $"{subTotal:N0} đ");
            }

            lblTotalPrice.Text = $"{_totalAmount:N0} đ";
            UpdateChangeDueDisplay();
        }

        private void UpdateChangeDueDisplay()
        {
            decimal changeDue = _customerCash - _totalAmount;
            lblChangeDue.Text = $"TIỀN THỐI LẠI (CHANGE DUE): {(changeDue >= 0 ? changeDue : 0):N0} đ";
            if (changeDue < 0)
            {
                lblChangeDue.ForeColor = ColorTranslator.FromHtml("#EF4444");
            }
            else
            {
                lblChangeDue.ForeColor = COLOR_PRIMARY;
            }
        }

        private void RegisterShortcuts()
        {
            this.KeyDown += (s, e) =>
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        ExecuteCheckout();
                        break;
                    case Keys.F1:
                        MessageBox.Show("Thanh toán: TIỀN MẶT [F1]", "Phương thức thanh toán");
                        break;
                    case Keys.F2:
                        MessageBox.Show("Thanh toán: QUẸT THẺ POS [F2]", "Phương thức thanh toán");
                        break;
                    case Keys.F3:
                        MessageBox.Show("Thanh toán: VIETQR / VÍ ELEC [F3]", "Phương thức thanh toán");
                        break;
                    case Keys.Escape:
                        if (MessageBox.Show("Hủy giao dịch hiện tại?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this.Close();
                        }
                        break;
                }
            };
        }

        private void ExecuteCheckout()
        {
            if (_selectedSeats.Count == 0 && _selectedCombos.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ghế hoặc bắp nước trước khi thanh toán!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_customerCash < _totalAmount)
            {
                MessageBox.Show("Số tiền khách đưa chưa đủ để thanh toán!", "Lỗi thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PaymentService paymentService = new PaymentService();
            decimal change = paymentService.CalculateChange(_totalAmount, _customerCash);

            MessageBox.Show($"Giao dịch thành công!\nTổng tiền: {_totalAmount:N0} đ\nTiền khách đưa: {_customerCash:N0} đ\nTiền thối lại: {change:N0} đ\nĐang in vé...", 
                            "In vé POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            _selectedSeats.Clear();
            _selectedCombos.Clear();
            foreach (Control ctrl in flpSeatGrid.Controls)
            {
                if (ctrl is Button btn && btn.Enabled)
                {
                    string seatId = btn.Tag?.ToString() ?? "";
                    if (seatId.StartsWith("G") && seatId != "G01") btn.BackColor = COLOR_VIP;
                    else if (seatId.StartsWith("SW")) btn.BackColor = COLOR_SWEETBOX;
                    else btn.BackColor = COLOR_PANEL_BG;
                }
            }
            UpdateCartDisplay();
        }
    }
}
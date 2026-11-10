using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace CinemaManagement.Forms.Staff
{
    public partial class FrmStaffPOS : Form
    {
        private readonly Color ColorBg = ColorTranslator.FromHtml("#101418");
        private readonly Color ColorSeatNormal = ColorTranslator.FromHtml("#1E293B");
        private readonly Color ColorSeatSelected = ColorTranslator.FromHtml("#F59E0B");
        private readonly Color ColorSeatText = ColorTranslator.FromHtml("#FFFFFF");

        private decimal currentTotal = 314100;
        private string selectedMovie = "Dune 2";
        private string selectedShowtime = "19:30 • P.01";
        private decimal ticketPrice = 110000;

        public FrmStaffPOS()
        {
            InitializeComponent();
            SetupCustomUI();
            SetupEvents();
            BuildDynamicSeatMap();
            LoadDefaultOrderItems();
            BuildComboCatalog();
            UpdateTotalDisplay();
        }

        private void SetupCustomUI()
        {
            this.BackColor = ColorBg;
            this.ForeColor = Color.White;
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            dgvOrderItems.BackgroundColor = ColorTranslator.FromHtml("#161B20");
            dgvOrderItems.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#161B20");
            dgvOrderItems.DefaultCellStyle.ForeColor = Color.White;
            dgvOrderItems.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#1E293B");
            dgvOrderItems.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvOrderItems.RowTemplate.Height = 35;
        }

        private void LoadDefaultOrderItems()
        {
            dgvOrderItems.Rows.Clear();
            dgvOrderItems.Rows.Add($"Ghế VIP IMAX Laser (G07, G08)\n{selectedMovie} • {selectedShowtime}", "02", $"{(ticketPrice * 2):N0} đ");
            dgvOrderItems.Rows.Add("Combo Couple IMAX\n01 Bắp Caramel L + 02 Pepsi", "01", "129.000 đ");
            dgvOrderItems.Rows.Add("Ưu đãi Thẻ Vàng (10%)", "-", "-34.900 đ");
            dgvOrderItems.ClearSelection();
        }

        private void SetupEvents()
        {
            lnkPos.LinkClicked += (s, e) => {
                MessageBox.Show("Bạn đang ở màn hình Bán vé Quầy POS (FrmStaffPOS).", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            lnkCombo.LinkClicked += (s, e) => {
                try
                {
                    FrmAdminCombos adminComboForm = new FrmAdminCombos();
                    adminComboForm.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể mở màn hình Quản lý Combo: " + ex.Message, "Lỗi Điều Hướng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            lnkFinance.LinkClicked += (s, e) => {
                MessageBox.Show("Mở phân hệ [FrmAdminReports - Tài chính & Doanh thu].", "Điều Hướng", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            lnkOccupancy.LinkClicked += (s, e) => {
                MessageBox.Show("Mở phân hệ [FrmOccupancyReport - Lấp đầy Khán phòng].", "Điều Hướng", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            lnkAbout.LinkClicked += (s, e) => {
                MessageBox.Show("Cineluxe Management Studio - Phiên bản .NET 8.0 WPF/WinForms Native Engine.", "Giới Thiệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            lblMovie1.Click += (s, e) => SelectMovie("Dune 2", "19:30 • P.01", 110000, lblMovie1, lblMovie2, lblMovie3);
            lblMovie2.Click += (s, e) => SelectMovie("Godzilla x Kong", "20:15 • P.03", 95000, lblMovie2, lblMovie1, lblMovie3);
            lblMovie3.Click += (s, e) => SelectMovie("18+ Mai", "21:00 • P.04", 90000, lblMovie3, lblMovie1, lblMovie2);

            btnCheckout.Click += (s, e) => {
                decimal cashGiven = ParseMoney(txtCashGiven.Text);
                if (cashGiven < currentTotal)
                {
                    MessageBox.Show("Tiền khách đưa chưa đủ để thanh toán giao dịch này! Vui lòng kiểm tra lại.", "Lỗi Thanh Toán", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Đã in vé và hoàn tất giao dịch thanh toán thành công!", "Hoàn Tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnCancelOrder.Click += (s, e) => {
                dgvOrderItems.Rows.Clear();
                currentTotal = 0;
                UpdateTotalDisplay();
                MessageBox.Show("Đã hủy giao dịch hiện tại.", "Hủy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            };

            btnReprint.Click += (s, e) => MessageBox.Show("Đang in lại vé gần nhất...", "In Lại", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnChangeSeat.Click += (s, e) => MessageBox.Show("Vui lòng click trực tiếp vào các ô ghế trên sơ đồ để thay đổi ghế ngồi.", "Đổi Ghế", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnChangeCustomer.Click += (s, e) => MessageBox.Show("Mở danh mục tra cứu thành viên CineGold.", "Đổi Khách", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnTabSeat.Click += (s, e) => {
                btnTabSeat.BackColor = ColorTranslator.FromHtml("#1E293B");
                btnTabSeat.ForeColor = Color.White;
                btnTabCombo.BackColor = ColorTranslator.FromHtml("#181C20");
                btnTabCombo.ForeColor = ColorTranslator.FromHtml("#94A3B8");
                pnlSeatMatrix.Visible = true;
                pnlComboCatalog.Visible = false;
            };

            btnTabCombo.Click += (s, e) => {
                btnTabCombo.BackColor = ColorTranslator.FromHtml("#1E293B");
                btnTabCombo.ForeColor = Color.White;
                btnTabSeat.BackColor = ColorTranslator.FromHtml("#181C20");
                btnTabSeat.ForeColor = ColorTranslator.FromHtml("#94A3B8");
                pnlSeatMatrix.Visible = false;
                pnlComboCatalog.Visible = true;
            };

            txtCashGiven.TextChanged += (s, e) => UpdateTotalDisplay();

            btnCash200k.Click += (s, e) => SetCash(200000);
            btnCash500k.Click += (s, e) => SetCash(500000);
            btnCash1m.Click += (s, e) => SetCash(1000000);
            btnCashExact.Click += (s, e) => SetCash(currentTotal);
        }

        private void SelectMovie(string movieName, string showtime, decimal price, Label activeLbl, Label other1, Label other2)
        {
            selectedMovie = movieName;
            selectedShowtime = showtime;
            ticketPrice = price;

            activeLbl.BackColor = ColorTranslator.FromHtml("#304159");
            activeLbl.ForeColor = Color.White;
            activeLbl.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);

            other1.BackColor = ColorTranslator.FromHtml("#181C20");
            other1.ForeColor = ColorTranslator.FromHtml("#94A3B8");
            other1.Font = new Font("Segoe UI", 8.5F);

            other2.BackColor = ColorTranslator.FromHtml("#181C20");
            other2.ForeColor = ColorTranslator.FromHtml("#94A3B8");
            other2.Font = new Font("Segoe UI", 8.5F);

            lblShowtime1.Text = $"SUẤT CHIẾU: {showtime}";

            if (dgvOrderItems.Rows.Count > 0)
            {
                dgvOrderItems.Rows[0].Cells[0].Value = $"Ghế VIP IMAX Laser (G07, G08)\n{selectedMovie} • {selectedShowtime}";
                dgvOrderItems.Rows[0].Cells[2].Value = $"{(ticketPrice * 2):N0} đ";
            }

            currentTotal = (ticketPrice * 2) + 129000 - 34900;
            UpdateTotalDisplay();
            MessageBox.Show($"Đã chuyển sang phim: {selectedMovie}", "Đổi Phim", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BuildDynamicSeatMap()
        {
            pnlSeatMatrix.Controls.Clear();
            string[] rows = { "A", "B", "C", "D", "G" };
            int startY = 15;

            foreach (string row in rows)
            {
                Label lblRow = new Label();
                lblRow.Text = row;
                lblRow.Font = new Font("Consolas", 10F, FontStyle.Bold);
                lblRow.ForeColor = ColorTranslator.FromHtml("#94A3B8");
                lblRow.Location = new Point(15, startY + 4);
                lblRow.Size = new Size(20, 25);
                pnlSeatMatrix.Controls.Add(lblRow);

                for (int i = 1; i <= 10; i++)
                {
                    Button btnSeat = new Button();
                    btnSeat.Text = i.ToString("D2");
                    btnSeat.Font = new Font("Consolas", 8.5F, FontStyle.Bold);
                    btnSeat.Size = new Size(38, 30);
                    btnSeat.Location = new Point(45 + (i * 42) + (i > 5 ? 20 : 0), startY);
                    btnSeat.FlatStyle = FlatStyle.Flat;
                    btnSeat.FlatAppearance.BorderSize = 1;
                    
                    if (row == "G" && (i == 7 || i == 8))
                    {
                        btnSeat.BackColor = ColorSeatSelected;
                        btnSeat.ForeColor = Color.Black;
                        btnSeat.FlatAppearance.BorderColor = Color.White;
                    }
                    else
                    {
                        btnSeat.BackColor = ColorSeatNormal;
                        btnSeat.ForeColor = ColorSeatText;
                        btnSeat.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#334155");
                    }

                    string seatName = $"{row}{i:D2}";
                    btnSeat.Click += (s, e) => {
                        if (btnSeat.BackColor == ColorSeatSelected)
                        {
                            btnSeat.BackColor = ColorSeatNormal;
                            btnSeat.ForeColor = ColorSeatText;
                            currentTotal -= ticketPrice;
                            lblSelectedSeats.Text = $"🟢 Đã hủy chọn ghế {seatName}";
                        }
                        else
                        {
                            btnSeat.BackColor = ColorSeatSelected;
                            btnSeat.ForeColor = Color.Black;
                            currentTotal += ticketPrice;
                            dgvOrderItems.Rows.Add($"Ghế VIP IMAX Laser ({seatName})\n{selectedMovie} • {selectedShowtime}", "01", $"{ticketPrice:N0} đ");
                            dgvOrderItems.ClearSelection();
                            lblSelectedSeats.Text = $"🟢 Đang chọn thêm ghế: {seatName}";
                        }
                        UpdateTotalDisplay();
                    };

                    pnlSeatMatrix.Controls.Add(btnSeat);
                }
                startY += 40;
            }
        }

        private void BuildComboCatalog()
        {
            pnlComboCatalog.Controls.Clear();
            pnlComboCatalog.Visible = false;

            string[,] combos = {
                { "Combo Single IMAX", "01 Bắp Ngọt M + 01 Pepsi M", "75000", "75.000 đ" },
                { "Combo Couple IMAX", "01 Bắp Caramel L + 02 Pepsi L", "129000", "129.000 đ" },
                { "Combo Family Đại Gia Đình", "02 Bắp L + 03 Nước Ngọt L + 01 Snack", "199000", "199.000 đ" },
                { "Bắp Phô Mai Đặc Biệt L", "01 Hũ Bắp Vị Phô Mai Thượng Hạng L", "65000", "65.000 đ" },
                { "Nước Ngọt Pepsi L", "01 Ly Pepsi L (Refill 1 lần)", "35000", "35.000 đ" },
                { "Nước Suối Aquafina", "01 Chai Nước Suối 500ml", "20000", "20.000 đ" }
            };

            int startX = 20;
            int startY = 20;

            for (int i = 0; i < combos.GetLength(0); i++)
            {
                string name = combos[i, 0];
                string desc = combos[i, 1];
                decimal price = decimal.Parse(combos[i, 2]);
                string priceStr = combos[i, 3];

                Panel pnlItem = new Panel();
                pnlItem.Size = new Size(330, 95);
                pnlItem.Location = new Point(startX, startY);
                pnlItem.BackColor = ColorTranslator.FromHtml("#1E293B");
                pnlItem.Padding = new Padding(10);

                Label lblName = new Label();
                lblName.Text = name;
                lblName.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                lblName.ForeColor = ColorTranslator.FromHtml("#38BDF8");
                lblName.Location = new Point(10, 10);
                lblName.AutoSize = true;

                Label lblDesc = new Label();
                lblDesc.Text = desc;
                lblDesc.Font = new Font("Segoe UI", 8F);
                lblDesc.ForeColor = ColorTranslator.FromHtml("#94A3B8");
                lblDesc.Location = new Point(10, 32);
                lblDesc.Size = new Size(210, 30);

                Label lblPrice = new Label();
                lblPrice.Text = priceStr;
                lblPrice.Font = new Font("Consolas", 10F, FontStyle.Bold);
                lblPrice.ForeColor = ColorTranslator.FromHtml("#F59E0B");
                lblPrice.Location = new Point(10, 62);
                lblPrice.AutoSize = true;

                Button btnAdd = new Button();
                btnAdd.Text = "+ Chọn";
                btnAdd.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                btnAdd.BackColor = ColorTranslator.FromHtml("#0284C7");
                btnAdd.ForeColor = Color.White;
                btnAdd.FlatStyle = FlatStyle.Flat;
                btnAdd.Location = new Point(230, 52);
                btnAdd.Size = new Size(85, 32);
                btnAdd.FlatAppearance.BorderSize = 0;

                btnAdd.Click += (s, e) => {
                    dgvOrderItems.Rows.Add($"{name}\n{desc}", "01", priceStr);
                    dgvOrderItems.ClearSelection();
                    currentTotal += price;
                    UpdateTotalDisplay();
                    MessageBox.Show($"Đã thêm [{name}] vào hóa đơn thành công!", "Thêm Combo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                pnlItem.Controls.Add(lblName);
                pnlItem.Controls.Add(lblDesc);
                pnlItem.Controls.Add(lblPrice);
                pnlItem.Controls.Add(btnAdd);

                pnlComboCatalog.Controls.Add(pnlItem);

                startX += 350;
                if ((i + 1) % 2 == 0)
                {
                    startX = 20;
                    startY += 105;
                }
            }
        }

        private void UpdateTotalDisplay()
        {
            lblTotalAmount.Text = $"{currentTotal:N0} đ";
            decimal cashGiven = ParseMoney(txtCashGiven.Text);
            decimal change = cashGiven - currentTotal;
            
            if (change >= 0)
            {
                lblChangeDue.Text = $"TIỀN THỐI LẠI: {change:N0} đ";
                lblChangeDue.ForeColor = ColorTranslator.FromHtml("#38BDF8");
            }
            else
            {
                lblChangeDue.Text = "TIỀN THỐI LẠI: Không đủ";
                lblChangeDue.ForeColor = Color.FromArgb(239, 68, 68);
            }
        }

        private void SetCash(decimal amount)
        {
            txtCashGiven.Text = $"{amount:N0} đ";
        }

        private decimal ParseMoney(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            string clean = text.Replace("đ", "").Replace(".", "").Replace(",", "").Trim();
            decimal.TryParse(clean, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal val);
            return val;
        }
    }
}
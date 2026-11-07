using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using CinemaManagement.Services;
using CinemaManagement.Models;

namespace CinemaManagement.Forms.Staff
{
    public partial class FrmStaffPOS : Form
    {
        private readonly ComboService _comboService;
        
        private Label lblTitle;
        
        private Label lblMovieSelection;
        private ComboBox cbMovies;
        private Label lblShowtimeSelection;
        private ComboBox cbShowtimes;

        private DataGridView dgvCombos; 
        private Button btnAddCombo;

        private Label lblTotalLabel;
        private Label lblTotalPrice;
        private Label lblCashGiven;
        private TextBox txtCashGiven;
        private Label lblChange;
        private Label lblChangeAmount;
        private Button btnCheckout;

        private decimal _ticketPrice = 60000; 
        private decimal _totalAmount = 60000; 
        private List<ComboItem> _selectedCombos = new List<ComboItem>();

        public FrmStaffPOS()
        {
            _comboService = new ComboService();
            SetupPOSComponents();
            LoadMovieAndShowtimeData();
            LoadComboDataToPOS();
        }

        private void SetupPOSComponents()
        {
            this.Text = "Quầy Bán Vé & POS - FrmStaffPOS";
            this.Size = new System.Drawing.Size(1024, 740);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(20, 20, 20);

            lblTitle = new Label();
            lblTitle.Text = "QUẦY THU NGÂN POS - BÁN VÉ & F&B";
            lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 15);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            lblMovieSelection = new Label();
            lblMovieSelection.Text = "Chọn Phim:";
            lblMovieSelection.ForeColor = Color.White;
            lblMovieSelection.Location = new Point(20, 60);
            lblMovieSelection.AutoSize = true;
            this.Controls.Add(lblMovieSelection);

            cbMovies = new ComboBox();
            cbMovies.Location = new Point(20, 85);
            cbMovies.Size = new System.Drawing.Size(280, 25);
            cbMovies.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            this.Controls.Add(cbMovies);

            lblShowtimeSelection = new Label();
            lblShowtimeSelection.Text = "Chọn Suất Chiếu:";
            lblShowtimeSelection.ForeColor = Color.White;
            lblShowtimeSelection.Location = new Point(320, 60);
            lblShowtimeSelection.AutoSize = true;
            this.Controls.Add(lblShowtimeSelection);

            cbShowtimes = new ComboBox();
            cbShowtimes.Location = new Point(320, 85);
            cbShowtimes.Size = new System.Drawing.Size(280, 25);
            cbShowtimes.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            cbShowtimes.SelectedIndexChanged += cbShowtimes_SelectedIndexChanged;
            this.Controls.Add(cbShowtimes);
   
            dgvCombos = new DataGridView();
            dgvCombos.Location = new Point(20, 130);
            dgvCombos.Size = new System.Drawing.Size(580, 320);
            dgvCombos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCombos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Controls.Add(dgvCombos);

            btnAddCombo = new Button();
            btnAddCombo.Text = "+ Thêm Combo vào đơn";
            btnAddCombo.BackColor = Color.DarkOrange;
            btnAddCombo.ForeColor = Color.White;
            btnAddCombo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAddCombo.FlatStyle = FlatStyle.Flat;
            btnAddCombo.Location = new Point(20, 465);
            btnAddCombo.Size = new System.Drawing.Size(580, 40);
            btnAddCombo.Click += btnAddCombo_Click;
            this.Controls.Add(btnAddCombo);

            lblTotalLabel = new Label();
            lblTotalLabel.Text = "TỔNG TIỀN THANH TOÁN:";
            lblTotalLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblTotalLabel.ForeColor = Color.Orange;
            lblTotalLabel.Location = new Point(630, 85);
            lblTotalLabel.AutoSize = true;
            this.Controls.Add(lblTotalLabel);

            lblTotalPrice = new Label();
            lblTotalPrice.Text = _totalAmount.ToString("N0") + " VNĐ";
            lblTotalPrice.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTotalPrice.ForeColor = Color.LimeGreen;
            lblTotalPrice.Location = new Point(630, 115);
            lblTotalPrice.AutoSize = true;
            this.Controls.Add(lblTotalPrice);

            lblCashGiven = new Label();
            lblCashGiven.Text = "Tiền khách đưa (VNĐ):";
            lblCashGiven.ForeColor = Color.White;
            lblCashGiven.Location = new Point(630, 180);
            lblCashGiven.AutoSize = true;
            this.Controls.Add(lblCashGiven);

            txtCashGiven = new TextBox();
            txtCashGiven.Location = new Point(630, 210);
            txtCashGiven.Size = new System.Drawing.Size(320, 25);
            txtCashGiven.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            txtCashGiven.TextChanged += txtCashGiven_TextChanged;
            this.Controls.Add(txtCashGiven);

            lblChange = new Label();
            lblChange.Text = "Tiền thừa trả khách:";
            lblChange.ForeColor = Color.White;
            lblChange.Location = new Point(630, 260);
            lblChange.AutoSize = true;
            this.Controls.Add(lblChange);

            lblChangeAmount = new Label();
            lblChangeAmount.Text = "0 VNĐ";
            lblChangeAmount.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblChangeAmount.ForeColor = Color.Cyan;
            lblChangeAmount.Location = new Point(630, 290);
            lblChangeAmount.AutoSize = true;
            this.Controls.Add(lblChangeAmount);

            btnCheckout = new Button();
            btnCheckout.Text = "IN VÉ & HOÀN TẤT GIAO DỊCH";
            btnCheckout.BackColor = Color.DodgerBlue;
            btnCheckout.ForeColor = Color.White;
            btnCheckout.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnCheckout.FlatStyle = FlatStyle.Flat;
            btnCheckout.Location = new Point(630, 360);
            btnCheckout.Size = new System.Drawing.Size(320, 50);
            btnCheckout.Click += btnCheckout_Click;
            this.Controls.Add(btnCheckout);
        }

        private void LoadMovieAndShowtimeData()
        {
            cbMovies.Items.Add("Avatar: Dòng Chảy Của Nước");
            cbMovies.Items.Add("Lật Mặt 7: Một Điều Ước");
            cbMovies.Items.Add("Doraemon: Nobita và Bản Giao Hưởng Địa Cầu");
            if (cbMovies.Items.Count > 0) cbMovies.SelectedIndex = 0;

            cbShowtimes.Items.Add("10:00 - Phòng chiếu 01 (60,000 VNĐ)");
            cbShowtimes.Items.Add("14:30 - Phòng chiếu 02 (75,000 VNĐ)");
            cbShowtimes.Items.Add("19:00 - Phòng chiếu VIP (100,000 VNĐ)");
            if (cbShowtimes.Items.Count > 0) cbShowtimes.SelectedIndex = 0;
        }

        private void cbShowtimes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbShowtimes.SelectedIndex == 0) _ticketPrice = 60000;
            else if (cbShowtimes.SelectedIndex == 1) _ticketPrice = 75000;
            else if (cbShowtimes.SelectedIndex == 2) _ticketPrice = 100000;

            RecalculateTotal();
        }

        private void LoadComboDataToPOS()
        {
            try
            {
                var combos = _comboService.GetAllCombos();
                dgvCombos.DataSource = combos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục combo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddCombo_Click(object sender, EventArgs e)
        {
            if (dgvCombos.SelectedRows.Count > 0)
            {
                var selectedCombo = dgvCombos.SelectedRows[0].DataBoundItem as ComboItem;
                if (selectedCombo != null)
                {
                    _selectedCombos.Add(selectedCombo);
                    RecalculateTotal();
                    
                    MessageBox.Show($"Đã thêm [{selectedCombo.ComboName}] vào hóa đơn!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một gói combo từ danh sách bên trái!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RecalculateTotal()
        {
            decimal comboTotal = 0;
            foreach (var combo in _selectedCombos)
            {
                comboTotal += combo.Price;
            }

            _totalAmount = _ticketPrice + comboTotal;
            lblTotalPrice.Text = _totalAmount.ToString("N0") + " VNĐ";
            
            txtCashGiven_TextChanged(null, null);
        }

        private void txtCashGiven_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtCashGiven.Text, out decimal cashGiven))
            {
                decimal change = cashGiven - _totalAmount;
                if (change >= 0)
                {
                    lblChangeAmount.Text = change.ToString("N0") + " VNĐ";
                    lblChangeAmount.ForeColor = Color.Cyan;
                }
                else
                {
                    lblChangeAmount.Text = "Chưa đủ tiền!";
                    lblChangeAmount.ForeColor = Color.Red;
                }
            }
            else
            {
                lblChangeAmount.Text = "0 VNĐ";
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtCashGiven.Text, out decimal cashGiven) || cashGiven < _totalAmount)
                {
                    MessageBox.Show("Số tiền khách đưa chưa đủ hoặc không hợp lệ!", "Lỗi thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal change = cashGiven - _totalAmount;

                MessageBox.Show($"In vé và thanh toán thành công!\nPhim: {cbMovies.Text}\nSuất: {cbShowtimes.Text}\nTiền thừa trả khách: {change:N0} VNĐ", 
                                  "Hoàn tất giao dịch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                txtCashGiven.Clear();
                lblChangeAmount.Text = "0 VNĐ";
                _selectedCombos.Clear();
                RecalculateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi giao dịch: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
using System;
using System.Drawing;
using System.Windows.Forms;
using CinemaManagement.Models;
using CinemaManagement.Services;

namespace CinemaManagement.Forms.Admin
{
    public partial class FrmAdminCombos : Form
    {
        private readonly ComboService _comboService;
        private int _selectedComboId = 0;
        
        private DataGridView dgvCombos;
        private TextBox txtName;
        private TextBox txtPrice;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Label lblName;
        private Label lblPrice;
        private Label lblTitle;

        public FrmAdminCombos()
        {
            _comboService = new ComboService();
            
            SetupComponents();
            LoadComboData();
        }

        private void SetupComponents()
        {
            this.Text = "Quản lý Combo Bắp Nước - FrmAdminCombos";
            this.Size = new System.Drawing.Size(950, 580);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30); 

            lblTitle = new Label();
            lblTitle.Text = "QUẢN LÝ DANH MỤC COMBO & F&B";
            lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new System.Drawing.Point(20, 20);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            dgvCombos = new DataGridView();
            dgvCombos.Location = new System.Drawing.Point(20, 70);
            dgvCombos.Size = new System.Drawing.Size(560, 440);
            dgvCombos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCombos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCombos.ReadOnly = true;
            dgvCombos.CellClick += new DataGridViewCellEventHandler(this.dgvCombos_CellClick);
            this.Controls.Add(dgvCombos);

            lblName = new Label();
            lblName.Text = "Tên Combo:";
            lblName.ForeColor = Color.White;
            lblName.Location = new System.Drawing.Point(600, 70);
            lblName.AutoSize = true;
            this.Controls.Add(lblName);

            txtName = new TextBox();
            txtName.Location = new System.Drawing.Point(600, 100);
            txtName.Size = new System.Drawing.Size(300, 25);
            this.Controls.Add(txtName);

            lblPrice = new Label();
            lblPrice.Text = "Giá Niêm Yết (VNĐ):";
            lblPrice.ForeColor = Color.White;
            lblPrice.Location = new System.Drawing.Point(600, 150);
            lblPrice.AutoSize = true;
            this.Controls.Add(lblPrice);

            txtPrice = new TextBox();
            txtPrice.Location = new System.Drawing.Point(600, 180);
            txtPrice.Size = new System.Drawing.Size(300, 25);
            this.Controls.Add(txtPrice);

            btnAdd = new Button();
            btnAdd.Text = "+ Thêm Combo";
            btnAdd.BackColor = Color.SeaGreen;
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Location = new System.Drawing.Point(600, 240);
            btnAdd.Size = new System.Drawing.Size(300, 40);
            btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.Controls.Add(btnAdd);

            btnUpdate = new Button();
            btnUpdate.Text = "Cập Nhật Thông Tin";
            btnUpdate.BackColor = Color.DodgerBlue;
            btnUpdate.ForeColor = Color.White;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Location = new System.Drawing.Point(600, 300);
            btnUpdate.Size = new System.Drawing.Size(300, 40);
            btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            this.Controls.Add(btnUpdate);

            btnDelete = new Button();
            btnDelete.Text = "Xóa Combo";
            btnDelete.BackColor = Color.Crimson;
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Location = new System.Drawing.Point(600, 360);
            btnDelete.Size = new System.Drawing.Size(300, 40);
            btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.Controls.Add(btnDelete);
        }

        private void LoadComboData()
        {
            try
            {
                dgvCombos.DataSource = _comboService.GetAllCombos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu combo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) || !decimal.TryParse(txtPrice.Text, out decimal price))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ tên và giá tiền hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var newCombo = new ComboItem
                {
                    ComboName = txtName.Text.Trim(),
                    Price = price
                };

                _comboService.AddCombo(newCombo);
                MessageBox.Show("Thêm combo thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadComboData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedComboId == 0)
                {
                    MessageBox.Show("Vui lòng chọn combo cần sửa từ bảng danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtName.Text) || !decimal.TryParse(txtPrice.Text, out decimal price))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ tên và giá tiền hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var combo = new ComboItem
                {
                    ComboId = _selectedComboId,
                    ComboName = txtName.Text.Trim(),
                    Price = price
                };

                _comboService.UpdateCombo(combo);
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadComboData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedComboId == 0)
                {
                    MessageBox.Show("Vui lòng chọn combo cần xóa từ bảng danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa combo này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    _comboService.DeleteCombo(_selectedComboId);
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadComboData();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCombos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvCombos.Rows[e.RowIndex];
                _selectedComboId = Convert.ToInt32(row.Cells["ComboId"].Value);
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtPrice.Text = row.Cells["Price"].Value.ToString();
            }
        }

        private void ClearInputs()
        {
            _selectedComboId = 0;
            txtName.Clear();
            txtPrice.Clear();
        }
    }
}
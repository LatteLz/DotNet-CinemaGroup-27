using System;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.Forms.Admin
{
    public partial class FrmAdminMovies : Form
    {
        private DataGridView dgvMovies = null!;
        private TextBox txtSearch = null!;
        private Button btnAdd = null!;
        private Button btnEdit = null!;
        private Button btnDelete = null!;
        private Button btnBack = null!;

        public FrmAdminMovies()
        {
            InitializeComponent();
            SetupAdminMoviesUi();
        }

        private void SetupAdminMoviesUi()
        {
            this.Text = "Admin - Quản lý Danh mục Phim";
            this.Size = new Size(1366, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(16, 20, 24); 

            Label lblTitle = new Label();
            lblTitle.Text = "Quản lý Danh mục Phim";
            lblTitle.ForeColor = Color.White;
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.Location = new Point(40, 30);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            txtSearch = new TextBox();
            txtSearch.PlaceholderText = "Nhập tên phim cần tìm...";
            txtSearch.Size = new Size(300, 30);
            txtSearch.Location = new Point(40, 90);
            txtSearch.Font = new Font("Segoe UI", 11);
            this.Controls.Add(txtSearch);

            btnAdd = CreateActionButton("Thêm Phim mới", Color.FromArgb(2, 132, 199), 360, 90);
            btnAdd.Click += BtnAdd_Click;

            btnEdit = CreateActionButton("Chỉnh sửa", Color.FromArgb(217, 119, 6), 510, 90);
            btnEdit.Click += BtnEdit_Click;

            btnDelete = CreateActionButton("Xóa phim", Color.FromArgb(220, 38, 38), 660, 90);
            btnDelete.Click += BtnDelete_Click;

            btnBack = CreateActionButton("← Quay lại Dashboard", Color.FromArgb(51, 65, 85), 1100, 90);
            btnBack.Click += (s, e) => this.Close();

            dgvMovies = new DataGridView();
            dgvMovies.Location = new Point(40, 150);
            dgvMovies.Size = new Size(1270, 530);
            dgvMovies.BackgroundColor = Color.FromArgb(24, 28, 32);
            dgvMovies.BorderStyle = BorderStyle.None;
            dgvMovies.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMovies.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMovies.ReadOnly = true;
            dgvMovies.RowHeadersVisible = false;
            
            dgvMovies.EnableHeadersVisualStyles = false;
            dgvMovies.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 45, 55);
            dgvMovies.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMovies.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvMovies.DefaultCellStyle.BackColor = Color.FromArgb(24, 28, 32);
            dgvMovies.DefaultCellStyle.ForeColor = Color.FromArgb(241, 245, 249);
            dgvMovies.DefaultCellStyle.SelectionBackColor = Color.FromArgb(2, 132, 199);
            dgvMovies.DefaultCellStyle.SelectionForeColor = Color.White;

            this.Controls.Add(dgvMovies);
        }

        private Button CreateActionButton(string text, Color bgColor, int x, int y)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.BackColor = bgColor;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Size = new Size(135, 38);
            btn.Location = new Point(x, y);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            this.Controls.Add(btn);
            return btn;
        }

        private void BtnAdd_Click(object? sender, EventArgs? e)
        {
            MessageBox.Show("Chức năng thêm phim đang được phát triển!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEdit_Click(object? sender, EventArgs? e)
        {
            MessageBox.Show("Chức năng sửa thông tin phim đang được phát triển!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDelete_Click(object? sender, EventArgs? e)
        {
            MessageBox.Show("Chức năng xóa phim đang được phát triển!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
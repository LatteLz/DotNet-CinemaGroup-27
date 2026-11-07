using System;
using System.Drawing;
using System.Windows.Forms;
using CinemaManagement.Services;
using CinemaManagement.Models;

namespace CinemaManagement.Forms.Admin
{
    public partial class FrmAdminUsers : Form
    {
        private readonly AuthService _authService;
        
        private Label lblTitle;
        private DataGridView dgvUsers;
        private Button btnAddUser;
        private Button btnDeleteUser;

        public FrmAdminUsers()
        {
            InitializeComponent(); 
            _authService = new AuthService();
            SetupComponents();
            LoadUserData();
        }

        private void SetupComponents()
        {
            this.Text = "Quản Lý Tài Khoản - FrmAdminUsers";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(24, 24, 24);

            lblTitle = new Label();
            lblTitle.Text = "QUẢN LÝ TÀI KHOẢN NGUỜI DÙNG";
            lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 20);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            dgvUsers = new DataGridView();
            dgvUsers.Location = new Point(20, 70);
            dgvUsers.Size = new Size(840, 420);
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Controls.Add(dgvUsers);

            btnAddUser = new Button();
            btnAddUser.Text = "+ Thêm Tài Khoản";
            btnAddUser.BackColor = Color.ForestGreen;
            btnAddUser.ForeColor = Color.White;
            btnAddUser.Location = new Point(20, 505);
            btnAddUser.Size = new Size(160, 40);
            this.Controls.Add(btnAddUser);

            btnDeleteUser = new Button();
            btnDeleteUser.Text = "Xóa Tài Khoản";
            btnDeleteUser.BackColor = Color.Crimson;
            btnDeleteUser.ForeColor = Color.White;
            btnDeleteUser.Location = new Point(190, 505);
            btnDeleteUser.Size = new Size(140, 40);
            this.Controls.Add(btnDeleteUser);
        }

        private void LoadUserData()
        {
            try
            {
   
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }
    }
}
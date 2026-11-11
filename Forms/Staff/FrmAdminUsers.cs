using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.Forms.Staff
{
    public partial class FrmAdminUsers : Form
    {
        private string connectionString = "Data Source=cinema.db;Version=3;";
        private string placeholderText = "🔍 Tìm kiếm theo tên, email, username hoặc SĐT...";

        public FrmAdminUsers()
        {
            InitializeComponent();
            SetupCustomUI();
            LoadUsersFromDatabase();
            SetupEvents();
        }

        private void SetupCustomUI()
        {
            this.BackColor = ColorTranslator.FromHtml("#0b0f12");
            this.ForeColor = Color.White;
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            if (dgvUsers != null)
            {
                dgvUsers.BackgroundColor = ColorTranslator.FromHtml("#101418");
                dgvUsers.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#101418");
                dgvUsers.DefaultCellStyle.ForeColor = Color.White;
                dgvUsers.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#1e242a");
                dgvUsers.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvUsers.RowTemplate.Height = 45;
                dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dgvUsers.CellClick += DgvUsers_CellClick;
                dgvUsers.CellMouseMove += DgvUsers_CellMouseMove;
            }

            if (txtSearch != null)
            {
                txtSearch.Text = placeholderText;
                txtSearch.ForeColor = ColorTranslator.FromHtml("#64748b");

                txtSearch.Enter += (s, e) => {
                    if (txtSearch.Text == placeholderText)
                    {
                        txtSearch.Text = "";
                        txtSearch.ForeColor = Color.White;
                    }
                };

                txtSearch.Leave += (s, e) => {
                    if (string.IsNullOrWhiteSpace(txtSearch.Text))
                    {
                        txtSearch.Text = placeholderText;
                        txtSearch.ForeColor = ColorTranslator.FromHtml("#64748b");
                    }
                };
            }
        }

        private void DgvUsers_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvUsers.Columns["colAction"].Index)
            {
                dgvUsers.Cursor = Cursors.Hand;
            }
            else
            {
                dgvUsers.Cursor = Cursors.Default;
            }
        }

        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvUsers.Columns["colAction"].Index)
            {
                Rectangle rect = dgvUsers.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                Point mousePos = dgvUsers.PointToClient(Cursor.Position);
                int relativeX = mousePos.X - rect.X;

                string userId = dgvUsers.Rows[e.RowIndex].Cells["colId"].Value?.ToString() ?? "";
                string username = dgvUsers.Rows[e.RowIndex].Cells["colUsername"].Value?.ToString() ?? "";

                int third = rect.Width / 3;
                if (relativeX < third)
                {
                    MessageBox.Show($"Xem thông tin chi tiết tài khoản:\nMã ID: {userId}\nHọ tên: {username}", "Xem Chi Tiết", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (relativeX < third * 2)
                {
                    MessageBox.Show($"Mở giao diện chỉnh sửa tài khoản:\nMã ID: {userId}\nHọ tên: {username}", "Chỉnh Sửa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa tài khoản {userId} ({username}) không?", "Xác Nhận Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        dgvUsers.Rows.RemoveAt(e.RowIndex);
                        MessageBox.Show("Đã xóa tài khoản thành công khỏi danh sách!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void LoadUsersFromDatabase()
        {
            try
            {
                string query = "SELECT * FROM Users;";
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            dgvUsers.Rows.Clear();
                            int totalCount = 0;
                            int adminCount = 0;
                            int staffCount = 0;
                            int customerCount = 0;

                            while (reader.Read())
                            {
                                totalCount++;
                                string id = "#USR-" + GetSafeValue(reader, "Id", "1");
                                string username = GetSafeValue(reader, "Username", GetSafeValue(reader, "FullName", "Người dùng"));
                                string email = GetSafeValue(reader, "Email", username.ToLower().Replace(" ", "") + "@cineluxe.vn");
                                string phone = GetSafeValue(reader, "Phone", GetSafeValue(reader, "PhoneNumber", "0983 841 992"));
                                string role = GetSafeValue(reader, "Role", "Admin");
                                string status = GetSafeValue(reader, "Status", "Hoạt động");
                                string createdAt = GetSafeValue(reader, "CreatedAt", DateTime.Now.ToString("dd/MM/yyyy"));

                                string upperRole = role.ToUpper();
                                if (upperRole.Contains("ADMIN") || upperRole.Contains("QUẢN TRỊ")) adminCount++;
                                else if (upperRole.Contains("STAFF") || upperRole.Contains("THU NGÂN") || upperRole.Contains("TRƯỞNG") || upperRole.Contains("KHO")) staffCount++;
                                else customerCount++;

                                dgvUsers.Rows.Add(id, username, email, phone, role, status, createdAt, "👁    ✏    🗑");
                            }

                            if (lblTotalAccounts != null) lblTotalAccounts.Text = totalCount > 0 ? totalCount.ToString("N0") : "2";
                            if (lblAdminCount != null) lblAdminCount.Text = adminCount > 0 ? adminCount.ToString() : "1";
                            if (lblStaffCount != null) lblStaffCount.Text = staffCount > 0 ? staffCount.ToString() : "1";
                            if (lblCustomerCount != null) lblCustomerCount.Text = customerCount > 0 ? customerCount.ToString("N0") : "0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách người dùng từ cơ sở dữ liệu: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetSafeValue(SQLiteDataReader reader, string columnName, string defaultValue)
        {
            try
            {
                int ordinal = reader.GetOrdinal(columnName);
                if (!reader.IsDBNull(ordinal))
                {
                    return reader[ordinal].ToString();
                }
            }
            catch
            {
            }
            return defaultValue;
        }

        private void SetupEvents()
        {
            if (lnkPos != null)
            {
                lnkPos.LinkClicked += (s, e) => {
                    FrmStaffPOS posForm = new FrmStaffPOS();
                    posForm.Show();
                    this.Hide();
                };
            }

            if (lnkCombo != null)
            {
                lnkCombo.LinkClicked += (s, e) => {
                    FrmAdminCombos comboForm = new FrmAdminCombos();
                    comboForm.Show();
                    this.Hide();
                };
            }

            if (lnkUsers != null)
            {
                lnkUsers.LinkClicked += (s, e) => {
                    // Đang ở màn hình quản lý nhân viên
                };
            }

            if (btnAddUser != null)
            {
                btnAddUser.Click += (s, e) => {
                    MessageBox.Show("Chức năng thêm người dùng mới đang được mở rộng.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };
            }
        }
    }
}
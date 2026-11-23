using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using CinemaManagement.Forms.Staff;

namespace CinemaManagement.Forms.Staff
{
    public partial class FrmAdminUsers : Form
    {
        private string connectionString = "Data Source=cinema.db;Version=3;";
        private string placeholderText = "🔍 Tìm kiếm theo tên, email, username hoặc SĐT...";
        private ErrorProvider errorProvider = new ErrorProvider();

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
                        errorProvider.SetError(txtSearch, string.Empty);
                    }
                    else
                    {
                        if (txtSearch.Text.Length < 2)
                        {
                            errorProvider.SetError(txtSearch, "Từ khóa tìm kiếm phải có ít nhất 2 ký tự!");
                        }
                        else
                        {
                            errorProvider.SetError(txtSearch, string.Empty);
                        }
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
                    errorProvider.Clear();
                    MessageBox.Show($"Xem thông tin chi tiết tài khoản:\nMã ID: {userId}\nHọ tên: {username}", "Xem Chi Tiết", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (relativeX < third * 2)
                {
                    errorProvider.Clear();
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
                                string username = GetSafeValue(reader, "Username", GetSafeValue(reader, "Fullname", GetSafeValue(reader, "FullName", "Người dùng")));
                                string email = GetSafeValue(reader, "Email", username.ToLower().Replace(" ", "") + "@cineluxe.vn");
                                string phone = GetSafeValue(reader, "Phone", GetSafeValue(reader, "PhoneNumber", "0983 841 992"));
                                string role = GetSafeValue(reader, "Role", "Admin");
                                string status = GetSafeValue(reader, "Status", "Hoạt động");
                                string createdAt = GetSafeValue(reader, "CreateAt", GetSafeValue(reader, "CreatedAt", DateTime.Now.ToString("dd/MM/yyyy")));

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

        private class UserColumnMeta
        {
            public string Name { get; set; } = "";
            public string Type { get; set; } = "";
            public bool NotNull { get; set; }
            public bool HasDefault { get; set; }
            public bool IsPrimaryKey { get; set; }
        }

        private static (string PasswordHash, string Salt) CreatePasswordHash(string password)
        {
            byte[] saltBytes = new byte[16];
            RandomNumberGenerator.Fill(saltBytes);
            string saltBase64 = Convert.ToBase64String(saltBytes);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256);
            string hashBase64 = Convert.ToBase64String(pbkdf2.GetBytes(32));
            return (hashBase64, saltBase64);
        }

        private bool ShowAddUserDialog(
            out string username,
            out string email,
            out string phone,
            out string role,
            out string status,
            out string password)
        {
            username = "";
            email = "";
            phone = "";
            role = "Staff";
            status = "Hoạt động";
            password = "";

            using (Form dlg = new Form())
            {
                dlg.Text = "Thêm Người Dùng Mới";
                dlg.Size = new Size(440, 415);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.BackColor = ColorTranslator.FromHtml("#181c20");
                dlg.ForeColor = Color.White;

                Color inputBg = ColorTranslator.FromHtml("#101418");
                Color muted = ColorTranslator.FromHtml("#94a3b8");
                Font lblFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);

                Label lblUser = new Label { Text = "Tên tài khoản / Họ tên:", Location = new Point(20, 14), Size = new Size(385, 18), Font = lblFont, ForeColor = muted };
                TextBox txtUser = new TextBox { Location = new Point(20, 34), Size = new Size(385, 25), BackColor = inputBg, ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Text = "staff_new" };

                Label lblEmail = new Label { Text = "Email liên hệ:", Location = new Point(20, 66), Size = new Size(385, 18), Font = lblFont, ForeColor = muted };
                TextBox txtEmail = new TextBox { Location = new Point(20, 86), Size = new Size(385, 25), BackColor = inputBg, ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Text = "staff_new@cineluxe.vn" };

                Label lblPhone = new Label { Text = "Số điện thoại:", Location = new Point(20, 118), Size = new Size(385, 18), Font = lblFont, ForeColor = muted };
                TextBox txtPhone = new TextBox { Location = new Point(20, 138), Size = new Size(385, 25), BackColor = inputBg, ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Text = "0983 841 992" };

                Label lblRole = new Label { Text = "Vai trò / Nhiệm vụ:", Location = new Point(20, 170), Size = new Size(185, 18), Font = lblFont, ForeColor = muted };
                ComboBox cboRole = new ComboBox
                {
                    Location = new Point(20, 190),
                    Size = new Size(185, 26),
                    BackColor = inputBg,
                    ForeColor = Color.White,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboRole.Items.AddRange(new object[] { "Staff", "Admin", "Customer" });
                cboRole.SelectedIndex = 0;

                Label lblStatus = new Label { Text = "Trạng thái:", Location = new Point(220, 170), Size = new Size(185, 18), Font = lblFont, ForeColor = muted };
                ComboBox cboStatus = new ComboBox
                {
                    Location = new Point(220, 190),
                    Size = new Size(185, 26),
                    BackColor = inputBg,
                    ForeColor = Color.White,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cboStatus.Items.AddRange(new object[] { "Hoạt động", "Tạm khóa" });
                cboStatus.SelectedIndex = 0;

                Label lblPass = new Label { Text = "Mật khẩu:", Location = new Point(20, 224), Size = new Size(385, 18), Font = lblFont, ForeColor = muted };
                TextBox txtPass = new TextBox { Location = new Point(20, 244), Size = new Size(385, 25), BackColor = inputBg, ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, UseSystemPasswordChar = true, Text = "123456" };

                Button btnSave = new Button
                {
                    Text = "Lưu vào CSDL",
                    DialogResult = DialogResult.OK,
                    Location = new Point(205, 315),
                    Size = new Size(115, 34),
                    BackColor = ColorTranslator.FromHtml("#0284c7"),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                };
                btnSave.FlatAppearance.BorderSize = 0;

                Button btnCancel = new Button
                {
                    Text = "Hủy",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(328, 315),
                    Size = new Size(77, 34),
                    BackColor = ColorTranslator.FromHtml("#27303a"),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                };
                btnCancel.FlatAppearance.BorderSize = 0;

                dlg.Controls.AddRange(new Control[]
                {
                    lblUser, txtUser,
                    lblEmail, txtEmail,
                    lblPhone, txtPhone,
                    lblRole, cboRole,
                    lblStatus, cboStatus,
                    lblPass, txtPass,
                    btnSave, btnCancel
                });
                dlg.AcceptButton = btnSave;
                dlg.CancelButton = btnCancel;

                while (true)
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK)
                    {
                        return false;
                    }

                    string u = txtUser.Text.Trim();
                    string em = txtEmail.Text.Trim();
                    string ph = txtPhone.Text.Trim();
                    string rl = cboRole.SelectedItem?.ToString() ?? "Staff";
                    string st = cboStatus.SelectedItem?.ToString() ?? "Hoạt động";
                    string pw = string.IsNullOrWhiteSpace(txtPass.Text) ? "123456" : txtPass.Text;

                    if (string.IsNullOrWhiteSpace(u))
                    {
                        MessageBox.Show("Tên tài khoản không được để trống!", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtUser.Focus();
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(em))
                    {
                        em = u.ToLower().Replace(" ", "") + "@cineluxe.vn";
                    }

                    username = u;
                    email = em;
                    phone = ph;
                    role = rl;
                    status = st;
                    password = pw;
                    return true;
                }
            }
        }

        private void InsertUserIntoSqlite(
            string username,
            string email,
            string phone,
            string role,
            string status,
            string password)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var createCmd = new SQLiteCommand(@"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL,
                        PasswordHash TEXT NOT NULL,
                        Fullname TEXT,
                        Role TEXT DEFAULT 'Staff',
                        Salt TEXT,
                        IsActive INTEGER DEFAULT 1,
                        CreateAt TEXT
                    );", conn))
                {
                    createCmd.ExecuteNonQuery();
                }

                var cols = new List<UserColumnMeta>();
                using (var pragmaCmd = new SQLiteCommand("PRAGMA table_info(Users);", conn))
                using (var reader = pragmaCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cols.Add(new UserColumnMeta
                        {
                            Name = reader["name"]?.ToString() ?? "",
                            Type = reader["type"]?.ToString() ?? "",
                            NotNull = reader["notnull"] != DBNull.Value && Convert.ToInt32(reader["notnull"]) != 0,
                            HasDefault = reader["dflt_value"] != DBNull.Value,
                            IsPrimaryKey = reader["pk"] != DBNull.Value && Convert.ToInt32(reader["pk"]) != 0
                        });
                    }
                }

                var (hashVal, saltVal) = CreatePasswordHash(password);
                string dateStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                int isActiveVal = status.Contains("khóa", StringComparison.OrdinalIgnoreCase) ? 0 : 1;

                var insertCols = new List<string>();
                var insertParams = new List<string>();
                using (var cmd = new SQLiteCommand(conn))
                {
                    foreach (var col in cols)
                    {
                        if (col.IsPrimaryKey && col.Type.IndexOf("INT", StringComparison.OrdinalIgnoreCase) >= 0)
                            continue;

                        string lower = col.Name.ToLowerInvariant();
                        object? val = null;

                        if (lower == "username")
                            val = username;
                        else if (lower == "fullname" || lower == "name")
                            val = username;
                        else if (lower == "passwordhash" || lower == "password")
                            val = hashVal;
                        else if (lower == "salt")
                            val = saltVal;
                        else if (lower == "isactive")
                            val = isActiveVal;
                        else if (lower == "email")
                            val = email;
                        else if (lower == "phone" || lower == "phonenumber" || lower == "sdt")
                            val = phone;
                        else if (lower == "role" || lower == "position")
                            val = role;
                        else if (lower == "status")
                            val = status;
                        else if (lower == "createat" || lower == "createdat" || lower == "datecreated")
                            val = dateStr;
                        else if (col.NotNull && !col.HasDefault)
                        {
                            val = col.Type.IndexOf("INT", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                  col.Type.IndexOf("DEC", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                  col.Type.IndexOf("NUM", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                  col.Type.IndexOf("REAL", StringComparison.OrdinalIgnoreCase) >= 0
                                ? 0
                                : "";
                        }

                        if (val != null)
                        {
                            string paramName = "@" + col.Name;
                            insertCols.Add(col.Name);
                            insertParams.Add(paramName);
                            cmd.Parameters.AddWithValue(paramName, val);
                        }
                    }

                    if (insertCols.Count > 0)
                    {
                        cmd.CommandText = $"INSERT INTO Users ({string.Join(", ", insertCols)}) VALUES ({string.Join(", ", insertParams)});";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
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

            if (lnkReports != null)
            {
                lnkReports.LinkClicked += (s, e) => {
                    FrmAdminReports reportsForm = new FrmAdminReports();
                    reportsForm.Show();
                    this.Hide();
                };
            }

            if (lnkOccupancy != null)
            {
                lnkOccupancy.LinkClicked += (s, e) => {
                    FrmOccupancyReport occupancyForm = new FrmOccupancyReport();
                    occupancyForm.Show();
                    this.Hide();
                };
            }

            if (lnkUsers != null)
            {
                lnkUsers.LinkClicked += (s, e) => {
                };
            }

            if (btnAddUser != null)
            {
                btnAddUser.Click += (s, e) => {
                    if (!ShowAddUserDialog(
                        out string username,
                        out string email,
                        out string phone,
                        out string role,
                        out string status,
                        out string password))
                    {
                        return;
                    }

                    errorProvider.SetError(btnAddUser, string.Empty);

                    try
                    {
                        InsertUserIntoSqlite(username, email, phone, role, status, password);
                        LoadUsersFromDatabase();
                        MessageBox.Show("Thêm người dùng mới thành công vào cơ sở dữ liệu!", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thêm người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
            }
        }
    }
}
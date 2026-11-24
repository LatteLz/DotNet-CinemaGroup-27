using CinemaManagement.Forms.Shared;
using CinemaManagement.Helpers;
using CinemaManagement.Models;
using CinemaManagement.Services;

namespace CinemaManagement.Forms.Staff
{
    public partial class FrmAdminUsers : Form
    {
        private readonly AuthService _authService = new();
        private List<User> _users = new();
        private string placeholderText = "🔍 Tìm kiếm theo tên, email, username hoặc SĐT...";
        private ErrorProvider errorProvider = new ErrorProvider();

        public FrmAdminUsers()
        {
            InitializeComponent();
            AppSession.RequireAdmin(this);
            SetupCustomUI();
            LoadUsersFromDatabase();
            SetupEvents();
            WindowLayout.ScaleToStandard(this);
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

                txtSearch.TextChanged += (s, e) => RenderGrid();

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
            if (e.RowIndex < 0 || e.ColumnIndex != dgvUsers.Columns["colAction"].Index) return;
            if (dgvUsers.Rows[e.RowIndex].Tag is not int userId) return;

            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return;

            Rectangle rect = dgvUsers.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
            Point mousePos = dgvUsers.PointToClient(Cursor.Position);
            int relativeX = mousePos.X - rect.X;
            int third = rect.Width / 3;
            errorProvider.Clear();

            try
            {
                if (relativeX < third)
                {
                    MessageBox.Show(
                        $"Mã: #{user.Id}\nTên đăng nhập: {user.Username}\nHọ tên: {user.Fullname}\n" +
                        $"Email: {user.Email ?? "-"}\nSĐT: {user.Phone ?? "-"}\nVai trò: {user.Role}\n" +
                        $"Trạng thái: {(user.IsActive ? "Hoạt động" : "Đã khóa")}\nNgày tạo: {user.CreateAt:dd/MM/yyyy HH:mm}",
                        "Thông tin tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (relativeX < third * 2)
                {
                    var result = ShowUserDialog(user);
                    if (result == null) return;

                    _authService.UpdateUser(user.Id, result.Fullname, result.Email, result.Phone, result.Role, result.IsActive);
                    if (!string.IsNullOrEmpty(result.Password))
                        _authService.ResetPassword(user.Id, result.Password);

                    LoadUsersFromDatabase();
                    MessageBox.Show("Đã cập nhật tài khoản.", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (MessageBox.Show($"Bạn có chắc chắn muốn xóa tài khoản '{user.Username}' không?", "Xác Nhận Xóa",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                        return;

                    _authService.DeleteUser(user.Id);
                    LoadUsersFromDatabase();
                    MessageBox.Show("Đã xóa tài khoản khỏi cơ sở dữ liệu.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (BusinessException ex)
            {
                MessageBox.Show(ex.Message, "Không thể thực hiện", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUsersFromDatabase()
        {
            try
            {
                _users = _authService.GetAllUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách người dùng từ cơ sở dữ liệu: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _users = new List<User>();
            }

            if (lblTotalAccounts != null) lblTotalAccounts.Text = _users.Count.ToString("N0");
            if (lblAdminCount != null) lblAdminCount.Text = _users.Count(u => u.Role == Roles.Admin).ToString();
            if (lblStaffCount != null) lblStaffCount.Text = _users.Count(u => u.Role == Roles.Staff).ToString();
            if (lblLockedCount != null) lblLockedCount.Text = _users.Count(u => !u.IsActive).ToString();

            RenderGrid();
        }

        private void RenderGrid()
        {
            if (dgvUsers == null) return;
            dgvUsers.Rows.Clear();

            string keyword = txtSearch == null || txtSearch.Text == placeholderText ? "" : txtSearch.Text.Trim();
            foreach (var u in _users.Where(u => keyword.Length < 2
                                                || u.Username.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                                || u.Fullname.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                                || (u.Email ?? "").Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                                || (u.Phone ?? "").Contains(keyword)))
            {
                int row = dgvUsers.Rows.Add(
                    $"#USR-{u.Id}",
                    $"{u.Fullname} ({u.Username})",
                    u.Email ?? "-",
                    u.Phone ?? "-",
                    u.Role,
                    u.IsActive ? "Hoạt động" : "Đã khóa",
                    u.CreateAt.ToString("dd/MM/yyyy"),
                    "👁    ✏    🗑");
                dgvUsers.Rows[row].Tag = u.Id;
            }
        }

        private sealed class UserDialogResult
        {
            public string Username { get; init; } = "";
            public string Fullname { get; init; } = "";
            public string? Email { get; init; }
            public string? Phone { get; init; }
            public string Role { get; init; } = Roles.Staff;
            public bool IsActive { get; init; } = true;
            public string Password { get; init; } = "";
        }

        private UserDialogResult? ShowUserDialog(User? existing)
        {
            bool isEdit = existing != null;
            using var dlg = new Form
            {
                Text = isEdit ? $"Sửa tài khoản: {existing!.Username}" : "Thêm Người Dùng Mới",
                Size = new Size(440, 470),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = ColorTranslator.FromHtml("#181c20"),
                ForeColor = Color.White
            };
            var dlgErrors = new ErrorProvider { ContainerControl = dlg };

            Color inputBg = ColorTranslator.FromHtml("#101418");
            Color muted = ColorTranslator.FromHtml("#94a3b8");
            Font lblFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);

            Label L(string text, int x, int y, int w = 385) => new Label { Text = text, Location = new Point(x, y), Size = new Size(w, 18), Font = lblFont, ForeColor = muted };
            TextBox T(int x, int y, string text, int w = 385) => new TextBox { Location = new Point(x, y), Size = new Size(w, 25), BackColor = inputBg, ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Text = text };

            var txtUser = T(20, 34, existing?.Username ?? "");
            txtUser.ReadOnly = isEdit;
            var txtFullname = T(20, 86, existing?.Fullname ?? "");
            var txtEmail = T(20, 138, existing?.Email ?? "");
            var txtPhone = T(20, 190, existing?.Phone ?? "");

            var cboRole = new ComboBox { Location = new Point(20, 242), Size = new Size(185, 26), BackColor = inputBg, ForeColor = Color.White, DropDownStyle = ComboBoxStyle.DropDownList };
            cboRole.Items.AddRange(new object[] { Roles.Staff, Roles.Admin });
            cboRole.SelectedItem = existing?.Role == Roles.Admin ? Roles.Admin : Roles.Staff;

            var cboStatus = new ComboBox { Location = new Point(220, 242), Size = new Size(185, 26), BackColor = inputBg, ForeColor = Color.White, DropDownStyle = ComboBoxStyle.DropDownList };
            cboStatus.Items.AddRange(new object[] { "Hoạt động", "Tạm khóa" });
            cboStatus.SelectedIndex = existing == null || existing.IsActive ? 0 : 1;

            var txtPass = T(20, 296, "");
            txtPass.UseSystemPasswordChar = true;

            var btnSave = new Button { Text = "Lưu vào CSDL", Location = new Point(205, 370), Size = new Size(115, 34), BackColor = ColorTranslator.FromHtml("#0284c7"), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            var btnCancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Location = new Point(328, 370), Size = new Size(77, 34), BackColor = ColorTranslator.FromHtml("#27303a"), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            btnSave.FlatAppearance.BorderSize = 0;
            btnCancel.FlatAppearance.BorderSize = 0;

            dlg.Controls.AddRange(new Control[]
            {
                L("Tên đăng nhập:", 20, 14), txtUser,
                L("Họ và tên:", 20, 66), txtFullname,
                L("Email (không bắt buộc):", 20, 118), txtEmail,
                L("Số điện thoại (không bắt buộc):", 20, 170), txtPhone,
                L("Vai trò:", 20, 222, 185), cboRole,
                L("Trạng thái:", 220, 222, 185), cboStatus,
                L(isEdit ? "Mật khẩu mới (để trống nếu không đổi):" : "Mật khẩu (ít nhất 6 ký tự):", 20, 276), txtPass,
                btnSave, btnCancel
            });
            dlg.AcceptButton = btnSave;
            dlg.CancelButton = btnCancel;

            btnSave.Click += (_, _) =>
            {
                dlgErrors.Clear();
                bool ok = true;
                void Fail(Control c, string msg) { dlgErrors.SetError(c, msg); ok = false; }

                if (!isEdit && !ValidationHelper.IsValidUsername(txtUser.Text)) Fail(txtUser, "3-50 ký tự: chữ, số, . _ - @");
                if (string.IsNullOrWhiteSpace(txtFullname.Text)) Fail(txtFullname, "Họ tên không được để trống");
                if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !ValidationHelper.IsValidEmail(txtEmail.Text)) Fail(txtEmail, "Email không đúng định dạng");
                if (!string.IsNullOrWhiteSpace(txtPhone.Text) && !ValidationHelper.IsValidPhoneNumber(txtPhone.Text)) Fail(txtPhone, "SĐT 10 số, bắt đầu 03/05/07/08/09");
                bool needPassword = !isEdit || txtPass.Text.Length > 0;
                if (needPassword && !ValidationHelper.IsStrongPassword(txtPass.Text)) Fail(txtPass, "Mật khẩu ít nhất 6 ký tự");

                if (ok) dlg.DialogResult = DialogResult.OK;
            };

            if (dlg.ShowDialog(this) != DialogResult.OK) return null;

            return new UserDialogResult
            {
                Username = txtUser.Text.Trim(),
                Fullname = txtFullname.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Role = cboRole.SelectedItem?.ToString() ?? Roles.Staff,
                IsActive = cboStatus.SelectedIndex == 0,
                Password = txtPass.Text
            };
        }

        private void SetupEvents()
        {
            if (lnkPos != null) lnkPos.LinkClicked += (s, e) => StaffNavigator.Navigate<FrmStaffPOS>(this);
            if (lnkCombo != null) lnkCombo.LinkClicked += (s, e) => StaffNavigator.Navigate<FrmAdminCombos>(this);
            if (lnkReports != null) lnkReports.LinkClicked += (s, e) => StaffNavigator.Navigate<FrmAdminReports>(this);
            if (lnkOccupancy != null) lnkOccupancy.LinkClicked += (s, e) => StaffNavigator.Navigate<FrmOccupancyReport>(this);

            if (btnAddUser != null)
            {
                btnAddUser.Click += (s, e) =>
                {
                    var result = ShowUserDialog(null);
                    if (result == null) return;

                    try
                    {
                        _authService.AddUser(result.Username, result.Password, result.Fullname, result.Email, result.Phone, result.Role, result.IsActive);
                        LoadUsersFromDatabase();
                        MessageBox.Show("Thêm người dùng mới thành công!", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (BusinessException ex)
                    {
                        MessageBox.Show(ex.Message, "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

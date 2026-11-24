using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Services;
using CinemaManagement.Helpers;
using CinemaManagement.Forms.Staff;
using CinemaManagement.Forms.Admin;

namespace CinemaManagement.Forms.Staff
{
    public partial class FrmLogin : Form
    {
        private readonly AuthService _authService;
        private readonly ErrorProvider _errors = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        private readonly Color COLOR_CANVAS = ColorTranslator.FromHtml("#101418");
        private readonly Color COLOR_CARD_BG = ColorTranslator.FromHtml("#181C20");
        private readonly Color COLOR_INPUT_BG = ColorTranslator.FromHtml("#22262B");
        private readonly Color COLOR_BORDER = ColorTranslator.FromHtml("#2C3238");
        private readonly Color COLOR_AMBER = ColorTranslator.FromHtml("#F59E0B");
        private readonly Color COLOR_AMBER_HOVER = ColorTranslator.FromHtml("#FBBF24");
        private readonly Color COLOR_TEXT_PRIMARY = ColorTranslator.FromHtml("#FFFFFF");
        private readonly Color COLOR_TEXT_SECONDARY = ColorTranslator.FromHtml("#94A3B8");
        private readonly Color COLOR_TEXT_MUTED = ColorTranslator.FromHtml("#64748B");

        private Panel pnlCard;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblUsername;
        private Label lblUsernameHint;
        private TextBox txtUsername;
        private Label lblPassword;
        private Label lblForgotPassword;
        private TextBox txtPassword;
        private Button btnShowPassword;
        private Button btnLogin;
        private Label lblQuickRoleTitle;
        private Button btnQuickAdmin;
        private Button btnQuickStaff;

        public FrmLogin()
        {
            InitializeComponent();
            _authService = new AuthService();
            SetupMasterLayout();
            BuildLoginCard();
            WindowLayout.ScaleToStandard(this);
        }

        private void SetupMasterLayout()
        {
            this.Text = "CINELUXE MANAGEMENT";
            this.Size = new Size(1440, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = COLOR_CANVAS;
        }

        private void BuildLoginCard()
        {
            pnlCard = new Panel
            {
                Size = new Size(460, 580),
                BackColor = COLOR_CARD_BG
            };
            pnlCard.Location = new Point((this.ClientSize.Width - pnlCard.Width) / 2, (this.ClientSize.Height - pnlCard.Height) / 2);
            pnlCard.Anchor = AnchorStyles.None;

            Label lblIconCinema = new Label
            {
                Text = "🎬",
                Font = new Font("Segoe UI", 22),
                ForeColor = COLOR_AMBER,
                Location = new Point((pnlCard.Width - 50) / 2, 20),
                Size = new Size(50, 45),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblTitle = new Label
            {
                Text = "CINELUXE",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = COLOR_TEXT_PRIMARY,
                Location = new Point(20, 68),
                Size = new Size(420, 32),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblSubtitle = new Label
            {
                Text = "EXECUTIVE SUITE LOGIN",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = COLOR_TEXT_SECONDARY,
                Location = new Point(20, 102),
                Size = new Size(420, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblUsername = new Label
            {
                Text = "TÀI KHOẢN QUẢN TRỊ",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = COLOR_TEXT_SECONDARY,
                Location = new Point(35, 138),
                AutoSize = true
            };

            lblUsernameHint = new Label
            {
                Text = "MSSV / STAFF ID",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = COLOR_TEXT_MUTED,
                Location = new Point(325, 138),
                AutoSize = true
            };

            txtUsername = new TextBox
            {
                Text = "",
                Location = new Point(35, 162),
                Size = new Size(390, 32),
                Font = new Font("Segoe UI", 10.5f),
                BackColor = COLOR_INPUT_BG,
                ForeColor = COLOR_TEXT_PRIMARY,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblPassword = new Label
            {
                Text = "MẬT KHẨU TRUY CẬP",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = COLOR_TEXT_SECONDARY,
                Location = new Point(35, 215),
                AutoSize = true
            };

            lblForgotPassword = new Label
            {
                Text = "Quên mật khẩu?",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = COLOR_TEXT_MUTED,
                Location = new Point(330, 215),
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            lblForgotPassword.Click += (s, e) => MessageBox.Show("Vui lòng liên hệ IT Administrator để cấp lại mật khẩu hệ thống.", "Hỗ trợ", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtPassword = new TextBox
            {
                Text = "",
                Location = new Point(35, 238),
                Size = new Size(345, 32),
                Font = new Font("Segoe UI", 10.5f),
                BackColor = COLOR_INPUT_BG,
                ForeColor = COLOR_TEXT_PRIMARY,
                PasswordChar = '●',
                BorderStyle = BorderStyle.FixedSingle
            };

            btnShowPassword = new Button
            {
                Text = "👁",
                Location = new Point(382, 238),
                Size = new Size(43, 32),
                Font = new Font("Segoe UI", 10f),
                ForeColor = COLOR_TEXT_SECONDARY,
                BackColor = COLOR_INPUT_BG,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnShowPassword.FlatAppearance.BorderColor = COLOR_BORDER;
            bool isPasswordHidden = true;
            btnShowPassword.Click += (s, e) =>
            {
                isPasswordHidden = !isPasswordHidden;
                if (isPasswordHidden)
                {
                    txtPassword.PasswordChar = '●';
                    btnShowPassword.ForeColor = COLOR_TEXT_SECONDARY;
                }
                else
                {
                    txtPassword.PasswordChar = '\0';
                    btnShowPassword.ForeColor = COLOR_AMBER;
                }
            };

            btnLogin = new Button
            {
                Text = "ĐĂNG NHẬP ➔",
                Location = new Point(35, 332),
                Size = new Size(390, 46),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = COLOR_AMBER,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = COLOR_AMBER_HOVER;
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = COLOR_AMBER;
            btnLogin.Click += (s, e) => ExecuteLogin();

            this.AcceptButton = btnLogin;

            lblQuickRoleTitle = new Label
            {
                Text = "Vai trò truy cập nhanh:",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = COLOR_TEXT_MUTED,
                Location = new Point(35, 412),
                AutoSize = true
            };

            btnQuickAdmin = new Button
            {
                Text = "Quản lý",
                Location = new Point(185, 404),
                Size = new Size(110, 32),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = COLOR_TEXT_PRIMARY,
                BackColor = COLOR_INPUT_BG,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnQuickAdmin.FlatAppearance.BorderColor = COLOR_BORDER;
            btnQuickAdmin.Click += (s, e) => {
                txtUsername.Text = DbInitializer.DefaultAdminUsername;
                txtPassword.Text = DbInitializer.DefaultAdminPassword;
                ExecuteLogin();
            };

            btnQuickStaff = new Button
            {
                Text = "Thu ngân POS",
                Location = new Point(302, 404),
                Size = new Size(123, 32),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = COLOR_TEXT_PRIMARY,
                BackColor = COLOR_INPUT_BG,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnQuickStaff.FlatAppearance.BorderColor = COLOR_BORDER;
            btnQuickStaff.Click += (s, e) => {
                txtUsername.Text = DbInitializer.DefaultStaffUsername;
                txtPassword.Text = DbInitializer.DefaultStaffPassword;
                ExecuteLogin();
            };

            pnlCard.Controls.Add(btnQuickStaff);
            pnlCard.Controls.Add(btnQuickAdmin);
            pnlCard.Controls.Add(lblQuickRoleTitle);
            pnlCard.Controls.Add(btnLogin);
            pnlCard.Controls.Add(btnShowPassword);
            pnlCard.Controls.Add(txtPassword);
            pnlCard.Controls.Add(lblForgotPassword);
            pnlCard.Controls.Add(lblPassword);
            pnlCard.Controls.Add(txtUsername);
            pnlCard.Controls.Add(lblUsernameHint);
            pnlCard.Controls.Add(lblUsername);
            pnlCard.Controls.Add(lblSubtitle);
            pnlCard.Controls.Add(lblTitle);
            pnlCard.Controls.Add(lblIconCinema);

            this.Controls.Add(pnlCard);
        }

        private void ExecuteLogin()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            _errors.Clear();
            if (!ValidationHelper.IsNotEmpty(username))
                _errors.SetError(txtUsername, "Vui lòng nhập tên đăng nhập");
            else if (!ValidationHelper.IsValidUsername(username))
                _errors.SetError(txtUsername, "Tên đăng nhập 3-50 ký tự: chữ, số, . _ - @");
            if (!ValidationHelper.IsNotEmpty(password))
                _errors.SetError(txtPassword, "Vui lòng nhập mật khẩu");

            if (_errors.GetError(txtUsername).Length > 0 || _errors.GetError(txtPassword).Length > 0)
            {
                (_errors.GetError(txtUsername).Length > 0 ? txtUsername : txtPassword).Focus();
                return;
            }

            try
            {
                var user = _authService.Login(username, password, out string error);
                if (user == null)
                {
                    _errors.SetError(txtPassword, error);
                    MessageBox.Show(error, "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                AppSession.SignIn(user);
                MessageBox.Show($"Đăng nhập thành công!\nXin chào: {user.Fullname} ({user.Role})", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();

                Form mainForm = user.Role == Roles.Admin
                    ? new FrmAdminDashboard()
                    : new FrmStaffPOS();
                mainForm.ShowDialog();

                foreach (var f in Application.OpenForms.Cast<Form>()
                             .Where(f => f != this && f is not FrmRoleSelection && f.GetType().Namespace != typeof(Kiosk.KioskFlow).Namespace)
                             .ToList())
                {
                    f.Close();
                }
                AppSession.SignOut();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

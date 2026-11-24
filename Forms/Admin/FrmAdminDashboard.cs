using System.Drawing.Drawing2D;
using System.Drawing.Text;
using CinemaManagement.Forms.Shared;
using CinemaManagement.Forms.Staff;
using CinemaManagement.Helpers;
using CinemaManagement.Models;
using static CinemaManagement.Helpers.UiHelper;

namespace CinemaManagement.Forms.Admin
{
    public partial class FrmAdminDashboard : Form
    {
        private static readonly Color BgBase = ColorTranslator.FromHtml("#0b0f12");
        private static readonly Color BgContent = ColorTranslator.FromHtml("#101418");
        private static readonly Color BorderCol = ColorTranslator.FromHtml("#232a32");
        private static readonly Color Primary = ColorTranslator.FromHtml("#0284c7");
        private static readonly Color Cyan = ColorTranslator.FromHtml("#38bdf8");
        private static readonly Color TextPrimary = ColorTranslator.FromHtml("#ffffff");
        private static readonly Color TextLabel = ColorTranslator.FromHtml("#94a3b8");
        private static readonly Color TextLabelLight = ColorTranslator.FromHtml("#cbd5e1");
        private static readonly Color TextMuted = ColorTranslator.FromHtml("#64748b");

        private const string IconFont = "Segoe MDL2 Assets";
        private static readonly string UiFontFamily = ResolveUiFont();

        private static string ResolveUiFont()
        {
            using var fonts = new InstalledFontCollection();
            return fonts.Families.Any(f => f.Name.Equals("Inter", StringComparison.OrdinalIgnoreCase)) ? "Inter" : "Segoe UI";
        }

        private static Font Px(float px, FontStyle style = FontStyle.Regular) => new(UiFontFamily, px, style, GraphicsUnit.Pixel);

        private static Font Glyph(float px) => new(IconFont, px, FontStyle.Regular, GraphicsUnit.Pixel);

        private Panel _sidebar = null!;
        private Panel _header = null!;
        private Panel _content = null!;

        public FrmAdminDashboard()
        {
            InitializeComponent();
            AppSession.RequireAdmin(this);

            Text = "CINELUXE MANAGEMENT";
            ClientSize = WindowLayout.FitToScreen(WindowLayout.StandardClientSize);
            MinimumSize = new Size(1100, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = BgBase;
            Font = Px(13);
            DoubleBuffered = true;

            BuildHeader();
            BuildSidebar();
            BuildContent();
        }

        private void BuildSidebar()
        {
            _sidebar = new Panel { Dock = DockStyle.Left, Width = 240, BackColor = BgBase };
            Controls.Add(_sidebar);

            int y = 20;
            void Section(string text)
            {
                _sidebar.Controls.Add(new Label
                {
                    Text = text,
                    Font = Px(11, FontStyle.Bold),
                    ForeColor = TextMuted,
                    Location = new Point(20, y),
                    AutoSize = true
                });
                y += 28;
            }

            void Item(string glyph, string text, Action onClick)
            {
                var item = new SidebarItem(glyph, text, false) { Location = new Point(10, y), Size = new Size(220, 42) };
                item.Clicked += onClick;
                _sidebar.Controls.Add(item);
                y += 46;
            }

            Section("TRUNG TÂM VẬN HÀNH");
            Item("\uE748", "Chọn vai trò", () => StaffNavigator.ReturnToRoleSelection(this));
            Item("\uE714", "Quản lý danh mục phim", () => StaffNavigator.Navigate<FrmAdminMovies>(this));
            Item("\uE8A9", "Cấu hình phòng chiếu", () => StaffNavigator.Navigate<FrmAdminRooms>(this));
            Item("\uE787", "Lịch chiếu phim", () => StaffNavigator.Navigate<FrmAdminShowtimes>(this));

            y += 10;
            Section("QUẦY & BÁO CÁO");
            Item("\uE7BF", "Quầy bán vé POS", () => StaffNavigator.Navigate<FrmStaffPOS>(this));
            Item("\uE719", "Combo bắp nước", () => StaffNavigator.Navigate<FrmAdminCombos>(this));
            Item("\uE9D2", "Báo cáo doanh thu", () => StaffNavigator.Navigate<FrmAdminReports>(this));
            Item("\uEB05", "Tỷ lệ lấp đầy", () => StaffNavigator.Navigate<FrmOccupancyReport>(this));
            Item("\uE716", "Tài khoản && phân quyền", () => StaffNavigator.Navigate<FrmAdminUsers>(this));
        }

        private void BuildHeader()
        {
            _header = new Panel { Dock = DockStyle.Top, Height = 64, BackColor = BgBase };
            _header.Paint += (_, e) =>
            {
                using var pen = new Pen(BorderCol);
                e.Graphics.DrawLine(pen, 0, _header.Height - 1, _header.Width, _header.Height - 1);
            };
            Controls.Add(_header);

            var logo = new Label
            {
                Text = "\uE714",
                Font = Glyph(18),
                ForeColor = TextPrimary,
                BackColor = Primary,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 14),
                Size = new Size(36, 36)
            };
            var brand = new Label
            {
                Text = "CineLuxe Management",
                Font = Px(18, FontStyle.Bold),
                ForeColor = Cyan,
                Location = new Point(66, 20),
                AutoSize = true
            };

            string role = AppSession.CurrentUser?.Role == Roles.Admin ? "Quản trị viên" : "Nhân viên";
            var lblName = new Label { Text = AppSession.DisplayName, Font = Px(13, FontStyle.Bold), ForeColor = TextPrimary, TextAlign = ContentAlignment.MiddleRight, Size = new Size(220, 18), Anchor = AnchorStyles.Top | AnchorStyles.Right };
            var lblRole = new Label { Text = role, Font = Px(12, FontStyle.Bold), ForeColor = Cyan, TextAlign = ContentAlignment.MiddleRight, Size = new Size(220, 16), Anchor = AnchorStyles.Top | AnchorStyles.Right };
            var avatar = new AvatarPanel(Initials(AppSession.DisplayName)) { Size = new Size(40, 40), Anchor = AnchorStyles.Top | AnchorStyles.Right };

            _header.Controls.AddRange(new Control[] { logo, brand, lblName, lblRole, avatar });

            void PlaceRight()
            {
                int right = _header.ClientSize.Width - 20;
                avatar.Location = new Point(right - 40, 12);
                lblName.Location = new Point(avatar.Left - 230, 14);
                lblRole.Location = new Point(avatar.Left - 230, 33);
            }
            _header.Resize += (_, _) => PlaceRight();
            PlaceRight();
        }

        private static string Initials(string name)
        {
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return "?";
            return parts.Length == 1 ? parts[0][..1].ToUpper() : (parts[0][..1] + parts[^1][..1]).ToUpper();
        }

        private void BuildContent()
        {
            _content = new Panel { Dock = DockStyle.Fill, BackColor = BgContent };
            Controls.Add(_content);
            _content.BringToFront();

            _content.Controls.Add(new Label
            {
                Text = "Trung Tâm Điều Hành Rạp",
                Font = Px(26, FontStyle.Bold),
                ForeColor = TextPrimary,
                Location = new Point(32, 32),
                AutoSize = true
            });
            _content.Controls.Add(new Label
            {
                Text = $"Xin chào, {AppSession.DisplayName}. Chọn một chức năng ở menu bên trái để bắt đầu.",
                Font = Px(14),
                ForeColor = TextLabel,
                Location = new Point(34, 80),
                AutoSize = true
            });
        }

        private class SidebarItem : Panel
        {
            public event Action? Clicked;
            private readonly bool _active;
            private bool _hover;

            public SidebarItem(string glyph, string text, bool active)
            {
                _active = active;
                DoubleBuffered = true;
                Cursor = active ? Cursors.Default : Cursors.Hand;
                BackColor = BgBase;

                var icon = new Label { Text = glyph, Font = Glyph(16), ForeColor = active ? TextPrimary : TextLabelLight, BackColor = Color.Transparent, Location = new Point(14, 12), Size = new Size(20, 20) };
                var label = new Label { Text = text, Font = Px(14, active ? FontStyle.Bold : FontStyle.Regular), ForeColor = active ? TextPrimary : TextLabelLight, BackColor = Color.Transparent, Location = new Point(44, 11), AutoSize = true };
                Controls.Add(icon);
                Controls.Add(label);

                foreach (Control c in new Control[] { this, icon, label })
                {
                    c.Click += (_, _) => Clicked?.Invoke();
                    c.MouseEnter += (_, _) => { _hover = true; Invalidate(); };
                    c.MouseLeave += (_, _) => { _hover = ClientRectangle.Contains(PointToClient(Cursor.Position)); Invalidate(); };
                }
            }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
                e.Graphics.Clear(BgBase);
                if (!_active && !_hover) return;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var path = RoundedRect(new Rectangle(0, 0, Width - 1, Height - 1), 6);
                using var brush = new SolidBrush(_active ? Primary : ColorTranslator.FromHtml("#1e242a"));
                e.Graphics.FillPath(brush, path);
            }
        }

        private class AvatarPanel : Panel
        {
            private readonly string _initials;
            public AvatarPanel(string initials)
            {
                _initials = initials;
                DoubleBuffered = true;
                BackColor = BgBase;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                using var brush = new SolidBrush(Primary);
                using var pen = new Pen(Cyan, 2);
                e.Graphics.FillEllipse(brush, 1, 1, Width - 3, Height - 3);
                e.Graphics.DrawEllipse(pen, 1, 1, Width - 3, Height - 3);
                using var font = Px(14, FontStyle.Bold);
                TextRenderer.DrawText(e.Graphics, _initials, font, ClientRectangle, TextPrimary,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

    }
}

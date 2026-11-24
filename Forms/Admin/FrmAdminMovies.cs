using System.Text;
using CinemaManagement.Forms.Shared;
using CinemaManagement.Helpers;
using CinemaManagement.Models;
using CinemaManagement.Services;
using static CinemaManagement.Helpers.UiHelper;

namespace CinemaManagement.Forms.Admin
{
    public partial class FrmAdminMovies : Form
    {
        private static readonly Color BgMain = ColorTranslator.FromHtml("#101418");
        private static readonly Color BgDark = ColorTranslator.FromHtml("#0B0F12");
        private static readonly Color BgCard = ColorTranslator.FromHtml("#181C20");
        private static readonly Color BgSurface = ColorTranslator.FromHtml("#222930");
        private static readonly Color BorderCol = ColorTranslator.FromHtml("#333C48");
        private static readonly Color TxtSoft = ColorTranslator.FromHtml("#CBD5E1");
        private static readonly Color TxtDim = ColorTranslator.FromHtml("#94A3B8");
        private static readonly Color TxtMuted = ColorTranslator.FromHtml("#64748B");
        private static readonly Color Accent = ColorTranslator.FromHtml("#F59E0B");
        private static readonly Color Primary = ColorTranslator.FromHtml("#1A5276");
        private static readonly Color Danger = ColorTranslator.FromHtml("#EF4444");
        private static readonly Color Highlight = ColorTranslator.FromHtml("#FBBF24");

        private readonly ToolTip tip = new();
        private List<Movie> allMovies = new();

        private Panel pnlSidebar = null!;
        private Panel pnlHeader = null!;
        private Panel pnlFooter = null!;
        private Panel pnlMain = null!;
        private FlowLayoutPanel flowMovieGrid = null!;
        private Button btnOpenSidebar = null!;
        private TextBox txtSearch = null!;
        private Label lblFooter = null!;
        private System.Windows.Forms.Timer searchTimer = null!;

        private Label kpiTotalValue = null!, kpiTotalSub = null!;
        private Label kpiGenreValue = null!, kpiGenreSub = null!;
        private Label kpiAvgValue = null!, kpiAvgSub = null!;
        private Label kpiLongValue = null!, kpiLongSub = null!;

        public FrmAdminMovies()
        {
            InitializeComponent();
            AppSession.RequireAdmin(this);
            BuildLayout();
            ReloadFromDatabase();
            WindowLayout.ApplyStandardSize(this);
        }

        private void BuildLayout()
        {
            SuspendLayout();

            Width = 1440;
            Height = 850;
            MinimumSize = new Size(1000, 600);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cinema Management System | Quản lý danh mục phim";
            BackColor = BgMain;
            Font = new Font("Segoe UI", 10F);

            pnlMain = new Panel { Dock = DockStyle.Fill, BackColor = BgMain };
            Controls.Add(pnlMain);

            BuildSidebar();
            BuildFooter();
            BuildHeader();
            BuildMainContent();

            searchTimer = new System.Windows.Forms.Timer { Interval = 300 };
            searchTimer.Tick += (_, _) =>
            {
                searchTimer.Stop();
                RenderMovies();
            };
            FormClosed += (_, _) =>
            {
                searchTimer.Dispose();
                tip.Dispose();
            };

            ResumeLayout(true);
        }

        private void BuildHeader()
        {
            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 40, BackColor = BgDark };

            btnOpenSidebar = MakeButton("☰", 36, 30, BgCard, Color.White, true, false);
            btnOpenSidebar.Location = new Point(12, 5);
            btnOpenSidebar.Visible = false;
            btnOpenSidebar.Click += (_, _) =>
            {
                pnlSidebar.Visible = true;
                btnOpenSidebar.Visible = false;
            };
            pnlHeader.Controls.Add(btnOpenSidebar);

            pnlHeader.Controls.Add(new Label
            {
                Text = "CINEMA MANAGEMENT SYSTEM   •   Quản trị  /  Danh mục phim",
                ForeColor = TxtDim,
                Font = new Font("Consolas", 9F),
                AutoSize = true,
                Location = new Point(60, 12)
            });

            Controls.Add(pnlHeader);
        }

        private void BuildFooter()
        {
            pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 32, BackColor = BgDark };
            lblFooter = new Label
            {
                ForeColor = TxtMuted,
                Font = new Font("Consolas", 9F),
                AutoSize = true,
                Location = new Point(16, 8)
            };
            pnlFooter.Controls.Add(lblFooter);
            Controls.Add(pnlFooter);
        }

        private void BuildSidebar()
        {
            pnlSidebar = new Panel { Dock = DockStyle.Left, Width = 256, BackColor = BgDark };

            pnlSidebar.Controls.Add(new Label
            {
                Text = "CineLuxe Admin",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(16, 16)
            });
            pnlSidebar.Controls.Add(new Label
            {
                Text = "EXECUTIVE SUITE MANAGEMENT",
                ForeColor = TxtMuted,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(17, 42)
            });

            var btnClose = MakeButton("✕", 32, 32, BgCard, TxtDim, true, false);
            btnClose.Location = new Point(210, 14);
            btnClose.Click += (_, _) =>
            {
                pnlSidebar.Visible = false;
                btnOpenSidebar.Visible = true;
            };
            btnClose.MouseEnter += (_, _) =>
            {
                btnClose.BackColor = Danger;
                btnClose.ForeColor = Color.White;
            };
            btnClose.MouseLeave += (_, _) =>
            {
                btnClose.BackColor = BgCard;
                btnClose.ForeColor = TxtDim;
            };
            pnlSidebar.Controls.Add(btnClose);

            AddSidebarItem("Chọn vai trò", 90, false, () => StaffNavigator.ReturnToRoleSelection(this));
            AddSidebarItem("Bảng điều khiển", 140, false, () => StaffNavigator.Navigate<FrmAdminDashboard>(this));
            AddSidebarItem("Quản lý danh mục phim", 190, true, null);
            AddSidebarItem("Thêm phim mới", 240, false, () => OpenMovieDetail(0));
            AddSidebarItem("Cấu hình phòng chiếu", 290, false, OpenRoomsForm);
            AddSidebarItem("Lịch chiếu phim", 340, false, OpenShowtimesForm);

            Controls.Add(pnlSidebar);
        }

        private void OpenRoomsForm() => StaffNavigator.Navigate<FrmAdminRooms>(this);

        private void OpenShowtimesForm() => StaffNavigator.Navigate<FrmAdminShowtimes>(this);

        private void OpenMovieDetail(int movieId)
        {
            if (movieId > 0)
            {
                try
                {
                    if (_movieService.GetById(movieId) == null)
                    {
                        MessageBox.Show("Phim này không còn tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ReloadFromDatabase();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowError("Không đọc được thông tin phim", ex);
                    return;
                }
            }

            using var frm = new FrmMovieDetail(movieId);
            if (frm.ShowDialog(this) == DialogResult.OK)
                ReloadFromDatabase();
        }

        private void AddSidebarItem(string text, int top, bool isActive, Action? onClick)
        {
            var back = isActive ? Primary : BgDark;
            var btn = new Button
            {
                Text = text,
                Width = 224,
                Height = 38,
                Location = new Point(16, top),
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                BackColor = back,
                ForeColor = isActive ? Color.White : TxtSoft,
                Font = new Font("Segoe UI", 9.5F, isActive ? FontStyle.Bold : FontStyle.Regular),
                Cursor = isActive ? Cursors.Default : Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderSize = 0;

            if (!isActive)
            {
                btn.MouseEnter += (_, _) => btn.BackColor = BgCard;
                btn.MouseLeave += (_, _) => btn.BackColor = back;
            }

            if (onClick != null) btn.Click += (_, _) => onClick();

            pnlSidebar.Controls.Add(btn);
        }

        private void BuildMainContent()
        {
            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(24, 20, 24, 8),
                BackColor = BgMain
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            tbl.Controls.Add(BuildKpiRow(), 0, 0);
            tbl.Controls.Add(BuildToolbar(), 0, 1);

            flowMovieGrid = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = BgMain,
                Padding = new Padding(0, 4, 0, 0),
                Margin = new Padding(0)
            };
            tbl.Controls.Add(flowMovieGrid, 0, 2);

            pnlMain.Controls.Add(tbl);
        }

        private TableLayoutPanel BuildKpiRow()
        {
            var row = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = BgMain,
                Margin = new Padding(0, 0, 0, 12)
            };
            for (int i = 0; i < 4; i++) row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            row.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            row.Controls.Add(CreateKpiCard("TỔNG SỐ PHIM", out kpiTotalValue, out kpiTotalSub), 0, 0);
            row.Controls.Add(CreateKpiCard("SỐ THỂ LOẠI", out kpiGenreValue, out kpiGenreSub), 1, 0);
            row.Controls.Add(CreateKpiCard("THỜI LƯỢNG TRUNG BÌNH", out kpiAvgValue, out kpiAvgSub), 2, 0);
            row.Controls.Add(CreateKpiCard("PHIM DÀI NHẤT", out kpiLongValue, out kpiLongSub), 3, 0);
            return row;
        }

        private Panel CreateKpiCard(string title, out Label value, out Label sub)
        {
            var card = new Panel { Dock = DockStyle.Fill, BackColor = BgCard, Margin = new Padding(0, 0, 16, 0) };

            card.Controls.Add(new Label
            {
                Text = title,
                ForeColor = TxtDim,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(14, 12)
            });

            value = new Label
            {
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                AutoSize = false,
                AutoEllipsis = true,
                Location = new Point(12, 34),
                Size = new Size(200, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            card.Controls.Add(value);

            sub = new Label
            {
                ForeColor = TxtMuted,
                Font = new Font("Segoe UI", 8.5F),
                AutoSize = false,
                AutoEllipsis = true,
                Location = new Point(14, 70),
                Size = new Size(196, 20),
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Left | AnchorStyles.Right
            };
            card.Controls.Add(sub);

            return card;
        }

        private Panel BuildToolbar()
        {
            var bar = new Panel { Dock = DockStyle.Fill, BackColor = BgCard, Margin = new Padding(0, 0, 0, 12) };

            txtSearch = new TextBox
            {
                PlaceholderText = "Tìm theo tên phim hoặc thể loại...",
                Width = 340,
                Location = new Point(12, 13),
                BackColor = BgSurface,
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10F)
            };
            txtSearch.TextChanged += (_, _) =>
            {
                searchTimer.Stop();
                searchTimer.Start();
            };
            bar.Controls.Add(txtSearch);

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Padding = new Padding(0, 8, 12, 0),
                BackColor = Color.Transparent
            };

            var btnAdd = MakeButton("+ Thêm phim", 130, 36, Accent, BgMain, true, false);
            btnAdd.Click += (_, _) => OpenMovieDetail(0);
            var btnExport = MakeButton("Xuất danh sách", 140, 36, BgSurface, TxtSoft, true);
            btnExport.Click += (_, _) => ExportCsv();

            buttons.Controls.Add(btnAdd);
            buttons.Controls.Add(btnExport);
            bar.Controls.Add(buttons);

            return bar;
        }

        private async void ReloadFromDatabase()
        {
            try
            {
                allMovies = await _movieService.GetAllAsync();
            }
            catch (Exception ex)
            {
                ShowError("Không tải được danh sách phim", ex);
                allMovies = new List<Movie>();
            }

            UpdateStats();
            RenderMovies();
        }

        private void UpdateStats()
        {
            int total = allMovies.Count;
            kpiTotalValue.Text = total.ToString();
            kpiTotalSub.Text = "phim trong danh mục";

            kpiGenreValue.Text = allMovies
                .SelectMany(m => (m.Genre ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .Distinct(StringComparer.OrdinalIgnoreCase).Count().ToString();
            kpiGenreSub.Text = "thể loại khác nhau";

            if (total == 0)
            {
                kpiAvgValue.Text = "—";
                kpiAvgSub.Text = "";
                kpiLongValue.Text = "—";
                kpiLongSub.Text = "";
                return;
            }

            kpiAvgValue.Text = $"{(int)Math.Round(allMovies.Average(m => (double)m.Duration))} phút";
            kpiAvgSub.Text = "trung bình mỗi phim";

            var longest = allMovies.OrderByDescending(m => m.Duration).First();
            kpiLongValue.Text = longest.Title ?? "";
            kpiLongSub.Text = $"{longest.Duration} phút";
        }

        private void RenderMovies()
        {
            string k = txtSearch.Text.Trim();
            var movies = allMovies
                .Where(m => k.Length == 0
                            || (m.Title ?? "").Contains(k, StringComparison.OrdinalIgnoreCase)
                            || (m.Genre ?? "").Contains(k, StringComparison.OrdinalIgnoreCase))
                .OrderBy(m => m.Title)
                .ToList();

            flowMovieGrid.SuspendLayout();

            var old = flowMovieGrid.Controls.Cast<Control>().ToList();
            flowMovieGrid.Controls.Clear();
            foreach (var c in old) c.Dispose();

            if (movies.Count == 0)
            {
                flowMovieGrid.Controls.Add(new Label
                {
                    Text = k.Length == 0
                        ? "Chưa có phim nào. Bấm \"+ Thêm phim\" để tạo mới."
                        : "Không tìm thấy phim phù hợp.",
                    ForeColor = TxtMuted,
                    Font = new Font("Segoe UI", 11F),
                    AutoSize = true,
                    Margin = new Padding(4, 16, 0, 0)
                });
            }
            else
            {
                foreach (var m in movies) flowMovieGrid.Controls.Add(CreateMovieCard(m));
            }

            flowMovieGrid.ResumeLayout(true);
            lblFooter.Text =
                $"Hiển thị {movies.Count}/{allMovies.Count} phim   •   Cập nhật lúc {DateTime.Now:HH:mm:ss}";
        }

        private Panel CreateMovieCard(Movie m)
        {
            int id = m.MovieId;
            string title = string.IsNullOrWhiteSpace(m.Title) ? "(Chưa đặt tên)" : m.Title.Trim();
            string age = string.IsNullOrWhiteSpace(m.AgeRating) ? "P" : m.AgeRating.Trim();

            var card = new Panel { Width = 330, Height = 332, Margin = new Padding(0, 0, 20, 20), BackColor = BgCard };

            var poster = new Panel { Width = 330, Height = 170, Location = new Point(0, 0), BackColor = BgSurface };
            var picPoster = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = BgSurface
            };

            if (!string.IsNullOrEmpty(m.PosterPath))
            {
                string rawPath = m.PosterPath.TrimStart('/', '\\');
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(rawPath);

                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string projectDir = Directory.GetParent(baseDir)?.Parent?.Parent?.Parent?.FullName ?? baseDir;
                string[] searchFolders = new string[]
                {
                    Path.Combine(baseDir, "Image"),
                    Path.Combine(projectDir, "Image"),
                    Path.Combine(baseDir, Path.GetDirectoryName(rawPath) ?? ""),
                    Path.Combine(projectDir, Path.GetDirectoryName(rawPath) ?? "")
                };

                string finalPath = "";

                foreach (var folder in searchFolders)
                {
                    if (Directory.Exists(folder))
                    {
                        var matchedFile = Directory.GetFiles(folder)
                            .FirstOrDefault(f => Path.GetFileNameWithoutExtension(f)
                                .Equals(fileNameWithoutExt, StringComparison.OrdinalIgnoreCase));

                        if (matchedFile != null)
                        {
                            finalPath = matchedFile;
                            break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(finalPath))
                {
                    try
                    {
                        using var fs = new FileStream(finalPath, FileMode.Open, FileAccess.Read);
                        using var temp = Image.FromStream(fs);
                        picPoster.Image = new Bitmap(temp);
                    }
                    catch
                    {
                    }
                }
            }

            poster.Controls.Add(picPoster);

            if (picPoster.Image == null)
            {
                poster.Controls.Add(new Label
                {
                    Text = title.Length > 0 ? title.Substring(0, 1).ToUpper() : "?",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = TxtMuted,
                    Font = new Font("Segoe UI", 42F, FontStyle.Bold)
                });
            }

            var lblAge = new Label
            {
                Text = age,
                BackColor = AgeColor(age),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(44, 22),
                Location = new Point(12, 12)
            };
            poster.Controls.Add(lblAge);
            lblAge.BringToFront();
            card.Controls.Add(poster);

            card.Controls.Add(new Label
            {
                Text = $"{m.Duration} phút",
                ForeColor = Highlight,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(14, 182)
            });

            var lblTitle = new Label
            {
                Text = title,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = false,
                AutoEllipsis = true,
                Size = new Size(302, 26),
                Location = new Point(14, 206)
            };
            tip.SetToolTip(lblTitle, title + " (bấm để xem chi tiết)");
            lblTitle.Cursor = Cursors.Hand;
            lblTitle.Click += (_, _) => OpenMovieDetail(m.MovieId);
            card.Controls.Add(lblTitle);

            card.Controls.Add(new Label
            {
                Text = string.IsNullOrWhiteSpace(m.Genre) ? "Chưa phân loại" : m.Genre,
                ForeColor = TxtMuted,
                Font = new Font("Segoe UI", 8.5F),
                AutoSize = false,
                AutoEllipsis = true,
                Size = new Size(302, 20),
                Location = new Point(14, 238)
            });

            var btnEdit = MakeButton("Sửa", 92, 34, BgSurface, TxtSoft);
            btnEdit.Location = new Point(14, 284);
            btnEdit.Click += (_, _) => OpenMovieDetail(id);

            var btnSchedule = MakeButton("Xếp lịch", 100, 34, BgSurface, TxtSoft);
            btnSchedule.Location = new Point(112, 284);
            btnSchedule.Click += (_, _) => OpenShowtimesForm();

            var btnDelete = MakeButton("Xóa", 98, 34, BgSurface, TxtSoft);
            btnDelete.Location = new Point(218, 284);
            btnDelete.MouseEnter += (_, _) =>
            {
                btnDelete.BackColor = Danger;
                btnDelete.ForeColor = Color.White;
            };
            btnDelete.MouseLeave += (_, _) =>
            {
                btnDelete.BackColor = BgSurface;
                btnDelete.ForeColor = TxtSoft;
            };
            btnDelete.Click += (_, _) => DeleteMovie(id, title);

            card.Controls.Add(btnEdit);
            card.Controls.Add(btnSchedule);
            card.Controls.Add(btnDelete);

            return card;
        }

        private void DeleteMovie(int id, string title)
        {
            var confirm = MessageBox.Show($"Xóa phim \"{title}\"?", "Xác nhận xóa", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _movieService.Delete(id);
                ReloadFromDatabase();
            }
            catch (BusinessException ex)
            {
                MessageBox.Show(ex.Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ShowError("Không xóa được phim", ex);
            }
        }

        private void ExportCsv()
        {
            try
            {
                using var dlg = new SaveFileDialog
                {
                    Filter = "CSV (*.csv)|*.csv",
                    FileName = $"DanhSachPhim_{DateTime.Now:yyyyMMdd}.csv"
                };
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                var sb = new StringBuilder();
                sb.AppendLine("MovieId,Tên phim,Thể loại,Thời lượng (phút),Độ tuổi");
                foreach (var m in allMovies.OrderBy(x => x.Title))
                    sb.AppendLine(string.Join(",", m.MovieId, Csv(m.Title), Csv(m.Genre), m.Duration,
                        Csv(m.AgeRating)));

                File.WriteAllText(dlg.FileName, sb.ToString(), new UTF8Encoding(true));
                MessageBox.Show("Đã xuất danh sách phim thành công.", "Thành công", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ShowError("Không xuất được file", ex);
            }
        }

        private readonly MovieService _movieService = new();

        private static void ShowError(string message, Exception ex)
            => MessageBox.Show($"{message}:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private static Color AgeColor(string? age) => (age ?? "").Trim().ToUpperInvariant() switch
        {
            "P" => ColorTranslator.FromHtml("#16A34A"),
            "T13" => ColorTranslator.FromHtml("#0284C7"),
            "T16" => ColorTranslator.FromHtml("#D97706"),
            "T18" => ColorTranslator.FromHtml("#DC2626"),
            _ => ColorTranslator.FromHtml("#64748B")
        };

        private static Button MakeButton(string text, int width, int height, Color back, Color fore, bool bold = false,
            bool border = true)
        {
            var btn = new Button
            {
                Text = text,
                Width = width,
                Height = height,
                FlatStyle = FlatStyle.Flat,
                BackColor = back,
                ForeColor = fore,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false,
                Font = new Font("Segoe UI", 9F, bold ? FontStyle.Bold : FontStyle.Regular)
            };
            btn.FlatAppearance.BorderSize = border ? 1 : 0;
            btn.FlatAppearance.BorderColor = BorderCol;

            var hover = ControlPaint.Light(back, 0.15f);
            btn.MouseEnter += (_, _) => btn.BackColor = hover;
            btn.MouseLeave += (_, _) => btn.BackColor = back;
            return btn;
        }
    }
}
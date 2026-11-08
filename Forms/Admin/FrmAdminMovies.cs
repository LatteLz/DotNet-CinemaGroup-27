using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CinemaManagement.Data;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

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
            BuildLayout();
            ReloadFromDatabase();
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
            searchTimer.Tick += (_, _) => { searchTimer.Stop(); RenderMovies(); };
            FormClosed += (_, _) => { searchTimer.Dispose(); tip.Dispose(); };

            ResumeLayout(true);
        }

        private void BuildHeader()
        {
            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 40, BackColor = BgDark };

            btnOpenSidebar = MakeButton("☰", 36, 30, BgCard, Color.White, true, false);
            btnOpenSidebar.Location = new Point(12, 5);
            btnOpenSidebar.Visible = false;
            btnOpenSidebar.Click += (_, _) => { pnlSidebar.Visible = true; btnOpenSidebar.Visible = false; };
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
            btnClose.Click += (_, _) => { pnlSidebar.Visible = false; btnOpenSidebar.Visible = true; };
            btnClose.MouseEnter += (_, _) => { btnClose.BackColor = Danger; btnClose.ForeColor = Color.White; };
            btnClose.MouseLeave += (_, _) => { btnClose.BackColor = BgCard; btnClose.ForeColor = TxtDim; };
            pnlSidebar.Controls.Add(btnClose);

            AddSidebarItem("Chọn vai trò", 90, false, Close);
            AddSidebarItem("Bảng điều khiển", 140, false, () => ShowComingSoon("Bảng điều khiển"));
            AddSidebarItem("Quản lý danh mục phim", 190, true, null);
            AddSidebarItem("Chi tiết phim", 240, false, () => ShowComingSoon("Chi tiết phim"));
            AddSidebarItem("Cấu hình phòng chiếu", 290, false, () => ShowComingSoon("Cấu hình phòng chiếu"));
            AddSidebarItem("Lịch chiếu phim", 340, false, () => ShowComingSoon("Lịch chiếu phim"));

            Controls.Add(pnlSidebar);
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
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
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
            txtSearch.TextChanged += (_, _) => { searchTimer.Stop(); searchTimer.Start(); };
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
            btnAdd.Click += (_, _) => AddMovie();
            var btnExport = MakeButton("Xuất danh sách", 140, 36, BgSurface, TxtSoft, true);
            btnExport.Click += (_, _) => ExportCsv();

            buttons.Controls.Add(btnAdd);
            buttons.Controls.Add(btnExport);
            bar.Controls.Add(buttons);

            return bar;
        }

        private void ReloadFromDatabase()
        {
            try
            {
                allMovies = ReadAllMovies();
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

            kpiGenreValue.Text = allMovies.Select(m => (m.Genre ?? "").Trim())
                .Where(g => g.Length > 0).Distinct(StringComparer.OrdinalIgnoreCase).Count().ToString();
            kpiGenreSub.Text = "thể loại khác nhau";

            if (total == 0)
            {
                kpiAvgValue.Text = "—";
                kpiAvgSub.Text = "";
                kpiLongValue.Text = "—";
                kpiLongSub.Text = "";
                return;
            }

            kpiAvgValue.Text = $"{(int)Math.Round(allMovies.Average(m => (double)Dur(m)))} phút";
            kpiAvgSub.Text = "trung bình mỗi phim";

            var longest = allMovies.OrderByDescending(Dur).First();
            kpiLongValue.Text = longest.Title ?? "";
            kpiLongSub.Text = $"{Dur(longest)} phút";
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
            lblFooter.Text = $"Hiển thị {movies.Count}/{allMovies.Count} phim   •   Cập nhật lúc {DateTime.Now:HH:mm:ss}";
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
                string cleanPath = m.PosterPath.TrimStart('/', '\\');
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cleanPath);

                if (File.Exists(fullPath))
                {
                    try
                    {
                        using var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                        picPoster.Image = Image.FromStream(fs);
                    }
                    catch { }
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
            tip.SetToolTip(lblTitle, title);
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
            btnEdit.Click += (_, _) => EditMovie(id);

            var btnSchedule = MakeButton("Xếp lịch", 100, 34, BgSurface, TxtSoft);
            btnSchedule.Location = new Point(112, 284);
            btnSchedule.Click += (_, _) => ShowComingSoon("Xếp lịch chiếu");

            var btnDelete = MakeButton("Xóa", 98, 34, BgSurface, TxtSoft);
            btnDelete.Location = new Point(218, 284);
            btnDelete.MouseEnter += (_, _) => { btnDelete.BackColor = Danger; btnDelete.ForeColor = Color.White; };
            btnDelete.MouseLeave += (_, _) => { btnDelete.BackColor = BgSurface; btnDelete.ForeColor = TxtSoft; };
            btnDelete.Click += (_, _) => DeleteMovie(id, title);

            card.Controls.Add(btnEdit);
            card.Controls.Add(btnSchedule);
            card.Controls.Add(btnDelete);

            return card;
        }

        private void AddMovie()
        {
            using var dlg = new MovieEditorDialog(null);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                SaveNewMovie(dlg.Result);
                ReloadFromDatabase();
            }
            catch (Exception ex)
            {
                ShowError("Không thêm được phim", ex);
            }
        }

        private void EditMovie(int id)
        {
            Movie? movie;
            try { movie = FindMovie(id); }
            catch (Exception ex) { ShowError("Không đọc được thông tin phim", ex); return; }

            if (movie == null)
            {
                MessageBox.Show("Phim này không còn tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReloadFromDatabase();
                return;
            }

            using var dlg = new MovieEditorDialog(movie);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                SaveChangedMovie(dlg.Result);
                ReloadFromDatabase();
            }
            catch (Exception ex)
            {
                ShowError("Không lưu được thay đổi", ex);
            }
        }

        private void DeleteMovie(int id, string title)
        {
            var confirm = MessageBox.Show($"Xóa phim \"{title}\"?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                RemoveMovie(id);
                ReloadFromDatabase();
            }
            catch (InvalidOperationException ex)
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
                    sb.AppendLine(string.Join(",", m.MovieId, Csv(m.Title), Csv(m.Genre), m.Duration, Csv(m.AgeRating)));

                File.WriteAllText(dlg.FileName, sb.ToString(), new UTF8Encoding(true));
                MessageBox.Show("Đã xuất danh sách phim.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ShowError("Không xuất được file", ex);
            }
        }

        private static string Csv(string? s) => "\"" + (s ?? "").Replace("\"", "\"\"") + "\"";

        private static List<Movie> ReadAllMovies()
        {
            using var db = new CinemaDbContext();
            return db.Movies.AsNoTracking().ToList();
        }

        private static Movie? FindMovie(int id)
        {
            using var db = new CinemaDbContext();
            return db.Movies.AsNoTracking().FirstOrDefault(m => m.MovieId == id); 
        }

        private static void SaveNewMovie(Movie movie)
        {
            using var db = new CinemaDbContext();
            db.Movies.Add(movie);
            db.SaveChanges();
        }

        private static void SaveChangedMovie(Movie movie)
        {
            using var db = new CinemaDbContext();
            db.Movies.Update(movie);
            db.SaveChanges();
        }

        private static void RemoveMovie(int id)
        {
            using var db = new CinemaDbContext();

            if (db.Showtimes.Any(s => s.MovieId == id))
                throw new InvalidOperationException(
                    "Phim này đã có suất chiếu nên không thể xóa. Hãy xóa các suất chiếu của phim trước.");

            var movie = db.Movies.Find(id)
                        ?? throw new InvalidOperationException("Không tìm thấy phim cần xóa.");
            db.Movies.Remove(movie);
            db.SaveChanges();
        }

        private static int Dur(Movie m) => Convert.ToInt32(m.Duration);

        private static void ShowError(string message, Exception ex)
            => MessageBox.Show($"{message}:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private static void ShowComingSoon(string feature)
            => MessageBox.Show($"Màn hình \"{feature}\" đang được phát triển.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

        private static Color AgeColor(string? age) => (age ?? "").Trim().ToUpperInvariant() switch
        {
            "P" => ColorTranslator.FromHtml("#16A34A"),
            "T13" => ColorTranslator.FromHtml("#0284C7"),
            "T16" => ColorTranslator.FromHtml("#D97706"),
            "T18" => ColorTranslator.FromHtml("#DC2626"),
            _ => ColorTranslator.FromHtml("#64748B")
        };

        private static Button MakeButton(string text, int width, int height, Color back, Color fore,
                                         bool bold = false, bool border = true)
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

        private sealed class MovieEditorDialog : Form
        {
            private readonly Movie movie;
            private readonly bool isNew;
            private readonly ErrorProvider error = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

            private TextBox txtTitle = null!;
            private ComboBox cboGenre = null!;
            private NumericUpDown numDuration = null!;
            private ComboBox cboAge = null!;
            private TextBox txtPosterPath = null!;
            private TextBox txtDescription = null!;

            public Movie Result => movie;

            public MovieEditorDialog(Movie? movieItem)
            {
                isNew = movieItem == null;
                movie = movieItem ?? new Movie();
                BuildUi();
                if (!isNew) LoadValues();
            }

            private void BuildUi()
            {
                Text = isNew ? "Thêm phim mới" : "Sửa thông tin phim";
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;
                StartPosition = FormStartPosition.CenterParent;
                ClientSize = new Size(460, 440);
                BackColor = BgMain;
                Font = new Font("Segoe UI", 10F);

                int y = 16;
                AddLabel("Tên phim *", 24, y);
                txtTitle = new TextBox
                {
                    Location = new Point(24, y + 22),
                    Width = 412,
                    MaxLength = 200,
                    BackColor = BgSurface,
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle
                };
                Controls.Add(txtTitle);

                y += 62;
                AddLabel("Thể loại", 24, y);
                cboGenre = new ComboBox
                {
                    Location = new Point(24, y + 22),
                    Width = 412,
                    DropDownStyle = ComboBoxStyle.DropDown,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = BgSurface,
                    ForeColor = Color.White
                };
                cboGenre.Items.AddRange(new object[]
                {
                    "Hành động", "Tình cảm", "Hài", "Kinh dị", "Khoa học viễn tưởng",
                    "Hoạt hình", "Phiêu lưu", "Tâm lý", "Gia đình"
                });
                Controls.Add(cboGenre);

                y += 62;
                AddLabel("Thời lượng (phút) *", 24, y);
                numDuration = new NumericUpDown
                {
                    Location = new Point(24, y + 22),
                    Width = 190,
                    Minimum = 30,
                    Maximum = 400,
                    Value = 100,
                    BackColor = BgSurface,
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle
                };
                Controls.Add(numDuration);

                AddLabel("Độ tuổi", 246, y);
                cboAge = new ComboBox
                {
                    Location = new Point(246, y + 22),
                    Width = 190,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = BgSurface,
                    ForeColor = Color.White
                };
                cboAge.Items.AddRange(new object[] { "P", "T13", "T16", "T18" });
                cboAge.SelectedIndex = 0;
                Controls.Add(cboAge);

                y += 62;
                AddLabel("Đường dẫn poster (PosterPath)", 24, y);
                txtPosterPath = new TextBox
                {
                    Location = new Point(24, y + 22),
                    Width = 412,
                    MaxLength = 255,
                    BackColor = BgSurface,
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle
                };
                Controls.Add(txtPosterPath);

                y += 62;
                AddLabel("Mô tả phim", 24, y);
                txtDescription = new TextBox
                {
                    Location = new Point(24, y + 22),
                    Width = 412,
                    Height = 50,
                    Multiline = true,
                    BackColor = BgSurface,
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle
                };
                Controls.Add(txtDescription);

                var btnSave = MakeButton("Lưu", 100, 36, Accent, BgMain, true, false);
                btnSave.Location = new Point(222, 385);
                btnSave.Click += (_, _) => Save();
                Controls.Add(btnSave);

                var btnCancel = MakeButton("Hủy", 100, 36, BgSurface, TxtSoft);
                btnCancel.Location = new Point(336, 385);
                btnCancel.DialogResult = DialogResult.Cancel;
                Controls.Add(btnCancel);

                AcceptButton = btnSave;
                CancelButton = btnCancel;
            }

            private void AddLabel(string text, int x, int y)
            {
                Controls.Add(new Label
                {
                    Text = text,
                    Location = new Point(x, y),
                    AutoSize = true,
                    ForeColor = TxtDim,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                });
            }

            private void LoadValues()
            {
                txtTitle.Text = movie.Title;
                cboGenre.Text = movie.Genre ?? "";
                numDuration.Value = Math.Clamp(Convert.ToInt32(movie.Duration), 30, 400);

                int idx = cboAge.Items.IndexOf((movie.AgeRating ?? "P").Trim().ToUpperInvariant());
                cboAge.SelectedIndex = idx >= 0 ? idx : 0;
                txtPosterPath.Text = movie.PosterPath ?? "";
                txtDescription.Text = movie.Description ?? "";
            }

            private void Save()
            {
                error.Clear();
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    error.SetError(txtTitle, "Vui lòng nhập tên phim");
                    txtTitle.Focus();
                    return;
                }

                movie.Title = txtTitle.Text.Trim();
                movie.Genre = cboGenre.Text.Trim();
                movie.Duration = (int)numDuration.Value;
                movie.AgeRating = cboAge.Text;
                movie.PosterPath = txtPosterPath.Text.Trim();
                movie.Description = txtDescription.Text.Trim();

                DialogResult = DialogResult.OK;
            }
        }
    }
}
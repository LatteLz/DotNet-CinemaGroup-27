using CinemaManagement.Helpers;
using CinemaManagement.Models;
using CinemaManagement.Services;

namespace CinemaManagement.Forms.Admin
{
    public partial class FrmMovieDetail : Form
    {
        private readonly Color COLOR_BG_BASE = ColorTranslator.FromHtml("#0B0F12");
        private readonly Color COLOR_BG_CARD = ColorTranslator.FromHtml("#181C20");
        private readonly Color COLOR_BG_INPUT = ColorTranslator.FromHtml("#0B0F12");
        private readonly Color COLOR_BORDER_INPUT = ColorTranslator.FromHtml("#2D3748");
        private readonly Color COLOR_BG_CHIP = ColorTranslator.FromHtml("#1E242A");

        private readonly Color COLOR_PRIMARY = ColorTranslator.FromHtml("#0284C7");
        private readonly Color COLOR_SECONDARY_BTN = ColorTranslator.FromHtml("#334155");
        private readonly Color COLOR_CANCEL_BTN = ColorTranslator.FromHtml("#475569");
        private readonly Color COLOR_AMBER = ColorTranslator.FromHtml("#F59E0B");

        private readonly Color COLOR_TEXT_PRIMARY = ColorTranslator.FromHtml("#FFFFFF");
        private readonly Color COLOR_TEXT_LABEL = ColorTranslator.FromHtml("#94A3B8");
        private readonly Color COLOR_TEXT_MUTED = ColorTranslator.FromHtml("#64748B");

        private Panel pnlHeader;
        private Panel pnlNavBar;
        private Panel pnlMainModal;
        private PictureBox picPoster;

        private TextBox txtTitleVi,
            txtTitleEn,
            txtDuration,
            txtReleaseDate,
            txtCountry,
            txtDirector,
            txtActors,
            txtTrailerUrl,
            txtDescription;

        private Label lblCharCount;
        private Button btnSave, btnCancel, btnUploadPoster;
        private string selectedAgeRating = "P";
        private Panel pnlAgeChipsContainer;

        private readonly MovieService _movieService = new();
        private readonly ErrorProvider _errors = new();
        private readonly int _movieId;
        private Movie? _movie;
        private Panel _pnlGenres = null!;
        private List<string> _extraGenres = new();
        private string? _uploadedPosterFile;

        public FrmMovieDetail() : this(0) { }

        public FrmMovieDetail(int movieId)
        {
            _movieId = movieId;
            if (movieId > 0)
            {
                try
                {
                    _movie = _movieService.GetById(movieId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không đọc được thông tin phim: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (!string.IsNullOrWhiteSpace(_movie?.AgeRating)) selectedAgeRating = _movie!.AgeRating!;
            }

            InitializeCustomComponent();
            AppSession.RequireAdmin(this);
            _errors.ContainerControl = this;

            txtTitleEn.PlaceholderText = "Ví dụ: Spider-Man";
            txtReleaseDate.PlaceholderText = "dd/MM/yyyy";
            txtTrailerUrl.PlaceholderText = "https://www.youtube.com/watch?v=...";
            txtDescription.MaxLength = 2000;
            txtDescription.TextChanged += (_, _) => UpdateCharCount();

            FillFromMovie();
            WindowLayout.ScaleToStandard(this);
        }

        private void FillFromMovie()
        {
            if (_movie == null)
            {
                Text = "Thêm phim mới";
                UpdateCharCount();
                return;
            }

            Text = $"Chi tiết phim: {_movie.Title}";
            txtTitleVi.Text = _movie.Title;
            txtTitleEn.Text = _movie.OriginalTitle ?? "";
            txtReleaseDate.Text = _movie.ReleaseDate?.ToString("dd/MM/yyyy") ?? "";
            txtTrailerUrl.Text = _movie.TrailerUrl ?? "";
            txtDuration.Text = _movie.Duration.ToString();
            txtCountry.Text = _movie.Country ?? "";
            txtDirector.Text = _movie.Director ?? "";
            txtActors.Text = _movie.Actors ?? "";
            txtDescription.Text = _movie.Description ?? "";
            UpdateCharCount();
            picPoster.Image = ImageHelper.Load(_movie.PosterPath);

            var genres = (_movie.Genre ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
            foreach (CheckBox chk in _pnlGenres.Controls.OfType<CheckBox>())
            {
                chk.Checked = genres.Any(g => g.Equals(chk.Text, StringComparison.OrdinalIgnoreCase));
            }
            _extraGenres = genres
                .Where(g => !_pnlGenres.Controls.OfType<CheckBox>().Any(c => c.Text.Equals(g, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        private void SaveMovie()
        {
            _errors.Clear();
            bool ok = true;

            if (string.IsNullOrWhiteSpace(txtTitleVi.Text))
            {
                _errors.SetError(txtTitleVi, "Tên phim không được để trống");
                ok = false;
            }
            if (!int.TryParse(txtDuration.Text.Trim(), out int duration) || duration <= 0 || duration > 400)
            {
                _errors.SetError(txtDuration, "Thời lượng là số phút từ 1 đến 400");
                ok = false;
            }
            var checkedGenres = _pnlGenres.Controls.OfType<CheckBox>().Where(c => c.Checked).Select(c => c.Text).ToList();
            if (checkedGenres.Count == 0 && _extraGenres.Count == 0)
            {
                _errors.SetError(_pnlGenres, "Chọn ít nhất một thể loại");
                ok = false;
            }
            DateTime? releaseDate = null;
            string releaseText = txtReleaseDate.Text.Trim();
            if (releaseText.Length > 0)
            {
                if (DateTime.TryParseExact(releaseText, new[] { "dd/MM/yyyy", "d/M/yyyy" },
                        System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var parsed)
                    && parsed.Year >= 1900 && parsed.Year <= DateTime.Today.Year + 5)
                {
                    releaseDate = parsed;
                }
                else
                {
                    _errors.SetError(txtReleaseDate, "Ngày khởi chiếu theo dạng dd/MM/yyyy, ví dụ 25/12/2026");
                    ok = false;
                }
            }
            string trailer = txtTrailerUrl.Text.Trim();
            if (trailer.Length > 0 && !IsHttpUrl(trailer))
            {
                _errors.SetError(txtTrailerUrl, "Đường dẫn phải bắt đầu bằng http:// hoặc https://");
                ok = false;
            }
            if (txtTitleEn.Text.Trim().Length > 200)
            {
                _errors.SetError(txtTitleEn, "Tối đa 200 ký tự");
                ok = false;
            }
            if (txtDescription.Text.Length > 2000)
            {
                _errors.SetError(txtDescription, "Tối đa 2000 ký tự");
                ok = false;
            }
            if (!ok) return;

            string? oldPoster = _movie?.PosterPath;
            string? newPoster = null;
            try
            {
                if (_uploadedPosterFile != null)
                    newPoster = ImageHelper.SavePosterCopy(_uploadedPosterFile);

                var movie = new Movie
                {
                    MovieId = _movieId,
                    Title = txtTitleVi.Text.Trim(),
                    OriginalTitle = NullIfEmpty(txtTitleEn.Text),
                    ReleaseDate = releaseDate,
                    TrailerUrl = NullIfEmpty(trailer),
                    Duration = duration,
                    Genre = string.Join(", ", checkedGenres.Concat(_extraGenres)),
                    AgeRating = selectedAgeRating,
                    Country = NullIfEmpty(txtCountry.Text),
                    Director = NullIfEmpty(txtDirector.Text),
                    Actors = NullIfEmpty(txtActors.Text),
                    Description = NullIfEmpty(txtDescription.Text),
                    PosterPath = newPoster ?? oldPoster
                };

                if (_movieId > 0) _movieService.Update(movie);
                else _movieService.Add(movie);

                if (newPoster != null && !string.Equals(newPoster, oldPoster, StringComparison.OrdinalIgnoreCase))
                    ImageHelper.DeleteUploadedPoster(oldPoster);

                MessageBox.Show("Đã lưu phim thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (BusinessException ex)
            {
                RemoveUnsavedPoster(newPoster, oldPoster);
                MessageBox.Show(ex.Message, "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                RemoveUnsavedPoster(newPoster, oldPoster);
                MessageBox.Show("Không lưu được phim: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void RemoveUnsavedPoster(string? newPoster, string? oldPoster)
        {
            if (newPoster != null && !string.Equals(newPoster, oldPoster, StringComparison.OrdinalIgnoreCase))
                ImageHelper.DeleteUploadedPoster(newPoster);
        }

        private static string? NullIfEmpty(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

        private static bool IsHttpUrl(string url)
            => Uri.TryCreate(url, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

        private void UpdateCharCount()
        {
            lblCharCount.Text = $"Ký tự: {txtDescription.Text.Length} / 2000";
        }

        private void InitializeCustomComponent()
        {
            this.Size = new Size(1440, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = COLOR_BG_BASE;
            this.DoubleBuffered = true;

            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 36, BackColor = COLOR_BG_BASE };
            Label lblAppTitle = new Label
            {
                Text = "  🎬  CINELUXE CINEMA MANAGEMENT SYSTEM",
                ForeColor = COLOR_TEXT_LABEL,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Dock = DockStyle.Left,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };
            Button btnCloseApp = new Button
            {
                Text = "✕",
                Dock = DockStyle.Right,
                Width = 45,
                FlatStyle = FlatStyle.Flat,
                ForeColor = COLOR_TEXT_LABEL,
                Cursor = Cursors.Hand
            };
            btnCloseApp.FlatAppearance.BorderSize = 0;
            btnCloseApp.Click += (s, e) => this.Close();
            pnlHeader.Controls.Add(lblAppTitle);
            pnlHeader.Controls.Add(btnCloseApp);

            pnlNavBar = new Panel { Dock = DockStyle.Top, Height = 68, BackColor = COLOR_BG_CARD };
            Label lblBrand = new Label
            {
                Text = "CineLuxe Management\nHỆ THỐNG QUẢN LÝ RẠP CHIẾU PHIM",
                ForeColor = COLOR_TEXT_PRIMARY,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };
            Label lblUser = new Label
            {
                Text = $"{AppSession.DisplayName}\n{(AppSession.IsAdmin ? "Quản trị viên" : "Nhân viên")}",
                ForeColor = COLOR_TEXT_LABEL,
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                Location = new Point(1250, 15),
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = true
            };
            pnlNavBar.Controls.Add(lblBrand);
            pnlNavBar.Controls.Add(lblUser);

            pnlMainModal = new Panel
            {
                Size = new Size(1080, 760),
                Location = new Point((1440 - 1080) / 2, 115),
                BackColor = COLOR_BG_CARD
            };

            BuildModalContent();

            this.Controls.Add(pnlMainModal);
            this.Controls.Add(pnlNavBar);
            this.Controls.Add(pnlHeader);
        }

        private void BuildModalContent()
        {
            Label lblFormTitle = new Label
            {
                Text = "[CineLuxe] Thông tin chi tiết phim",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = COLOR_TEXT_PRIMARY,
                Location = new Point(24, 16),
                AutoSize = true
            };
            Label lblSubStatus = new Label
            {
                Text = "⚙ Chế độ: Xem / Chỉnh sửa thông tin",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = COLOR_TEXT_MUTED,
                Location = new Point(24, 42),
                AutoSize = true
            };

            Panel pnlLeftCol = new Panel
                { Location = new Point(24, 75), Size = new Size(320, 660), BackColor = Color.Transparent };

            Label lblPosterHead = new Label
            {
                Text = "POSTER PHIM (Tỉ lệ 2:3)",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = COLOR_TEXT_LABEL,
                Location = new Point(0, 0),
                AutoSize = true
            };

            picPoster = new PictureBox
            {
                Location = new Point(0, 25),
                Size = new Size(320, 480),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = COLOR_BG_INPUT,
                BorderStyle = BorderStyle.FixedSingle
            };
            btnUploadPoster = CreateCustomButton("📁 Tải ảnh Poster từ máy...", COLOR_PRIMARY, COLOR_TEXT_PRIMARY, new Point(0, 515), new Size(320, 36));
            btnUploadPoster.Click += btnUploadPoster_Click;
            txtTrailerUrl = CreateStyledTextBox("", new Point(0, 580), 246);
            var btnWatchTrailer = CreateCustomButton("▶ Xem", COLOR_SECONDARY_BTN, COLOR_TEXT_PRIMARY, new Point(252, 578), new Size(68, 28));
            btnWatchTrailer.Click += (_, _) => OpenTrailerLink(txtTrailerUrl.Text.Trim());
            Label lblTrailer = new Label
            {
                Text = "🎬 Đường dẫn Trailer / Video giới thiệu", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = COLOR_TEXT_LABEL, Location = new Point(0, 560), AutoSize = true
            };

            pnlLeftCol.Controls.Add(lblPosterHead);
            pnlLeftCol.Controls.Add(picPoster);
            pnlLeftCol.Controls.Add(btnUploadPoster);
            pnlLeftCol.Controls.Add(lblTrailer);
            pnlLeftCol.Controls.Add(txtTrailerUrl);
            pnlLeftCol.Controls.Add(btnWatchTrailer);

            Panel pnlRightCol = new Panel
                { Location = new Point(364, 75), Size = new Size(690, 660), BackColor = Color.Transparent };

            Label lblNameVi = CreateFieldLabel("Tên phim (Tiếng Việt) *", new Point(0, 0));
            txtTitleVi = CreateStyledTextBox("", new Point(0, 22), 330);

            Label lblNameEn = CreateFieldLabel("Tên gốc (Tiếng Anh)", new Point(350, 0));
            txtTitleEn = CreateStyledTextBox("", new Point(350, 22), 330);

            Label lblDuration = CreateFieldLabel("Thời lượng (Phút) *", new Point(0, 65));
            txtDuration = CreateStyledTextBox("", new Point(0, 87), 160);

            Label lblRelease = CreateFieldLabel("Ngày khởi chiếu", new Point(180, 65));
            txtReleaseDate = CreateStyledTextBox("", new Point(180, 87), 150);

            Label lblCountry = CreateFieldLabel("Quốc gia sản xuất", new Point(350, 65));
            txtCountry = CreateStyledTextBox("", new Point(350, 87), 330);

            Label lblAgeTitle = CreateFieldLabel("Phân loại độ tuổi phổ biến (Cục Điện ảnh) *", new Point(0, 130));
            pnlAgeChipsContainer = CreateAgeRatingChips(new Point(0, 152));

            Label lblGenreTitle = CreateFieldLabel("Thể loại phim (Chọn nhiều) *", new Point(0, 210));
            Panel pnlGenres = _pnlGenres = CreateGenreCheckboxes(new Point(0, 232));

            Label lblDirector = CreateFieldLabel("Đạo diễn", new Point(0, 290));
            txtDirector = CreateStyledTextBox("", new Point(0, 312), 330);

            Label lblActors = CreateFieldLabel("Diễn viên chính", new Point(350, 290));
            txtActors = CreateStyledTextBox("", new Point(350, 312), 330);

            Label lblDesc = CreateFieldLabel("Nội dung tóm tắt phim", new Point(0, 355));
            lblCharCount = new Label
            {
                Text = "Ký tự: 0 / 2000", Font = new Font("Segoe UI", 8f), ForeColor = COLOR_TEXT_MUTED,
                Location = new Point(560, 355), AutoSize = true
            };
            txtDescription = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Size = new Size(680, 120),
                Location = new Point(0, 377),
                BackColor = COLOR_BG_INPUT,
                ForeColor = COLOR_TEXT_PRIMARY,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9.5f),
                Text = ""
            };

            btnCancel = CreateCustomButton("🚫 Hủy bỏ (Esc)", COLOR_CANCEL_BTN, COLOR_TEXT_PRIMARY, new Point(270, 520),
                new Size(120, 38));
            btnCancel.Click += (s, e) => this.Close();

            btnSave = CreateCustomButton("✔ Lưu && Áp dụng", COLOR_PRIMARY, COLOR_TEXT_PRIMARY, new Point(520, 520),
                new Size(160, 38));
            btnSave.Click += (s, e) => SaveMovie();

            pnlRightCol.Controls.AddRange(new Control[]
            {
                lblNameVi, txtTitleVi, lblNameEn, txtTitleEn,
                lblDuration, txtDuration, lblRelease, txtReleaseDate, lblCountry, txtCountry,
                lblAgeTitle, pnlAgeChipsContainer, lblGenreTitle, pnlGenres,
                lblDirector, txtDirector, lblActors, txtActors,
                lblDesc, lblCharCount, txtDescription,
                btnCancel, btnSave
            });

            pnlMainModal.Controls.Add(lblFormTitle);
            pnlMainModal.Controls.Add(lblSubStatus);
            pnlMainModal.Controls.Add(pnlLeftCol);
            pnlMainModal.Controls.Add(pnlRightCol);
        }

        private Label CreateFieldLabel(string text, Point loc)
        {
            return new Label
            {
                Text = text,
                Location = loc,
                AutoSize = true,
                ForeColor = COLOR_TEXT_LABEL,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
        }

        private TextBox CreateStyledTextBox(string initText, Point loc, int width)
        {
            return new TextBox
            {
                Text = initText,
                Location = loc,
                Width = width,
                Height = 32,
                BackColor = COLOR_BG_INPUT,
                ForeColor = COLOR_TEXT_PRIMARY,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9.5f)
            };
        }

        private Button CreateCustomButton(string text, Color bg, Color fg, Point loc, Size size)
        {
            Button btn = new Button
            {
                Text = text,
                BackColor = bg,
                ForeColor = fg,
                Location = loc,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private Panel CreateAgeRatingChips(Point loc)
        {
            Panel pnl = new Panel { Location = loc, Size = new Size(680, 45), BackColor = Color.Transparent };
            string[] ratings =
            {
                "P\nMọi lứa tuổi", "K\nDưới 13 kèm PH", "T13\nTừ 13 tuổi", "T16\nTừ 16 tuổi", "T18\nNgười lớn (18+)"
            };
            string[] ratingKeys = { "P", "K", "T13", "T16", "T18" };

            int xPos = 0;
            for (int i = 0; i < ratings.Length; i++)
            {
                string key = ratingKeys[i];
                bool isDefaultSelected = (key == selectedAgeRating);

                Button chip = new Button
                {
                    Text = ratings[i],
                    Tag = key,
                    Size = new Size(125, 40),
                    Location = new Point(xPos, 0),
                    BackColor = isDefaultSelected ? COLOR_AMBER : COLOR_BG_CHIP,
                    ForeColor = isDefaultSelected ? Color.Black : COLOR_TEXT_PRIMARY,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 7.5f, isDefaultSelected ? FontStyle.Bold : FontStyle.Regular),
                    Cursor = Cursors.Hand
                };
                chip.FlatAppearance.BorderSize = isDefaultSelected ? 2 : 1;
                chip.FlatAppearance.BorderColor = isDefaultSelected ? COLOR_AMBER : COLOR_BORDER_INPUT;

                chip.Click += (s, e) =>
                {
                    selectedAgeRating = key;
                    foreach (Control ctrl in pnl.Controls)
                    {
                        if (ctrl is Button btnChip)
                        {
                            bool isSelected = btnChip.Tag.ToString() == selectedAgeRating;
                            btnChip.BackColor = isSelected ? COLOR_AMBER : COLOR_BG_CHIP;
                            btnChip.ForeColor = isSelected ? Color.Black : COLOR_TEXT_PRIMARY;
                            btnChip.Font = new Font("Segoe UI", 7.5f, isSelected ? FontStyle.Bold : FontStyle.Regular);
                            btnChip.FlatAppearance.BorderSize = isSelected ? 2 : 1;
                            btnChip.FlatAppearance.BorderColor = isSelected ? COLOR_AMBER : COLOR_BORDER_INPUT;
                        }
                    }
                };

                pnl.Controls.Add(chip);
                xPos += 135;
            }

            return pnl;
        }

        private Panel CreateGenreCheckboxes(Point loc)
        {
            Panel pnl = new Panel { Location = loc, Size = new Size(680, 45), BackColor = Color.Transparent };
            string[] genres = { "Hành động", "Phiêu lưu", "Viễn tưởng", "Hoạt hình", "Kinh dị", "Hài", "Gia đình", "Tâm lý", "Trinh thám", "Hình sự" };

            int xPos = 0, yPos = 0;
            for (int i = 0; i < genres.Length; i++)
            {
                CheckBox chk = new CheckBox
                {
                    Text = genres[i],
                    AutoSize = false,
                    Size = new Size(130, 22),
                    AutoEllipsis = true,
                    Location = new Point(xPos, yPos),
                    ForeColor = COLOR_TEXT_PRIMARY,
                    Font = new Font("Segoe UI", 9f),
                    Checked = false
                };
                pnl.Controls.Add(chk);
                xPos += 135;
                if (i == 4)
                {
                    xPos = 0;
                    yPos += 22;
                }
            }

            return pnl;
        }

        private void btnUploadPoster_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.webp";
                openFileDialog.Title = "Chọn ảnh Poster phim";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var img = ImageHelper.Load(openFileDialog.FileName);
                    if (img == null)
                    {
                        MessageBox.Show("Không đọc được file ảnh đã chọn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    picPoster.Image = img;
                    _uploadedPosterFile = openFileDialog.FileName;
                }
            }
        }

        private void OpenTrailerLink(string url)
        {
            if (!string.IsNullOrWhiteSpace(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("Đường dẫn Trailer không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
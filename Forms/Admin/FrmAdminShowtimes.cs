using CinemaManagement.Helpers;
using CinemaManagement.Models;
using CinemaManagement.Services;

namespace CinemaManagement.Forms.Admin
{
    public partial class FrmAdminShowtimes : Form
    {
        private static readonly Color BgDark = ColorTranslator.FromHtml("#0B0F12");
        private static readonly Color BgSurface = ColorTranslator.FromHtml("#101418");
        private static readonly Color BgCard = ColorTranslator.FromHtml("#181C20");
        private static readonly Color BorderCol = ColorTranslator.FromHtml("#2A323D");
        private static readonly Color Accent = ColorTranslator.FromHtml("#0284C7");
        private static readonly Color Danger = ColorTranslator.FromHtml("#EF4444");
        private static readonly Color TextDim = ColorTranslator.FromHtml("#94A3B8");

        private DataGridView _dgvShowtimes = null!;
        private ComboBox _cboMovies = null!;
        private ComboBox _cboRooms = null!;
        private DateTimePicker _dtpStartDate = null!;
        private DateTimePicker _dtpStartTime = null!;
        private NumericUpDown _numPrice = null!;
        private Button _btnAddShowtime = null!;
        private Button _btnDeleteShowtime = null!;
        private Button _btnUpdateShowtime = null!;
        private Label _lblEditing = null!;
        private CheckBox _chkShowPast = null!;
        private int _editingShowtimeId;
        private readonly ErrorProvider _errors = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        private readonly ShowtimeService _showtimeService;

        public FrmAdminShowtimes()
        {
            InitializeComponent();
            AppSession.RequireAdmin(this);
            _showtimeService = new ShowtimeService();
            SetupCustomUI();
            LoadComboBoxData();
            LoadShowtimes();
            WindowLayout.ApplyStandardSize(this);
        }

        private void SetupCustomUI()
        {
            Text = "CINELUXE - Quản Lý & Xếp Lịch Chiếu Phim";
            Size = new Size(1200, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = BgDark;
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 9.5F);

            Panel pnlInput = new Panel
            {
                Dock = DockStyle.Top,
                Height = 130,
                BackColor = BgCard,
                Padding = new Padding(15)
            };

            Label lblMovie = new Label { Text = "Chọn Phim:", Location = new Point(20, 15), AutoSize = true, ForeColor = TextDim };
            _cboMovies = new ComboBox { Location = new Point(20, 38), Width = 220, DropDownStyle = ComboBoxStyle.DropDownList, BackColor = BgSurface, ForeColor = Color.White };

            Label lblRoom = new Label { Text = "Chọn Phòng:", Location = new Point(260, 15), AutoSize = true, ForeColor = TextDim };
            _cboRooms = new ComboBox { Location = new Point(260, 38), Width = 160, DropDownStyle = ComboBoxStyle.DropDownList, BackColor = BgSurface, ForeColor = Color.White };

            Label lblDate = new Label { Text = "Ngày chiếu:", Location = new Point(440, 15), AutoSize = true, ForeColor = TextDim };
            _dtpStartDate = new DateTimePicker { Location = new Point(440, 38), Width = 140, Format = DateTimePickerFormat.Short, BackColor = BgSurface, ForeColor = Color.White };

            Label lblTime = new Label { Text = "Giờ bắt đầu:", Location = new Point(600, 15), AutoSize = true, ForeColor = TextDim };
            _dtpStartTime = new DateTimePicker { Location = new Point(600, 38), Width = 100, Format = DateTimePickerFormat.Custom, CustomFormat = "HH:mm", ShowUpDown = true, BackColor = BgSurface, ForeColor = Color.White };

            Label lblPrice = new Label { Text = "Giá vé (VNĐ):", Location = new Point(720, 15), AutoSize = true, ForeColor = TextDim };
            _numPrice = new NumericUpDown { Location = new Point(720, 38), Width = 130, Minimum = 30000, Maximum = 500000, Value = 90000, Increment = 5000, BackColor = BgSurface, ForeColor = Color.White };

            _btnAddShowtime = new Button
            {
                Text = "+ Xếp Lịch Chiếu",
                Location = new Point(870, 35),
                Size = new Size(150, 34),
                BackColor = Accent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };
            _btnAddShowtime.FlatAppearance.BorderSize = 0;
            _btnAddShowtime.Click += OnAddShowtimeClick;

            _btnDeleteShowtime = new Button
            {
                Text = "🗑 Xóa Suất Chiếu",
                Location = new Point(1030, 35),
                Size = new Size(130, 34),
                BackColor = Danger,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };
            _btnDeleteShowtime.FlatAppearance.BorderSize = 0;
            _btnDeleteShowtime.Click += OnDeleteShowtimeClick;

            _btnUpdateShowtime = new Button
            {
                Text = "💾 Lưu thay đổi suất đang chọn",
                Location = new Point(870, 80),
                Size = new Size(290, 34),
                BackColor = ColorTranslator.FromHtml("#334155"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Enabled = false
            };
            _btnUpdateShowtime.FlatAppearance.BorderSize = 0;
            _btnUpdateShowtime.Click += OnUpdateShowtimeClick;

            _lblEditing = new Label
            {
                Text = "Chọn 1 dòng trong bảng để sửa suất chiếu (suất đã bán vé chỉ được đổi giá).",
                Location = new Point(20, 88),
                AutoSize = true,
                ForeColor = TextDim
            };

            _chkShowPast = new CheckBox
            {
                Text = "Hiện cả suất đã chiếu",
                Location = new Point(620, 86),
                AutoSize = true,
                ForeColor = TextDim
            };
            _chkShowPast.CheckedChanged += (_, _) => LoadShowtimes();

            pnlInput.Controls.AddRange(new Control[] { _btnUpdateShowtime, _lblEditing, _chkShowPast });
            pnlInput.Controls.AddRange(new Control[] { 
                lblMovie, _cboMovies, lblRoom, _cboRooms, 
                lblDate, _dtpStartDate, lblTime, _dtpStartTime, 
                lblPrice, _numPrice, _btnAddShowtime, _btnDeleteShowtime 
            });

            _dgvShowtimes = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = BgSurface,
                GridColor = BorderCol,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true
            };
            _dgvShowtimes.DefaultCellStyle.BackColor = BgSurface;
            _dgvShowtimes.DefaultCellStyle.ForeColor = Color.White;
            _dgvShowtimes.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#1E293B");
            _dgvShowtimes.DefaultCellStyle.SelectionForeColor = ColorTranslator.FromHtml("#38BDF8");
            _dgvShowtimes.ColumnHeadersDefaultCellStyle.BackColor = BgCard;
            _dgvShowtimes.ColumnHeadersDefaultCellStyle.ForeColor = TextDim;
            _dgvShowtimes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _dgvShowtimes.RowTemplate.Height = 36;

            _dgvShowtimes.SelectionChanged += OnGridSelectionChanged;
            Controls.Add(_dgvShowtimes);
            Controls.Add(pnlInput);
        }

        private void LoadComboBoxData()
        {
            try
            {
                _cboMovies.DataSource = new MovieService().GetAll().Select(m => new { m.MovieId, m.Title }).ToList();
                _cboMovies.DisplayMember = "Title";
                _cboMovies.ValueMember = "MovieId";

                _cboRooms.DataSource = new RoomService().GetAll();
                _cboRooms.DisplayMember = "RoomName";
                _cboRooms.ValueMember = "RoomId";
                _cboRooms.SelectedIndexChanged += OnRoomChanged;
                OnRoomChanged(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách Phim/Phòng: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnRoomChanged(object? sender, EventArgs e)
        {
            if (_editingShowtimeId > 0 || _cboRooms.SelectedItem is not Room room) return;
            _numPrice.Value = Math.Clamp(RoomTypes.SuggestedPrice(room.RoomType), _numPrice.Minimum, _numPrice.Maximum);
        }

        private sealed class ShowtimeRow
        {
            public int ShowtimeId { get; init; }
            public string MovieTitle { get; init; } = "";
            public string Duration { get; init; } = "";
            public string RoomName { get; init; } = "";
            public string StartTime { get; init; } = "";
            public string EndTime { get; init; } = "";
            public string Price { get; init; } = "";
            public string Sold { get; init; } = "";
        }

        private void LoadShowtimes()
        {
            try
            {
                var list = _showtimeService.GetAllForAdmin(_chkShowPast.Checked)
                    .Select(s => new ShowtimeRow
                    {
                        ShowtimeId = s.ShowtimeId,
                        MovieTitle = s.MovieTitle,
                        Duration = s.Duration + " phút",
                        RoomName = s.RoomName,
                        StartTime = s.StartTime.ToString("dd/MM/yyyy HH:mm"),
                        EndTime = s.EndTime.ToString("HH:mm"),
                        Price = s.Price.ToString("N0") + " VNĐ",
                        Sold = $"{s.SoldSeats}/{s.TotalSeats}"
                    })
                    .ToList();

                _dgvShowtimes.DataSource = list;

                void Header(string col, string text)
                {
                    if (_dgvShowtimes.Columns[col] != null) _dgvShowtimes.Columns[col]!.HeaderText = text;
                }
                Header("ShowtimeId", "MÃ SUẤT");
                Header("MovieTitle", "TÊN PHIM");
                Header("Duration", "THỜI LƯỢNG");
                Header("RoomName", "PHÒNG CHIẾU");
                Header("StartTime", "GIỜ BẮT ĐẦU");
                Header("EndTime", "PHÒNG TRỐNG LÚC (gồm 15' dọn)");
                Header("Price", "GIÁ VÉ");
                Header("Sold", "ĐÃ BÁN");

                _dgvShowtimes.ClearSelection();
                ExitEditMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách suất chiếu: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnGridSelectionChanged(object? sender, EventArgs e)
        {
            if (_dgvShowtimes.SelectedRows.Count == 0 || _dgvShowtimes.SelectedRows[0].DataBoundItem is not ShowtimeRow row)
            {
                ExitEditMode();
                return;
            }

            try
            {
                var st = _showtimeService.GetById(row.ShowtimeId);
                if (st == null) return;

                _editingShowtimeId = st.ShowtimeId;
                _cboMovies.SelectedValue = st.MovieId;
                _cboRooms.SelectedValue = st.RoomId;
                _dtpStartDate.Value = st.StartTime.Date;
                _dtpStartTime.Value = DateTime.Today.Add(st.StartTime.TimeOfDay);
                _numPrice.Value = Math.Clamp(st.Price, _numPrice.Minimum, _numPrice.Maximum);

                _btnUpdateShowtime.Enabled = true;
                _btnUpdateShowtime.BackColor = Accent;
                _lblEditing.Text = st.SoldSeats > 0
                    ? $"Đang sửa suất #{st.ShowtimeId} - đã bán {st.SoldSeats} vé nên chỉ được đổi giá vé."
                    : $"Đang sửa suất #{st.ShowtimeId}. Đổi phim/phòng/giờ sẽ được kiểm tra trùng lịch.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không đọc được suất chiếu: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExitEditMode()
        {
            _editingShowtimeId = 0;
            if (_btnUpdateShowtime == null) return;
            _btnUpdateShowtime.Enabled = false;
            _btnUpdateShowtime.BackColor = ColorTranslator.FromHtml("#334155");
            _lblEditing.Text = "Chọn 1 dòng trong bảng để sửa suất chiếu (suất đã bán vé chỉ được đổi giá).";
        }

        private void OnUpdateShowtimeClick(object? sender, EventArgs e)
        {
            if (_editingShowtimeId <= 0) return;
            _errors.Clear();
            if (_cboMovies.SelectedValue is not int movieId || _cboRooms.SelectedValue is not int roomId)
            {
                _errors.SetError(_cboMovies, "Chọn đầy đủ phim và phòng chiếu");
                return;
            }

            DateTime start = _dtpStartDate.Value.Date.Add(_dtpStartTime.Value.TimeOfDay);
            start = new DateTime(start.Year, start.Month, start.Day, start.Hour, start.Minute, 0);

            try
            {
                _showtimeService.UpdateShowtime(_editingShowtimeId, movieId, roomId, start, _numPrice.Value);
                MessageBox.Show("Cập nhật suất chiếu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadShowtimes();
            }
            catch (BusinessException ex)
            {
                _errors.SetError(_dtpStartTime, ex.Message);
                MessageBox.Show(ex.Message, "Không thể cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật suất chiếu: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnAddShowtimeClick(object? sender, EventArgs e)
        {
            _errors.Clear();
            if (_cboMovies.SelectedValue == null || _cboRooms.SelectedValue == null)
            {
                if (_cboMovies.SelectedValue == null) _errors.SetError(_cboMovies, "Chưa chọn phim");
                if (_cboRooms.SelectedValue == null) _errors.SetError(_cboRooms, "Chưa chọn phòng chiếu");
                return;
            }

            int movieId = (int)_cboMovies.SelectedValue;
            int roomId = (int)_cboRooms.SelectedValue;

            DateTime selectedDate = _dtpStartDate.Value.Date;
            TimeSpan selectedTime = _dtpStartTime.Value.TimeOfDay;
            DateTime fullStartTime = selectedDate.Add(new TimeSpan(selectedTime.Hours, selectedTime.Minutes, 0));
            decimal price = _numPrice.Value;

            try
            {
                _showtimeService.AddShowtime(movieId, roomId, fullStartTime, price);
                MessageBox.Show("Thêm suất chiếu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadShowtimes();
            }
            catch (BusinessException ex)
            {
                _errors.SetError(_dtpStartTime, ex.Message);
                MessageBox.Show(ex.Message, "Cảnh báo xung đột", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm suất chiếu: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnDeleteShowtimeClick(object? sender, EventArgs e)
        {
            if (_dgvShowtimes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn suất chiếu muốn xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_dgvShowtimes.SelectedRows[0].DataBoundItem is not ShowtimeRow selectedRow) return;
            int showtimeId = selectedRow.ShowtimeId;

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa suất chiếu mã #{showtimeId}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _showtimeService.DeleteShowtime(showtimeId);
                    MessageBox.Show("Đã xóa suất chiếu thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadShowtimes();
                }
                catch (BusinessException ex)
                {
                    MessageBox.Show(ex.Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa suất chiếu: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
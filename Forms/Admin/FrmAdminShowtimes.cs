using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Services;
using Microsoft.EntityFrameworkCore;

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

        private readonly ShowtimeService _showtimeService;

        public FrmAdminShowtimes()
        {
            InitializeComponent();
            _showtimeService = new ShowtimeService();
            SetupCustomUI();
            LoadComboBoxData();
            LoadShowtimes();
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

            Controls.Add(_dgvShowtimes);
            Controls.Add(pnlInput);
        }

        private void LoadComboBoxData()
        {
            try
            {
                using var db = new CinemaDbContext();

                _cboMovies.DataSource = db.Movies.Select(m => new { m.MovieId, m.Title }).ToList();
                _cboMovies.DisplayMember = "Title";
                _cboMovies.ValueMember = "MovieId";

                _cboRooms.DataSource = db.Rooms.Select(r => new { r.RoomId, r.RoomName }).ToList();
                _cboRooms.DisplayMember = "RoomName";
                _cboRooms.ValueMember = "RoomId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách Phim/Phòng: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadShowtimes()
        {
            try
            {
                using var db = new CinemaDbContext();
                var list = db.Showtimes
                    .Include(s => s.Movie)
                    .Include(s => s.Room)
                    .OrderByDescending(s => s.StartTime)
                    .Select(s => new
                    {
                        s.ShowtimeId,
                        MovieTitle = s.Movie != null ? s.Movie.Title : "N/A",
                        Duration = s.Movie != null ? s.Movie.Duration + " phút" : "N/A",
                        RoomName = s.Room != null ? s.Room.RoomName : "N/A",
                        StartTime = s.StartTime.ToString("dd/MM/yyyy HH:mm"),
                        EndTime = s.EndTime.HasValue ? s.EndTime.Value.ToString("HH:mm") : "N/A",
                        Price = s.Price.ToString("N0") + " VNĐ"
                    }).ToList();

                _dgvShowtimes.DataSource = list;

                if (_dgvShowtimes.Columns["ShowtimeId"] != null) _dgvShowtimes.Columns["ShowtimeId"].HeaderText = "MÃ SUẤT";
                if (_dgvShowtimes.Columns["MovieTitle"] != null) _dgvShowtimes.Columns["MovieTitle"].HeaderText = "TÊN PHIM";
                if (_dgvShowtimes.Columns["Duration"] != null) _dgvShowtimes.Columns["Duration"].HeaderText = "THỜI LƯỢNG";
                if (_dgvShowtimes.Columns["RoomName"] != null) _dgvShowtimes.Columns["RoomName"].HeaderText = "PHÒNG CHIẾU";
                if (_dgvShowtimes.Columns["StartTime"] != null) _dgvShowtimes.Columns["StartTime"].HeaderText = "GIỜ BẮT ĐẦU";
                if (_dgvShowtimes.Columns["EndTime"] != null) _dgvShowtimes.Columns["EndTime"].HeaderText = "GIỜ KẾT THÚC";
                if (_dgvShowtimes.Columns["Price"] != null) _dgvShowtimes.Columns["Price"].HeaderText = "GIÁ VÉ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách suất chiếu: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnAddShowtimeClick(object? sender, EventArgs e)
        {
            if (_cboMovies.SelectedValue == null || _cboRooms.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ Phim và Phòng chiếu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int movieId = (int)_cboMovies.SelectedValue;
            int roomId = (int)_cboRooms.SelectedValue;

            DateTime selectedDate = _dtpStartDate.Value.Date;
            TimeSpan selectedTime = _dtpStartTime.Value.TimeOfDay;
            DateTime fullStartTime = selectedDate.Add(selectedTime);

            decimal price = _numPrice.Value;

            var result = _showtimeService.AddShowtime(movieId, roomId, fullStartTime, price);

            if (result.Success)
            {
                MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadShowtimes();
            }
            else
            {
                MessageBox.Show(result.Message, "Cảnh báo xung đột", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OnDeleteShowtimeClick(object? sender, EventArgs e)
        {
            if (_dgvShowtimes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn suất chiếu muốn xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int showtimeId = Convert.ToInt32(_dgvShowtimes.SelectedRows[0].Cells["ShowtimeId"].Value);

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa suất chiếu mã #{showtimeId}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                var result = _showtimeService.DeleteShowtime(showtimeId);
                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadShowtimes();
                }
                else
                {
                    MessageBox.Show(result.Message, "Lỗi xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
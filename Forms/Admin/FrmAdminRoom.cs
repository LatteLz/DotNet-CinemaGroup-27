using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CinemaManagement.Data;
using CinemaManagement.Models;

namespace CinemaManagement.Forms.Admin
{
    public partial class FrmAdminRooms : Form
    {
        private static readonly Color BgDark = ColorTranslator.FromHtml("#0B0F12");
        private static readonly Color BgSurface = ColorTranslator.FromHtml("#101418");
        private static readonly Color BgCard = ColorTranslator.FromHtml("#181C20");
        private static readonly Color BorderCol = ColorTranslator.FromHtml("#2A323D");
        private static readonly Color Accent = ColorTranslator.FromHtml("#0284C7");
        private static readonly Color TextDim = ColorTranslator.FromHtml("#94A3B8");

        private DataGridView dgvRooms = null!;
        private TextBox txtRoomName = null!;
        private ComboBox cboRoomType = null!;
        private NumericUpDown numRows = null!;
        private NumericUpDown numSeatsPerRow = null!;
        private Button btnAddRoom = null!;

        public FrmAdminRooms()
        {
            InitializeComponent();
            SetupCustomUI();
            LoadRooms();
        }

        private void SetupCustomUI()
        {
            this.Text = "CINELUXE - Quản Lý Phòng Chiếu & Tự Động Sinh Ghế";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = BgDark;
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5F);

            Panel pnlInput = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = BgCard,
                Padding = new Padding(15)
            };

            Label lblName = new Label { Text = "Tên phòng chiếu:", Location = new Point(20, 20), AutoSize = true, ForeColor = TextDim };
            txtRoomName = new TextBox { Location = new Point(20, 45), Width = 180, BackColor = BgSurface, ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            Label lblType = new Label { Text = "Loại phòng:", Location = new Point(220, 20), AutoSize = true, ForeColor = TextDim };
            cboRoomType = new ComboBox { Location = new Point(220, 45), Width = 160, DropDownStyle = ComboBoxStyle.DropDownList, BackColor = BgSurface, ForeColor = Color.White };
            cboRoomType.Items.AddRange(new object[] { "Standard 2D", "IMAX Laser", "VIP Suite", "4DX" });
            cboRoomType.SelectedIndex = 0;

            Label lblRows = new Label { Text = "Số hàng ghế (A-Z):", Location = new Point(400, 20), AutoSize = true, ForeColor = TextDim };
            numRows = new NumericUpDown { Location = new Point(400, 45), Width = 120, Minimum = 1, Maximum = 15, Value = 5, BackColor = BgSurface, ForeColor = Color.White };

            Label lblSeatsPerRow = new Label { Text = "Số ghế / Hàng:", Location = new Point(540, 20), AutoSize = true, ForeColor = TextDim };
            numSeatsPerRow = new NumericUpDown { Location = new Point(540, 45), Width = 120, Minimum = 1, Maximum = 20, Value = 10, BackColor = BgSurface, ForeColor = Color.White };

            btnAddRoom = new Button
            {
                Text = "+ Tạo Phòng & Sinh Ghế",
                Location = new Point(680, 42),
                Size = new Size(180, 32),
                BackColor = Accent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };
            btnAddRoom.FlatAppearance.BorderSize = 0;
            btnAddRoom.Click += (s, e) => AddRoomAndGenerateSeats();

            pnlInput.Controls.AddRange(new Control[] { lblName, txtRoomName, lblType, cboRoomType, lblRows, numRows, lblSeatsPerRow, numSeatsPerRow, btnAddRoom });

            dgvRooms = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = BgSurface,
                GridColor = BorderCol,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true
            };
            dgvRooms.DefaultCellStyle.BackColor = BgSurface;
            dgvRooms.DefaultCellStyle.ForeColor = Color.White;
            dgvRooms.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#1E293B");
            dgvRooms.DefaultCellStyle.SelectionForeColor = ColorTranslator.FromHtml("#38BDF8");
            dgvRooms.ColumnHeadersDefaultCellStyle.BackColor = BgCard;
            dgvRooms.ColumnHeadersDefaultCellStyle.ForeColor = TextDim;
            dgvRooms.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvRooms.RowTemplate.Height = 36;

            this.Controls.Add(dgvRooms);
            this.Controls.Add(pnlInput);
        }

        private void LoadRooms()
        {
            try
            {
                using var db = new CinemaDbContext();
                var list = db.Rooms.Select(r => new
                {
                    r.RoomId,
                    r.RoomName,
                    r.RoomType,
                    r.RowCount,
                    r.ColCount,
                    r.TotalSeats,
                    SeatsInDb = db.Seats.Count(s => s.RoomId == r.RoomId)
                }).ToList();

                dgvRooms.DataSource = list;
                if (dgvRooms.Columns["RoomId"] != null) dgvRooms.Columns["RoomId"].HeaderText = "MÃ PHÒNG";
                if (dgvRooms.Columns["RoomName"] != null) dgvRooms.Columns["RoomName"].HeaderText = "TÊN PHÒNG CHIẾU";
                if (dgvRooms.Columns["RoomType"] != null) dgvRooms.Columns["RoomType"].HeaderText = "LOẠI PHÒNG";
                if (dgvRooms.Columns["RowCount"] != null) dgvRooms.Columns["RowCount"].HeaderText = "SỐ HÀNG";
                if (dgvRooms.Columns["ColCount"] != null) dgvRooms.Columns["ColCount"].HeaderText = "SỐ CỘT";
                if (dgvRooms.Columns["TotalSeats"] != null) dgvRooms.Columns["TotalSeats"].HeaderText = "TỔNG SỐ GHẾ LÝ THUYẾT";
                if (dgvRooms.Columns["SeatsInDb"] != null) dgvRooms.Columns["SeatsInDb"].HeaderText = "SỐ GHẾ THỰC TẾ TRONG DB";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách phòng: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddRoomAndGenerateSeats()
        {
            string roomName = txtRoomName.Text.Trim();
            if (string.IsNullOrWhiteSpace(roomName))
            {
                MessageBox.Show("Vui lòng nhập tên phòng chiếu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int rowsCount = (int)numRows.Value;
            int seatsPerRow = (int)numSeatsPerRow.Value;
            int totalSeats = rowsCount * seatsPerRow;
            string roomType = cboRoomType.SelectedItem?.ToString() ?? "Standard 2D";

            try
            {
                using var db = new CinemaDbContext();
                var room = new Room
                {
                    RoomName = roomName,
                    RoomType = roomType,
                    RowCount = rowsCount,
                    ColCount = seatsPerRow,
                    TotalSeats = totalSeats
                };
                db.Rooms.Add(room);
                db.SaveChanges(); 

                GenerateSeatsForRoom(db, room.RoomId, rowsCount, seatsPerRow);

                MessageBox.Show($"Đã khởi tạo phòng '{roomName}' và sinh tự động {totalSeats} ghế thành công vào CSDL!", 
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtRoomName.Clear();
                LoadRooms();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo phòng và sinh ghế: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateSeatsForRoom(CinemaDbContext db, int roomId, int rowsCount, int seatsPerRow)
        {
            string[] rowLetters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O" };
            var seatList = new List<Seat>();

            for (int r = 0; r < rowsCount && r < rowLetters.Length; r++)
            {
                string rowLetter = rowLetters[r];
                bool isVipRow = (r >= rowsCount - 2 && rowsCount > 2);
                string seatType = isVipRow ? "VIP" : "Standard";
                decimal basePrice = isVipRow ? 140000m : 110000m;

                for (int s = 1; s <= seatsPerRow; s++)
                {
                    seatList.Add(new Seat
                    {
                        RoomId = roomId,
                        SeatNumber = $"{rowLetter}{s}",
                        SeatType = seatType,
                        Price = basePrice,
                        IsAvailable = true
                    });
                }
            }

            db.Seats.AddRange(seatList);
            db.SaveChanges();
        }
    }
}
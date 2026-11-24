using CinemaManagement.Helpers;
using CinemaManagement.Models;
using CinemaManagement.Services;

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

        private readonly RoomService _roomService = new();
        private readonly ErrorProvider _errors = new();

        private DataGridView dgvRooms = null!;
        private TextBox txtRoomName = null!;
        private ComboBox cboRoomType = null!;
        private NumericUpDown numRows = null!;
        private NumericUpDown numSeatsPerRow = null!;
        private NumericUpDown numVipRows = null!;
        private CheckBox chkCoupleRow = null!;
        private Button btnAddRoom = null!;
        private Button btnDeleteRoom = null!;

        public FrmAdminRooms()
        {
            InitializeComponent();
            AppSession.RequireAdmin(this);
            SetupCustomUI();
            LoadRooms();
            WindowLayout.ApplyStandardSize(this);
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
            cboRoomType.Items.AddRange(RoomTypes.All);
            cboRoomType.SelectedIndex = 0;

            Label lblRows = new Label { Text = "Số hàng (A-Z):", Location = new Point(400, 20), AutoSize = true, ForeColor = TextDim };
            numRows = new NumericUpDown { Location = new Point(400, 45), Width = 90, Minimum = 1, Maximum = RoomService.MaxRows, Value = 8, BackColor = BgSurface, ForeColor = Color.White };

            Label lblSeatsPerRow = new Label { Text = "Ghế / hàng:", Location = new Point(505, 20), AutoSize = true, ForeColor = TextDim };
            numSeatsPerRow = new NumericUpDown { Location = new Point(505, 45), Width = 90, Minimum = 1, Maximum = RoomService.MaxCols, Value = 10, BackColor = BgSurface, ForeColor = Color.White };

            Label lblVip = new Label { Text = "Số hàng VIP:", Location = new Point(610, 20), AutoSize = true, ForeColor = TextDim };
            numVipRows = new NumericUpDown { Location = new Point(610, 45), Width = 90, Minimum = 0, Maximum = RoomService.MaxRows, Value = 2, BackColor = BgSurface, ForeColor = Color.White };

            chkCoupleRow = new CheckBox { Text = "Hàng cuối là ghế Đôi", Location = new Point(400, 80), AutoSize = true, ForeColor = Color.White, Checked = true };

            btnAddRoom = new Button
            {
                Text = "+ Tạo Phòng & Sinh Ghế",
                Location = new Point(720, 42),
                Size = new Size(180, 32),
                BackColor = Accent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };
            btnAddRoom.FlatAppearance.BorderSize = 0;
            btnAddRoom.Click += (s, e) => AddRoomAndGenerateSeats();

            btnDeleteRoom = new Button
            {
                Text = "🗑 Xóa phòng",
                Location = new Point(910, 42),
                Size = new Size(130, 32),
                BackColor = ColorTranslator.FromHtml("#EF4444"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };
            btnDeleteRoom.FlatAppearance.BorderSize = 0;
            btnDeleteRoom.Click += (s, e) => DeleteSelectedRoom();

            pnlInput.Controls.AddRange(new Control[] { lblName, txtRoomName, lblType, cboRoomType, lblRows, numRows, lblSeatsPerRow, numSeatsPerRow, lblVip, numVipRows, chkCoupleRow, btnAddRoom, btnDeleteRoom });

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
                dgvRooms.DataSource = _roomService.GetSummaries();

                void Header(string col, string text)
                {
                    if (dgvRooms.Columns[col] != null) dgvRooms.Columns[col]!.HeaderText = text;
                }
                Header(nameof(RoomSummaryDto.RoomId), "MÃ PHÒNG");
                Header(nameof(RoomSummaryDto.RoomName), "TÊN PHÒNG CHIẾU");
                Header(nameof(RoomSummaryDto.RoomType), "LOẠI PHÒNG");
                Header(nameof(RoomSummaryDto.RowCount), "SỐ HÀNG");
                Header(nameof(RoomSummaryDto.ColCount), "SỐ CỘT");
                Header(nameof(RoomSummaryDto.StandardSeats), "GHẾ THƯỜNG");
                Header(nameof(RoomSummaryDto.VipSeats), "GHẾ VIP");
                Header(nameof(RoomSummaryDto.CoupleSeats), "GHẾ ĐÔI");
                Header(nameof(RoomSummaryDto.TotalSeats), "TỔNG SỐ GHẾ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách phòng: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddRoomAndGenerateSeats()
        {
            _errors.Clear();
            string roomName = txtRoomName.Text.Trim();
            if (string.IsNullOrWhiteSpace(roomName))
            {
                _errors.SetError(txtRoomName, "Vui lòng nhập tên phòng chiếu!");
                txtRoomName.Focus();
                return;
            }

            int rows = (int)numRows.Value;
            int cols = (int)numSeatsPerRow.Value;
            int vipRows = (int)numVipRows.Value;
            bool coupleRow = chkCoupleRow.Checked;

            if (vipRows > rows)
            {
                _errors.SetError(numVipRows, "Số hàng VIP không được lớn hơn số hàng ghế");
                return;
            }
            if (coupleRow && cols < 2)
            {
                _errors.SetError(chkCoupleRow, "Hàng ghế đôi cần ít nhất 2 ghế mỗi hàng");
                return;
            }

            try
            {
                var room = _roomService.CreateRoom(roomName, cboRoomType.SelectedItem?.ToString() ?? RoomTypes.Standard, rows, cols, vipRows, coupleRow);
                int seatCount = _roomService.GetSummaries().FirstOrDefault(r => r.RoomId == room.RoomId)?.TotalSeats ?? 0;

                MessageBox.Show($"Đã tạo phòng '{roomName}' và tự động sinh {seatCount} ghế!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtRoomName.Clear();
                LoadRooms();
            }
            catch (BusinessException ex)
            {
                MessageBox.Show(ex.Message, "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo phòng và sinh ghế: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteSelectedRoom()
        {
            if (dgvRooms.CurrentRow?.DataBoundItem is not RoomSummaryDto room)
            {
                MessageBox.Show("Vui lòng chọn phòng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show($"Xóa phòng '{room.RoomName}' cùng toàn bộ {room.TotalSeats} ghế?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                _roomService.DeleteRoom(room.RoomId);
                LoadRooms();
            }
            catch (BusinessException ex)
            {
                MessageBox.Show(ex.Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa phòng: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

using CinemaManagement.Models;
using CinemaManagement.Services;

namespace CinemaManagement.Forms.Shared
{
    public class FrmAudiencePicker : Form
    {
        private static readonly Color Bg = ColorTranslator.FromHtml("#181c20");
        private static readonly Color Surface = ColorTranslator.FromHtml("#101418");
        private static readonly Color Muted = ColorTranslator.FromHtml("#94a3b8");
        private static readonly Color Accent = ColorTranslator.FromHtml("#ffb961");

        private const int RowHeight = 46;
        private const int MaxVisibleRows = 6;

        private readonly IReadOnlyList<SeatStatusDto> _seats;
        private readonly decimal _showtimePrice;
        private readonly string _roomType;
        private readonly DateTime _start;
        private readonly Dictionary<int, ComboBox> _pickers = new();
        private readonly Dictionary<int, Label> _priceLabels = new();
        private readonly Label _lblTotal;

        public Dictionary<int, string> Selections { get; } = new();
        public decimal Total { get; private set; }

        public FrmAudiencePicker(IReadOnlyList<SeatStatusDto> seats, decimal showtimePrice, string roomType, DateTime start,
            IReadOnlyDictionary<int, string>? current = null)
        {
            _seats = seats;
            _showtimePrice = showtimePrice;
            _roomType = roomType;
            _start = start;
            bool premium = TicketPricing.IsPremiumFormat(roomType);

            Text = "Chọn đối tượng vé";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Bg;
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 10F);

            int listHeight = Math.Min(seats.Count, MaxVisibleRows) * RowHeight + 8;
            ClientSize = new Size(620, 128 + listHeight + 150);

            var lblTitle = new Label
            {
                Text = "CHỌN ĐỐI TƯỢNG CHO TỪNG VÉ",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Accent,
                Location = new Point(20, 14),
                AutoSize = true
            };

            var lblNote = new Label
            {
                Text = premium
                    ? $"Phòng {roomType}: mọi đối tượng cùng một giá."
                    : $"Suất {start:HH:mm dd/MM} ({DayTypes.ToDisplay(DayTypes.Of(start))}). U22 và trẻ em được miễn phụ thu ghế VIP.",
                ForeColor = Muted,
                Location = new Point(20, 50),
                Size = new Size(580, 24)
            };

            var lblAll = new Label { Text = "Đặt tất cả là:", ForeColor = Muted, Location = new Point(20, 88), AutoSize = true };
            var cboAll = CreatePicker();
            cboAll.Location = new Point(150, 84);
            cboAll.Enabled = !premium;
            cboAll.SelectionChangeCommitted += (_, _) =>
            {
                foreach (var picker in _pickers.Values) picker.SelectedIndex = cboAll.SelectedIndex;
            };

            var list = new FlowLayoutPanel
            {
                Location = new Point(20, 122),
                Size = new Size(580, listHeight),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Surface
            };

            foreach (var seat in seats)
            {
                var row = new Panel { Size = new Size(550, RowHeight - 4), Margin = new Padding(4, 4, 4, 0) };
                var lblSeat = new Label
                {
                    Text = $"Ghế {seat.SeatNumber}  •  {SeatTypes.ToDisplay(seat.SeatType)}",
                    Location = new Point(8, 10),
                    Size = new Size(200, 24)
                };
                var picker = CreatePicker();
                picker.Location = new Point(215, 7);
                picker.SelectedIndex = premium ? 0 : Array.IndexOf(AudienceTypes.All, AudienceTypes.Normalize(current?.GetValueOrDefault(seat.SeatId)));
                picker.Enabled = !premium;
                picker.SelectedIndexChanged += (_, _) => Recalculate();
                var lblPrice = new Label
                {
                    Location = new Point(410, 10),
                    Size = new Size(130, 24),
                    TextAlign = ContentAlignment.MiddleRight,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };

                row.Controls.AddRange(new Control[] { lblSeat, picker, lblPrice });
                list.Controls.Add(row);
                _pickers[seat.SeatId] = picker;
                _priceLabels[seat.SeatId] = lblPrice;
            }

            var lblRules = new Label
            {
                Text = "U22: 12–22 tuổi, có thẻ thành viên hoặc CCCD  •  Trẻ em: trên 2 tuổi, cao dưới 1m30  •  " +
                       "Người cao tuổi: trên 55 tuổi, có giấy tờ. Nhân viên soát vé kiểm tra giấy tờ khi vào phòng.",
                ForeColor = Muted,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(20, list.Bottom + 8),
                Size = new Size(580, 42)
            };

            _lblTotal = new Label
            {
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Accent,
                Location = new Point(20, lblRules.Bottom + 12),
                AutoSize = true
            };

            var btnOk = new Button
            {
                Text = "Xác nhận",
                DialogResult = DialogResult.OK,
                Size = new Size(130, 38),
                Location = new Point(ClientSize.Width - 270, ClientSize.Height - 52),
                BackColor = Accent,
                ForeColor = Surface,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.Click += (_, _) =>
            {
                Selections.Clear();
                foreach (var seat in _seats) Selections[seat.SeatId] = AudienceAt(_pickers[seat.SeatId]);
            };

            var btnCancel = new Button
            {
                Text = "Hủy",
                DialogResult = DialogResult.Cancel,
                Size = new Size(110, 38),
                Location = new Point(ClientSize.Width - 130, ClientSize.Height - 52),
                BackColor = ColorTranslator.FromHtml("#334155"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            Controls.AddRange(new Control[] { lblTitle, lblNote, lblAll, cboAll, list, lblRules, _lblTotal, btnOk, btnCancel });
            AcceptButton = btnOk;
            CancelButton = btnCancel;

            Recalculate();
        }

        private static ComboBox CreatePicker()
        {
            var cbo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 185,
                BackColor = Surface,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            cbo.Items.AddRange(AudienceTypes.All.Select(AudienceTypes.ToDisplay).Cast<object>().ToArray());
            return cbo;
        }

        private static string AudienceAt(ComboBox picker)
            => picker.SelectedIndex >= 0 ? AudienceTypes.All[picker.SelectedIndex] : AudienceTypes.Adult;

        private void Recalculate()
        {
            Total = 0;
            foreach (var seat in _seats)
            {
                decimal price = TicketPricing.TicketPrice(_showtimePrice, _roomType, _start, seat.SeatType, AudienceAt(_pickers[seat.SeatId]));
                _priceLabels[seat.SeatId].Text = $"{price:N0} đ";
                Total += price;
            }
            _lblTotal.Text = $"TỔNG TIỀN VÉ ({_seats.Count} vé): {Total:N0} đ";
        }
    }
}

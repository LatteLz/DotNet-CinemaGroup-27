using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;
using CinemaManagement.Models;

namespace CinemaManagement.Forms.Kiosk;

public partial class FrmKioskPayment : Form
    {
        private readonly int _showtimeId;
        private readonly List<string> _selectedSeats;
        private readonly decimal _ticketTotal;
        private readonly List<KioskComboSelection> _selectedCombos;
        private readonly string _sessionId;
        private string _paymentMethod = "QR_MOMO";
        private System.Windows.Forms.Timer _timer;

        private decimal ComboTotal => _selectedCombos.Sum(c => c.UnitPrice * c.Quantity);
        private decimal GrandTotal => _ticketTotal + ComboTotal;

        public FrmKioskPayment(
            int showtimeId,
            List<string> selectedSeats,
            decimal ticketTotal,
            List<KioskComboSelection>? selectedCombos = null,
            string sessionId = "")
        {
            _showtimeId = showtimeId;
            _selectedSeats = selectedSeats ?? new List<string>();
            _ticketTotal = ticketTotal;
            _selectedCombos = selectedCombos ?? new List<KioskComboSelection>();
            _sessionId = sessionId;

            InitializeComponent();
            UpdateDynamicTexts();
            RegisterEvents();
            LoadOrderSummary();
        }

        private void UpdateDynamicTexts()
        {
            btnConfirmPay.Text = $"XÁC NHẬN ĐÃ THANH TOÁN ({GrandTotal:N0} VNĐ)";

            qrPlaceholder.Controls.Clear();
            qrPlaceholder.Padding = new Padding(8);

            PictureBox picQrPayment = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };
            
            string[] possibleFiles = { "My qr.jpg", "my_qr.png", "my_qr.jpg", "my_qr.jpeg" };
            string finalPath = "";

            foreach (var fileName in possibleFiles)
            {
                string path1 = System.IO.Path.Combine(Application.StartupPath, "Image", fileName);
                string path2 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Image", fileName);

                if (System.IO.File.Exists(path1)) { finalPath = path1; break; }
                if (System.IO.File.Exists(path2)) { finalPath = path2; break; }
            }

            if (!string.IsNullOrEmpty(finalPath))
            {
                using var stream = new System.IO.FileStream(finalPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                picQrPayment.Image = Image.FromStream(stream);
            }

            qrPlaceholder.Controls.Add(picQrPayment);
        }

        private void RegisterEvents()
        {
            btnBack.Click += (s, e) =>
            {
                _timer?.Stop();
                var comboForm = Application.OpenForms.OfType<FrmKioskCombo>().FirstOrDefault();
                if (comboForm != null)
                {
                    comboForm.TopMost = true;
                    comboForm.Show();
                    comboForm.Activate();
                }
                else
                {
                    var newComboForm = new FrmKioskCombo(_showtimeId, _selectedSeats, _ticketTotal);
                    newComboForm.TopMost = true;
                    newComboForm.Show();
                }
                this.Close();
            };

            btnCancel.Click += (s, e) =>
            {
                if (MessageBox.Show("Bạn có chắc muốn hủy đơn hàng và quay về trang chủ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _timer?.Stop();
                    ReleaseSeatHolds();
                    new FrmKioskHome().Show();
                    this.Close();
                }
            };

            btnQr.Click += (s, e) => SelectPaymentMethod("QR_MOMO", "Quét mã QR Ngân hàng / Ví điện tử", btnQr, new[] { btnCard, btnCash });
            btnCard.Click += (s, e) => SelectPaymentMethod("CARD_POS", "Chạm hoặc quẹt thẻ vào đầu đọc bên dưới", btnCard, new[] { btnQr, btnCash });
            btnCash.Click += (s, e) => SelectPaymentMethod("COUNTER_CASH", "Nhận mã đặt chỗ và thanh toán tiền mặt tại quầy", btnCash, new[] { btnQr, btnCard });

            btnConfirmPay.Click += BtnConfirmPay_Click;

            _timer = new System.Windows.Forms.Timer { Interval = 1000 };
            _timer.Tick += (s, e) =>
            {
                if (lblClock != null && !lblClock.IsDisposed)
                    lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
            };
            _timer.Start();

            this.FormClosed += (_, _) => _timer?.Dispose();
        }

        private void SelectPaymentMethod(string code, string desc, Button activeBtn, Button[] otherBtns)
        {
            _paymentMethod = code;
            lblSelectedMethod.Text = $"Phương thức đang chọn: {desc}";
            activeBtn.BackColor = ColorTranslator.FromHtml("#1a5276");
            activeBtn.ForeColor = Color.White;

            foreach (var b in otherBtns)
            {
                b.BackColor = ColorTranslator.FromHtml("#262a2f");
                b.ForeColor = ColorTranslator.FromHtml("#c1c7cf");
            }

            if (code == "QR_MOMO")
                lblQrInstruction.Text = "Mở ứng dụng Ngân hàng hoặc Ví điện tử để quét mã QR.\nHệ thống sẽ tự động xuất vé ngay sau khi giao dịch thành công.";
            else if (code == "CARD_POS")
                lblQrInstruction.Text = "Vui lòng chạm thẻ (Contactless) hoặc cắm thẻ chip vào thiết bị POS bên dưới màn hình.";
            else
                lblQrInstruction.Text = "Bấm xác nhận bên dưới để in phiếu tạm, sau đó mang phiếu đến quầy thu ngân thanh toán trong 15 phút.";
        }

        private void LoadOrderSummary()
        {
            pnlOrderDetails.Controls.Clear();
            using var db = new CinemaDbContext();

            var showtime = db.Showtimes
                .Include(st => st.Movie)
                .Include(st => st.Room)
                .FirstOrDefault(st => st.ShowtimeId == _showtimeId);

            int yPos = 10;

            void AddLine(string label, string value, bool isBold = false, Color? valueColor = null, int fontSize = 11)
            {
                Label lblLeft = new Label
                {
                    Text = label,
                    Font = new Font("Segoe UI", fontSize, isBold ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = ColorTranslator.FromHtml("#8b9198"),
                    Location = new Point(0, yPos),
                    AutoSize = true
                };

                int rightX = fontSize >= 15 ? 310 : 240;

                Label lblRight = new Label
                {
                    Text = value,
                    Font = new Font("Segoe UI", fontSize, isBold ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = valueColor ?? ColorTranslator.FromHtml("#e0e3e8"),
                    Location = new Point(rightX, yPos),
                    MaximumSize = new Size(680 - rightX, 0),
                    AutoSize = true
                };

                pnlOrderDetails.Controls.Add(lblLeft);
                pnlOrderDetails.Controls.Add(lblRight);
                yPos += Math.Max(lblLeft.Height, lblRight.Height) + 16;
            }

            if (showtime != null)
            {
                AddLine("Phim:", showtime.Movie?.Title.ToUpper() ?? "N/A", true, Color.White, 13);
                AddLine("Phòng chiếu:", showtime.Room?.RoomName ?? "N/A");
                AddLine("Suất chiếu:", showtime.StartTime.ToString("HH:mm - dd/MM/yyyy"));
            }

            AddLine("Ghế đã chọn:", string.Join(", ", _selectedSeats), true, ColorTranslator.FromHtml("#38bdf8"));
            AddLine($"Tiền vé ({_selectedSeats.Count} ghế):", $"{_ticketTotal:N0} VNĐ", true);

            if (_selectedCombos.Any())
            {
                yPos += 10;
                AddLine("Bắp & Nước:", "", true, ColorTranslator.FromHtml("#ffb961"));
                foreach (var combo in _selectedCombos)
                {
                    AddLine($"  • {combo.ComboName} (x{combo.Quantity})", $"{(combo.UnitPrice * combo.Quantity):N0} VNĐ");
                }
            }

            Panel divider = new Panel
            {
                Size = new Size(680, 2),
                Location = new Point(0, yPos + 10),
                BackColor = ColorTranslator.FromHtml("#262a2f")
            };
            pnlOrderDetails.Controls.Add(divider);
            yPos += 30;

            AddLine("TỔNG THANH TOÁN:", $"{GrandTotal:N0} VNĐ", true, ColorTranslator.FromHtml("#ffb961"), 16);
        }

       private void BtnConfirmPay_Click(object? sender, EventArgs e)
{
    btnConfirmPay.Enabled = false;
    btnConfirmPay.Text = "ĐANG XỬ LÝ GIAO DỊCH...";

    try
    {
        using var db = new CinemaDbContext();
        using var transaction = db.Database.BeginTransaction();

        var showtime = db.Showtimes
            .Include(s => s.Movie)
            .Include(s => s.Room)
            .FirstOrDefault(s => s.ShowtimeId == _showtimeId);

        if (showtime == null)
            throw new Exception("Suất chiếu không tồn tại!");

        // 1. Lấy danh sách ghế của phòng & danh sách SeatId đã bán
        var roomSeats = db.Seats.Where(s => s.RoomId == showtime.RoomId).ToList();
        var soldSeatIds = db.Tickets
            .Where(t => t.ShowtimeId == _showtimeId)
            .Select(t => t.SeatId)
            .ToHashSet();

        // 2. Kiểm tra chống bán trùng ghế trong transaction
        var selectedSeatEntities = new List<Seat>();
        foreach (var seatCode in _selectedSeats)
        {
            var seatEntity = roomSeats.FirstOrDefault(s =>
                s.SeatNumber != null && s.SeatNumber.ToString().Equals(seatCode, StringComparison.OrdinalIgnoreCase));

            if (seatEntity != null)
            {
                if (soldSeatIds.Contains(seatEntity.SeatId))
                {
                    throw new Exception($"Ghế {seatCode} vừa được khách hàng khác thanh toán! Giao dịch đã được hủy.");
                }
                selectedSeatEntities.Add(seatEntity);
            }
        }

        // 3. Tạo đơn đặt vé (Booking)
        string movieTitle = showtime.Movie?.Title ?? "N/A";
        string showtimeInfo = $"{showtime.Room?.RoomName} | {showtime.StartTime:HH:mm dd/MM/yyyy}";

        var booking = new Booking
        {
            MovieTitle = movieTitle,
            ShowtimeInfo = showtimeInfo,
            TotalAmount = GrandTotal,
            CreatedAt = DateTime.Now
        };
        db.Bookings.Add(booking);
        db.SaveChanges();

        // 4. Lưu chi tiết vé (Tickets)
        decimal pricePerTicket = _selectedSeats.Count > 0 ? _ticketTotal / _selectedSeats.Count : 0;
        foreach (var seatEntity in selectedSeatEntities)
        {
            db.Tickets.Add(new Ticket
            {
                BookingId = booking.Id,
                ShowtimeId = _showtimeId,
                SeatId = seatEntity.SeatId,
                Price = pricePerTicket
            });
        }

        // 5. Lưu chi tiết combo bắp nước (BookingCombos)
        foreach (var combo in _selectedCombos)
        {
            db.BookingCombos.Add(new BookingCombo
            {
                BookingId = booking.Id,
                ComboId = combo.ComboId,
                Quantity = combo.Quantity,
                UnitPrice = combo.UnitPrice
            });
        }

        // 6. Lưu thông tin thanh toán (Payment)
        db.Payments.Add(new Payment
        {
            BookingId = booking.Id,
            Amount = GrandTotal,
            PaymentMethod = _paymentMethod,
            PaymentDate = DateTime.Now,
            Status = _paymentMethod == "COUNTER_CASH" ? "Pending" : "Completed"
        });

        // 7. Xóa ghế đang giữ tạm (SeatHolds)
        var holds = string.IsNullOrEmpty(_sessionId)
            ? db.SeatHolds.Where(h => h.ShowtimeId == _showtimeId).ToList()
            : db.SeatHolds.Where(h => h.ShowtimeId == _showtimeId && h.SessionId == _sessionId).ToList();

        if (holds.Any())
        {
            db.SeatHolds.RemoveRange(holds);
        }

        db.SaveChanges();
        transaction.Commit();

        // 8. Chuyển sang màn hình xuất vé thành công & QR Code
        _timer?.Stop();
        var frmSuccess = new FrmKioskSuccess(
            booking.Id,
            movieTitle,
            showtimeInfo,
            _selectedSeats,
            GrandTotal,
            _paymentMethod
        );
        frmSuccess.TopMost = true;
        frmSuccess.Show();
        this.Close();
    }
    catch (Exception ex)
    {
        btnConfirmPay.Enabled = true;
        btnConfirmPay.Text = $"XÁC NHẬN ĐÃ THANH TOÁN ({GrandTotal:N0} VNĐ)";
        MessageBox.Show($"Không thể hoàn tất giao dịch: {ex.Message}", "Cảnh báo đặt vé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}

        private void ReleaseSeatHolds()
        {
            if (string.IsNullOrEmpty(_sessionId)) return;
            try
            {
                using var db = new CinemaDbContext();
                var holds = db.SeatHolds.Where(h => h.ShowtimeId == _showtimeId && h.SessionId == _sessionId).ToList();
                db.SeatHolds.RemoveRange(holds);
                db.SaveChanges();
            }
            catch { }
        }
    }

    public class KioskComboSelection
    {
        public int ComboId { get; set; }
        public string ComboName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
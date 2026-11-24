using CinemaManagement.Helpers;
using CinemaManagement.Models;
using CinemaManagement.Services;

namespace CinemaManagement.Forms.Kiosk;

public partial class FrmKioskPayment : Form
{
    private readonly KioskOrder _order;
    private readonly BookingService _bookingService = new();
    private readonly ShowtimeService _showtimeService = new();
    private string _paymentMethod = PaymentMethods.QrTransfer;
    private System.Windows.Forms.Timer _timer = null!;
    private PictureBox? _picQrPayment;

    public FrmKioskPayment(KioskOrder order)
    {
        _order = order;

        InitializeComponent();

        UpdateDynamicTexts();
        RegisterEvents();
        LoadOrderSummary();
        WindowLayout.ScaleToScreen(this);
    }

    private void UpdateDynamicTexts()
    {
        btnConfirmPay.Text = $"XÁC NHẬN ĐÃ THANH TOÁN ({_order.GrandTotal:N0} VNĐ)";

        qrPlaceholder.Controls.Clear();
        qrPlaceholder.Padding = new Padding(8);
        _picQrPayment = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.White
        };
        qrPlaceholder.Controls.Add(_picQrPayment);
        RenderPaymentQr();
    }

    private void RenderPaymentQr()
    {
        if (_picQrPayment == null) return;
        try
        {
            var old = _picQrPayment.Image;
            _picQrPayment.Image = QrHelper.Generate(
                QrHelper.BuildMockPaymentPayload(_order.SessionId[..8].ToUpper(), _order.GrandTotal), 8);
            old?.Dispose();
        }
        catch (Exception ex)
        {
            lblQrInstruction.Text = "Không tạo được mã QR: " + ex.Message;
        }
    }

    private void RegisterEvents()
    {
        btnBack.Click += (_, _) =>
        {
            _timer.Stop();
            var comboForm = Application.OpenForms.OfType<FrmKioskCombo>().FirstOrDefault();
            if (comboForm != null)
            {
                comboForm.Show();
                comboForm.Activate();
            }
            Close();
        };

        btnCancel.Click += (_, _) =>
        {
            if (MessageBox.Show("Bạn có chắc muốn hủy đơn hàng và quay về trang chủ?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            _timer.Stop();
            try { new SeatHoldService().ReleaseSession(_order.SessionId); } catch { }
            KioskFlow.ReturnHome(this);
        };

        btnQr.Click += (_, _) => SelectPaymentMethod(PaymentMethods.QrTransfer, "Quét mã QR Ngân hàng / Ví điện tử (mô phỏng)", btnQr, new[] { btnCard });
        btnCard.Click += (_, _) => SelectPaymentMethod(PaymentMethods.Card, "Chạm hoặc quẹt thẻ (mô phỏng)", btnCard, new[] { btnQr });

        btnConfirmPay.Click += BtnConfirmPay_Click;

        _timer = new System.Windows.Forms.Timer { Interval = 1000 };
        _timer.Tick += (_, _) =>
        {
            if (lblClock != null && !lblClock.IsDisposed)
                lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
        };
        _timer.Start();

        FormClosed += (_, _) => _timer.Dispose();
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

        qrPlaceholder.Visible = code == PaymentMethods.QrTransfer;
        lblQrInstruction.Text = code == PaymentMethods.QrTransfer
            ? "Đây là mã QR mô phỏng (chế độ offline).\nBấm xác nhận bên dưới, hệ thống mặc định giao dịch thành công và xuất vé."
            : "Thanh toán thẻ được mô phỏng (chế độ offline).\nBấm xác nhận bên dưới, hệ thống mặc định giao dịch thành công và xuất vé.";
    }

    private void LoadOrderSummary()
    {
        pnlOrderDetails.Controls.Clear();

        ShowtimeListItemDto? showtime = null;
        try { showtime = _showtimeService.GetById(_order.ShowtimeId); }
        catch (Exception ex)
        {
            MessageBox.Show("Không tải được thông tin suất chiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        int yPos = 10;

        void AddLine(string label, string value, bool isBold = false, Color? valueColor = null, int fontSize = 11)
        {
            var lblLeft = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", fontSize, isBold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#8b9198"),
                Location = new Point(0, yPos),
                AutoSize = true
            };

            int rightX = fontSize >= 15 ? 310 : 240;
            var lblRight = new Label
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
            AddLine("Phim:", showtime.MovieTitle.ToUpper(), true, Color.White, 13);
            AddLine("Phòng chiếu:", showtime.RoomName);
            AddLine("Suất chiếu:", showtime.StartTime.ToString("HH:mm - dd/MM/yyyy"));
        }

        AddLine("Ghế đã chọn:", string.Join(", ", _order.SeatNumbers), true, ColorTranslator.FromHtml("#38bdf8"));
        AddLine($"Tiền vé ({_order.SeatNumbers.Count} ghế):", $"{_order.TicketTotal:N0} VNĐ", true);

        if (_order.Combos.Any())
        {
            yPos += 10;
            AddLine("Bắp & Nước:", "", true, ColorTranslator.FromHtml("#ffb961"));
            foreach (var combo in _order.Combos)
            {
                AddLine($"  • {combo.ComboName} (x{combo.Quantity})", $"{combo.UnitPrice * combo.Quantity:N0} VNĐ");
            }
        }

        pnlOrderDetails.Controls.Add(new Panel
        {
            Size = new Size(680, 2),
            Location = new Point(0, yPos + 10),
            BackColor = ColorTranslator.FromHtml("#262a2f")
        });
        yPos += 30;

        AddLine("TỔNG THANH TOÁN:", $"{_order.GrandTotal:N0} VNĐ", true, ColorTranslator.FromHtml("#ffb961"), 16);
    }

    private async void BtnConfirmPay_Click(object? sender, EventArgs e)
    {
        btnConfirmPay.Enabled = false;
        btnConfirmPay.Text = "ĐANG XỬ LÝ GIAO DỊCH...";

        try
        {
            BookingReceipt receipt = await _bookingService.CreateBookingAsync(new BookingRequest
            {
                ShowtimeId = _order.ShowtimeId,
                SeatIds = _order.SeatIds,
                SessionId = _order.SessionId,
                Combos = _order.Combos.Select(c => new ComboOrderLine { ComboId = c.ComboId, Quantity = c.Quantity }).ToList(),
                Channel = SalesChannels.Kiosk,
                PaymentMethod = _paymentMethod,
                ExpectedTotal = _order.GrandTotal
            });

            _timer.Stop();
            var frmSuccess = new FrmKioskSuccess(receipt) { TopMost = true };
            frmSuccess.Show();
            KioskFlow.CompleteOrder(frmSuccess);
        }
        catch (BusinessException ex)
        {
            ResetConfirmButton();
            MessageBox.Show($"Không thể hoàn tất giao dịch: {ex.Message}", "Cảnh báo đặt vé", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            var seatForm = Application.OpenForms.OfType<FrmKioskSeat>().FirstOrDefault();
            if (seatForm != null)
            {
                Application.OpenForms.OfType<FrmKioskCombo>().ToList().ForEach(f => f.Close());
                seatForm.Show();
                seatForm.Activate();
                Close();
            }
        }
        catch (Exception ex)
        {
            ResetConfirmButton();
            MessageBox.Show($"Lỗi hệ thống khi thanh toán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ResetConfirmButton()
    {
        btnConfirmPay.Enabled = true;
        btnConfirmPay.Text = $"XÁC NHẬN ĐÃ THANH TOÁN ({_order.GrandTotal:N0} VNĐ)";
    }
}

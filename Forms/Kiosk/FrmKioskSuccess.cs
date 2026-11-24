using CinemaManagement.Helpers;
using CinemaManagement.Models;
using CinemaManagement.Services;

namespace CinemaManagement.Forms.Kiosk;

public partial class FrmKioskSuccess : Form
    {
        private readonly BookingReceipt _receipt;

        private System.Windows.Forms.Timer _clockTimer = null!;
        private System.Windows.Forms.Timer _redirectTimer = null!;
        private int _secondsLeft = 30;

        public FrmKioskSuccess(BookingReceipt receipt)
        {
            _receipt = receipt;

            InitializeComponent();
            RenderTicketData();
            RegisterEvents();
            WindowLayout.ScaleToScreen(this);
        }

        private void RenderTicketData()
        {
            lblTicketCode.Text = $"MÃ ĐẶT VÉ: {_receipt.BookingCode}";

            string combos = _receipt.Combos.Any()
                ? "\nBắp nước: " + string.Join(", ", _receipt.Combos.Select(c => $"{c.ComboName} x{c.Quantity}"))
                : string.Empty;

            lblTicketDetails.Text =
                $"Phim: {_receipt.MovieTitle.ToUpper()}\n" +
                $"Suất chiếu: {_receipt.RoomName} | {_receipt.StartTime:HH:mm dd/MM/yyyy}\n" +
                $"Ghế: {_receipt.SeatList}{combos}\n" +
                $"Tổng thanh toán: {_receipt.TotalAmount:N0} VNĐ ({PaymentMethods.ToDisplay(_receipt.PaymentMethod)})\n" +
                "Trạng thái: Đã thanh toán thành công";

            try
            {
                picTicketQr.SizeMode = PictureBoxSizeMode.Zoom;
                picTicketQr.Image = QrHelper.Generate(_receipt.QrPayload, 8);
            }
            catch (Exception ex)
            {
                lblTicketDetails.Text += "\n(Không tạo được mã QR: " + ex.Message + ")";
            }
        }

        private void RegisterEvents()
        {
            btnFinish.Click += (s, e) => ReturnToHome();

            _clockTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _clockTimer.Tick += (s, e) =>
            {
                if (lblClock != null && !lblClock.IsDisposed)
                    lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
            };
            _clockTimer.Start();

            _redirectTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _redirectTimer.Tick += (s, e) =>
            {
                _secondsLeft--;
                if (_secondsLeft > 0)
                {
                    lblAutoRedirect.Text = $"Tự động quay về màn hình chính sau {_secondsLeft} giây...";
                }
                else
                {
                    ReturnToHome();
                }
            };
            _redirectTimer.Start();

            this.FormClosed += (_, _) =>
            {
                _clockTimer?.Dispose();
                _redirectTimer?.Dispose();
            };
        }

        private void ReturnToHome()
        {
            _clockTimer?.Stop();
            _redirectTimer?.Stop();

            KioskFlow.ReturnHome(this);
        }
    }
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.Forms.Kiosk;

public partial class FrmKioskSuccess : Form
    {
        private readonly int _bookingId;
        private readonly string _movieTitle;
        private readonly string _showtimeInfo;
        private readonly List<string> _seats;
        private readonly decimal _totalAmount;
        private readonly string _paymentMethod;

        private System.Windows.Forms.Timer _clockTimer;
        private System.Windows.Forms.Timer _redirectTimer;
        private int _secondsLeft = 15;

        public FrmKioskSuccess(
            int bookingId,
            string movieTitle,
            string showtimeInfo,
            List<string> seats,
            decimal totalAmount,
            string paymentMethod)
        {
            _bookingId = bookingId;
            _movieTitle = movieTitle;
            _showtimeInfo = showtimeInfo;
            _seats = seats ?? new List<string>();
            _totalAmount = totalAmount;
            _paymentMethod = paymentMethod;

            InitializeComponent();
            RenderTicketData();
            RegisterEvents();
        }

        private void RenderTicketData()
        {
            string ticketCode = $"CLX-{DateTime.Now:yyyyMMdd}-{_bookingId:D4}";
            lblTicketCode.Text = $"MÃ VÉ ĐIỆN TỬ: {ticketCode}";

            string statusText = _paymentMethod == "COUNTER_CASH"
                ? "Chờ thanh toán tại quầy (Vui lòng thanh toán trong 15 phút)"
                : "Đã thanh toán thành công";

            lblTicketDetails.Text =
                $"Phim: {_movieTitle.ToUpper()}\n" +
                $"Suất chiếu: {_showtimeInfo}\n" +
                $"Ghế đã đặt: {string.Join(", ", _seats)}\n" +
                $"Tổng thanh toán: {_totalAmount:N0} VNĐ\n" +
                $"Trạng thái: {statusText}";

            // Sinh mã QR giả lập độc nhất theo mã vé
            picTicketQr.Image = GenerateMockQrBitmap(ticketCode, 230);
        }

        private Bitmap GenerateMockQrBitmap(string seedText, int size)
        {
            Bitmap bmp = new Bitmap(size, size);
            using Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            int grid = 21;
            int cellSize = size / grid;
            int hash = seedText.GetHashCode();
            Random rng = new Random(hash);

            for (int r = 0; r < grid; r++)
            {
                for (int c = 0; c < grid; c++)
                {
                    // Vẽ 3 ô định vị ở 3 góc chuẩn QR Code
                    bool isTopLeft = r < 7 && c < 7;
                    bool isTopRight = r < 7 && c >= grid - 7;
                    bool isBottomLeft = r >= grid - 7 && c < 7;

                    if (isTopLeft || isTopRight || isBottomLeft)
                    {
                        int lr = isBottomLeft ? r - (grid - 7) : r;
                        int lc = isTopRight ? c - (grid - 7) : c;
                        bool isBorder = lr == 0 || lr == 6 || lc == 0 || lc == 6;
                        bool isCenter = lr >= 2 && lr <= 4 && lc >= 2 && lc <= 4;
                        if (isBorder || isCenter)
                            g.FillRectangle(Brushes.Black, c * cellSize, r * cellSize, cellSize, cellSize);
                    }
                    else if (rng.Next(0, 2) == 1)
                    {
                        g.FillRectangle(Brushes.Black, c * cellSize, r * cellSize, cellSize, cellSize);
                    }
                }
            }
            return bmp;
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

            var home = new FrmKioskHome();
            home.TopMost = true;
            home.Show();
            this.Close();
        }
    }
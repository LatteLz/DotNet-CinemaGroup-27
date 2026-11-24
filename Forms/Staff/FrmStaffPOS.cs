using CinemaManagement.Controls;
using CinemaManagement.Forms.Shared;
using CinemaManagement.Helpers;
using CinemaManagement.Models;
using CinemaManagement.Services;

namespace CinemaManagement.Forms.Staff
{
    public partial class FrmStaffPOS : Form
    {
        private readonly Color ColorBg = ColorTranslator.FromHtml("#0b0f12");
        private readonly Color ColorSurface = ColorTranslator.FromHtml("#101418");
        private readonly Color ColorCardBg = ColorTranslator.FromHtml("#181c20");
        private readonly Color ColorCardActive = ColorTranslator.FromHtml("#0284c7");

        private readonly MovieService _movieService = new();
        private readonly ShowtimeService _showtimeService = new();
        private readonly ComboService _comboService = new();
        private readonly BookingService _bookingService = new();

        private decimal currentTotal = 0;
        private decimal currentTicketRevenue = 0;
        private decimal currentComboRevenue = 0;

        private int selectedMovieId;
        private int selectedShowtimeId;

        private List<Movie> dbMovies = new();
        private List<ShowtimeListItemDto> dbShowtimes = new();
        private List<ComboItem> dbCombos = new();

        private readonly Dictionary<int, int> selectedCombos = new();

        private FlowLayoutPanel pnlDynamicMovies = null!;
        private FlowLayoutPanel pnlDynamicShowtimes = null!;
        private SeatMapControl seatMapControl = null!;
        private readonly ErrorProvider errorProvider = new();
        private readonly ToolTip _showtimeTip = new();

        public FrmStaffPOS()
        {
            InitializeComponent();
            AppSession.RequireLogin(this);

            SetupCustomUI();
            BindPaymentMethodEvents();
            SetupEvents();
            InitializeSeatMapControl();
            ReloadCatalog();
            ApplyPaymentModeUI();
            UpdateTotalDisplay();

            FormClosed += (_, _) =>
            {
                try { new SeatHoldService().ReleaseSession(seatMapControl.SessionId); } catch { }
            };
            WindowLayout.ApplyStandardSize(this);
            Resize += (_, _) => ArrangeLayout();
            ArrangeLayout();
        }

        private float DpiScale => DeviceDpi / 96f;

        private int S(int value) => (int)Math.Round(value * DpiScale);

        private readonly Font _badgeFont = new("Segoe UI", 7.5F, FontStyle.Bold);
        private readonly Font _movieTitleFont = new("Segoe UI", 8.5F, FontStyle.Bold);
        private readonly Font _movieSubFont = new("Segoe UI", 7F);
        private readonly Font _showtimeFont = new("Segoe UI", 8F, FontStyle.Bold);

        private static int TextHeight(Font font) => TextRenderer.MeasureText("ÁgỹĐ", font).Height;
        private int BadgeHeight => TextHeight(_badgeFont) + S(2);
        private int MovieTitleTop => S(5) + BadgeHeight + S(3);
        private int MovieSubTop => MovieTitleTop + TextHeight(_movieTitleFont) + S(2);
        private int MovieCardHeight => MovieSubTop + TextHeight(_movieSubFont) + S(8);
        private int ShowtimeButtonHeight => TextHeight(_showtimeFont) + S(16);
        private const int ItemMargin = 3;

        private static void VerticalScrollOnly(FlowLayoutPanel panel)
        {
            panel.AutoScroll = false;
            panel.HorizontalScroll.Maximum = 0;
            panel.HorizontalScroll.Visible = false;
            panel.HorizontalScroll.Enabled = false;
            panel.VerticalScroll.Visible = true;
            panel.AutoScroll = true;
        }

        private static void ClearAndDispose(Control parent)
        {
            var old = parent.Controls.Cast<Control>().ToList();
            parent.Controls.Clear();
            if (parent.IsHandleCreated)
                parent.BeginInvoke(new Action(() => old.ForEach(c => c.Dispose())));
            else
                old.ForEach(c => c.Dispose());
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ArrangeLayout();
        }

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            ArrangeLayout();
        }

        private void ArrangeLayout()
        {
            if (seatMapControl == null || pnlDynamicMovies == null || pnlDynamicShowtimes == null) return;
            int w = ClientSize.Width, h = ClientSize.Height;
            if (w <= 0 || h <= 0) return;

            SuspendLayout();
            int gap = S(10);
            int left = pnlSidebar.Right + gap;
            int usable = w - left - gap * 2;
            int leftW = (int)(usable * 0.54);
            int right = left + leftW + gap;
            int rightW = w - right - gap;

            int y = S(8);
            int moviesH = 2 * (MovieCardHeight + ItemMargin * 2) + S(4);
            pnlDynamicMovies.SetBounds(left, y, leftW, moviesH);
            y += moviesH + S(6);

            int showsH = 2 * (ShowtimeButtonHeight + ItemMargin * 2) + S(4);
            pnlDynamicShowtimes.SetBounds(left, y, leftW, showsH);
            y += showsH + S(6);

            int comboH = Math.Max(S(130), (int)(h * 0.2));
            int comboTop = h - S(8) - comboH;
            seatMapControl.SetBounds(left, y, leftW, Math.Max(S(220), comboTop - S(8) - y));
            if (pnlComboCatalog != null) pnlComboCatalog.SetBounds(left, comboTop, leftW, comboH);

            int bottom = h - S(8);
            btnCancelOrder.SetBounds(right, bottom - btnCancelOrder.Height, rightW, btnCancelOrder.Height);
            bottom = btnCancelOrder.Top - S(8);
            btnCheckout.SetBounds(right, bottom - btnCheckout.Height, rightW, btnCheckout.Height);
            bottom = btnCheckout.Top - S(8);
            pnlCashBox.SetBounds(right, bottom - pnlCashBox.Height, rightW, pnlCashBox.Height);
            bottom = pnlCashBox.Top - S(6);
            pnlQrBox.SetBounds(right, bottom - pnlQrBox.Height, rightW, pnlQrBox.Height);
            bottom = pnlQrBox.Top - S(6);
            lblTotalAmount.Location = new Point(right, bottom - lblTotalAmount.Height);
            lblTotalLabel.Location = new Point(right, bottom - lblTotalLabel.Height);
            bottom = lblTotalAmount.Top - S(6);

            int dgvH = Math.Min(S(200), Math.Max(S(120), (bottom - S(8)) / 2));
            dgvOrderItems.SetBounds(right, S(8), rightW, dgvH);
            int slotTop = dgvOrderItems.Bottom + S(8);
            pnlRightQrSlot.SetBounds(right, slotTop, rightW, Math.Max(0, bottom - slotTop));

            txtCashGiven.Width = pnlCashBox.ClientSize.Width - txtCashGiven.Left * 2;
            picQrCode.Left = pnlQrBox.ClientSize.Width - picQrCode.Width - S(8);
            lblQrHint.Width = Math.Max(0, picQrCode.Left - lblQrHint.Left - S(6));
            lblRightQrTitle.Width = pnlRightQrSlot.ClientSize.Width;
            lblRightQrCaption.Width = pnlRightQrSlot.ClientSize.Width;
            int qrSize = Math.Max(S(80), Math.Min(S(210), pnlRightQrSlot.ClientSize.Height - lblRightQrTitle.Bottom - lblRightQrCaption.Height - S(16)));
            picRightQr.SetBounds((pnlRightQrSlot.ClientSize.Width - qrSize) / 2, lblRightQrTitle.Bottom + S(6), qrSize, qrSize);
            lblRightQrCaption.Top = picRightQr.Bottom + S(6);
            ResumeLayout(true);

            BuildDynamicMovieCatalog();
            BuildDynamicShowtimeCatalog();
        }

        private static int ThreeColumnWidth(Control panel)
            => Math.Max(60, (panel.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 4) / 3 - ItemMargin * 2);

        private void SetupCustomUI()
        {
            this.BackColor = ColorBg;
            this.ForeColor = Color.White;
            this.SetStyle(
                ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer,
                true);

            dgvOrderItems.BackgroundColor = ColorSurface;
            dgvOrderItems.DefaultCellStyle.BackColor = ColorSurface;
            dgvOrderItems.DefaultCellStyle.ForeColor = Color.White;
            dgvOrderItems.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#1e242a");
            dgvOrderItems.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvOrderItems.RowTemplate.Height = 38;
            dgvOrderItems.AllowUserToAddRows = false;
            dgvOrderItems.ReadOnly = true;
        }

        private void BindPaymentMethodEvents()
        {
            EventHandler onMethodChanged = (_, _) =>
            {
                ApplyPaymentModeUI();
                UpdateTotalDisplay();
            };

            if (rdoCashMethod != null)
            {
                rdoCashMethod.CheckedChanged -= onMethodChanged;
                rdoCashMethod.CheckedChanged += onMethodChanged;
            }

            if (rdoQrMethod != null)
            {
                rdoQrMethod.CheckedChanged -= onMethodChanged;
                rdoQrMethod.CheckedChanged += onMethodChanged;
            }
        }

        private Image? CreatePaymentQrImage()
        {
            if (currentTotal <= 0) return null;
            try
            {
                string reference = $"POS{selectedShowtimeId}-{DateTime.Now:HHmmss}";
                return QrHelper.Generate(QrHelper.BuildMockPaymentPayload(reference, currentTotal), 6);
            }
            catch
            {
                return null;
            }
        }

        private void ApplyPaymentModeUI()
        {
            bool isQr = rdoQrMethod != null && rdoQrMethod.Checked;

            if (pnlCashBox != null)
            {
                pnlCashBox.Visible = !isQr;
            }
            else if (txtCashGiven != null && lblChangeDue != null)
            {
                txtCashGiven.Visible = !isQr;
                lblChangeDue.Visible = !isQr;
            }

            if (picQrCode != null)
            {
                picQrCode.Visible = isQr;
            }

            if (pnlRightQrSlot != null)
            {
                pnlRightQrSlot.Visible = isQr;
                if (isQr)
                {
                    pnlRightQrSlot.BringToFront();
                }
            }

            if (lblQrHint != null)
            {
                lblQrHint.Text = isQr
                    ? $"Quét mã QR phía trên • {currentTotal:N0} đ"
                    : "Thanh toán tiền mặt tại quầy";
            }

            if (isQr && txtCashGiven != null)
            {
                errorProvider.SetError(txtCashGiven, string.Empty);
            }

            RefreshQrCodeImages(isQr);
        }

        private void RefreshQrCodeImages(bool isQr)
        {
            if (!isQr) return;

            if (lblRightQrCaption != null)
                lblRightQrCaption.Text = $"Số tiền thanh toán: {currentTotal:N0} VNĐ (QR mô phỏng)";

            var qrImg = CreatePaymentQrImage();
            if (picQrCode != null)
            {
                var oldSmall = picQrCode.Image;
                picQrCode.Image = qrImg != null ? new Bitmap(qrImg) : null;
                oldSmall?.Dispose();
            }

            if (picRightQr != null)
            {
                var oldRight = picRightQr.Image;
                picRightQr.Image = qrImg;
                oldRight?.Dispose();
            }
            else
            {
                qrImg?.Dispose();
            }
        }

        private void InitializeSeatMapControl()
        {
            seatMapControl = new SeatMapControl();
            Controls.Add(seatMapControl);
            seatMapControl.BringToFront();
            seatMapControl.OnSeatSelectionChanged += _ => UpdateOrderItems();
        }

        private void ReloadCatalog()
        {
            try
            {
                dbShowtimes = _showtimeService.GetSellableShowtimes();
                var movieIds = dbShowtimes.Select(s => s.MovieId).ToHashSet();
                dbMovies = _movieService.GetAll().Where(m => movieIds.Contains(m.MovieId)).ToList();
                dbCombos = _comboService.GetActiveCombos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được dữ liệu bán vé: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dbShowtimes = new();
                dbMovies = new();
                dbCombos = new();
            }

            foreach (var id in selectedCombos.Keys.Where(id => dbCombos.All(c => c.ComboId != id)).ToList())
                selectedCombos.Remove(id);

            if (dbMovies.All(m => m.MovieId != selectedMovieId))
                selectedMovieId = dbMovies.FirstOrDefault()?.MovieId ?? 0;

            SelectMovie(selectedMovieId, keepShowtime: true);
            BuildDynamicComboCatalog();
        }

        private void SelectMovie(int movieId, bool keepShowtime = false)
        {
            selectedMovieId = movieId;
            var shows = dbShowtimes.Where(s => s.MovieId == movieId).ToList();

            int showtimeId = keepShowtime && shows.Any(s => s.ShowtimeId == selectedShowtimeId)
                ? selectedShowtimeId
                : shows.FirstOrDefault()?.ShowtimeId ?? 0;

            BuildDynamicMovieCatalog();
            SelectShowtime(showtimeId);
        }

        private void SelectShowtime(int showtimeId)
        {
            bool changed = showtimeId != selectedShowtimeId;
            if (changed && selectedShowtimeId > 0)
            {
                try { new SeatHoldService().ReleaseSession(seatMapControl.SessionId); } catch { }
            }

            selectedShowtimeId = showtimeId;
            BuildDynamicShowtimeCatalog();

            if (showtimeId > 0)
            {
                if (changed || seatMapControl.ShowtimeId != showtimeId)
                    seatMapControl.LoadSeatMap(showtimeId, SeatHoldService.NewSessionId());
                else
                    seatMapControl.RefreshStates();
                seatMapControl.Visible = true;
            }
            else
            {
                seatMapControl.Visible = false;
            }

            UpdateOrderItems();
        }

        private void BuildDynamicMovieCatalog()
        {
            if (pnlDynamicMovies == null)
            {
                pnlDynamicMovies = new FlowLayoutPanel();
                pnlDynamicMovies.BackColor = ColorBg;
                pnlDynamicMovies.FlowDirection = FlowDirection.LeftToRight;
                pnlDynamicMovies.WrapContents = true;
                VerticalScrollOnly(pnlDynamicMovies);
                this.Controls.Add(pnlDynamicMovies);
                pnlDynamicMovies.BringToFront();
            }

            pnlDynamicMovies.SuspendLayout();
            ClearAndDispose(pnlDynamicMovies);
            int cardW = ThreeColumnWidth(pnlDynamicMovies);

            foreach (var movie in dbMovies)
            {
                Panel pnlCard = new Panel();
                pnlCard.Size = new Size(cardW, MovieCardHeight);
                pnlCard.Margin = new Padding(ItemMargin);
                pnlCard.Cursor = Cursors.Hand;
                pnlCard.BackColor = (movie.MovieId == selectedMovieId) ? ColorCardActive : ColorCardBg;

                Label lblBadge = new Label();
                lblBadge.Text = string.IsNullOrWhiteSpace(movie.AgeRating) ? "P" : movie.AgeRating;
                lblBadge.Font = _badgeFont;
                lblBadge.BackColor = Color.FromArgb(225, 29, 72);
                lblBadge.ForeColor = Color.White;
                lblBadge.Location = new Point(S(6), S(5));
                lblBadge.Size = new Size(S(34), BadgeHeight);
                lblBadge.TextAlign = ContentAlignment.MiddleCenter;
                pnlCard.Controls.Add(lblBadge);

                Label lblTitle = new Label();
                lblTitle.Text = movie.Title;
                lblTitle.UseMnemonic = false;
                lblTitle.AutoEllipsis = true;
                lblTitle.Font = _movieTitleFont;
                lblTitle.ForeColor = Color.White;
                lblTitle.Location = new Point(S(6), MovieTitleTop);
                lblTitle.Size = new Size(cardW - S(12), TextHeight(_movieTitleFont));
                pnlCard.Controls.Add(lblTitle);

                Label lblSub = new Label();
                lblSub.Text = $"{movie.Duration} phút • {movie.Genre}";
                lblSub.UseMnemonic = false;
                lblSub.AutoEllipsis = true;
                lblSub.Font = _movieSubFont;
                lblSub.ForeColor = ColorTranslator.FromHtml("#94a3b8");
                lblSub.Location = new Point(S(6), MovieSubTop);
                lblSub.Size = new Size(cardW - S(12), TextHeight(_movieSubFont));
                pnlCard.Controls.Add(lblSub);

                var targetMovie = movie;
                EventHandler clickHandler = (s, e) =>
                {
                    SelectMovie(targetMovie.MovieId);
                };

                pnlCard.Click += clickHandler;
                lblBadge.Click += clickHandler;
                lblTitle.Click += clickHandler;
                lblSub.Click += clickHandler;

                pnlDynamicMovies.Controls.Add(pnlCard);
            }
            pnlDynamicMovies.ResumeLayout(true);
        }

        private void BuildDynamicShowtimeCatalog()
        {
            if (pnlDynamicShowtimes == null)
            {
                pnlDynamicShowtimes = new FlowLayoutPanel
                {
                    BackColor = ColorBg,
                    FlowDirection = FlowDirection.LeftToRight,
                    WrapContents = true
                };
                VerticalScrollOnly(pnlDynamicShowtimes);
                Controls.Add(pnlDynamicShowtimes);
                pnlDynamicShowtimes.BringToFront();
            }

            ClearAndDispose(pnlDynamicShowtimes);

            var movieShows = dbShowtimes.Where(s => s.MovieId == selectedMovieId).ToList();
            if (movieShows.Count == 0)
            {
                pnlDynamicShowtimes.Controls.Add(new Label
                {
                    Text = dbMovies.Count == 0 ? "Chưa có suất chiếu nào để bán." : "Phim này không còn suất chiếu.",
                    ForeColor = ColorTranslator.FromHtml("#94a3b8"),
                    AutoSize = true,
                    Margin = new Padding(4, 10, 4, 4)
                });
                return;
            }

            foreach (var show in movieShows)
            {
                bool isSelected = show.ShowtimeId == selectedShowtimeId;
                var btnShow = new Button
                {
                    Text = $"{show.StartTime:dd/MM HH:mm} • {show.RoomName}",
                    UseMnemonic = false,
                    AutoEllipsis = true,
                    Font = _showtimeFont,
                    Size = new Size(ThreeColumnWidth(pnlDynamicShowtimes), ShowtimeButtonHeight),
                    Margin = new Padding(ItemMargin),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = isSelected ? ColorCardActive : ColorCardBg,
                    ForeColor = Color.White
                };
                btnShow.FlatAppearance.BorderSize = 1;
                btnShow.FlatAppearance.BorderColor = isSelected ? Color.White : ColorTranslator.FromHtml("#1e242a");

                var targetShow = show;
                btnShow.Click += (_, _) => SelectShowtime(targetShow.ShowtimeId);
                _showtimeTip.SetToolTip(btnShow, $"{show.MovieTitle}\n{show.StartTime:HH:mm dd/MM/yyyy} - {show.RoomName}\nCòn {show.AvailableSeats}/{show.TotalSeats} ghế");

                pnlDynamicShowtimes.Controls.Add(btnShow);
            }
        }

        private void BuildDynamicComboCatalog()
        {
            if (pnlComboCatalog == null) return;
            pnlComboCatalog.Controls.Clear();
            pnlComboCatalog.AutoScroll = true;
            pnlComboCatalog.BackColor = ColorSurface;

            var pnlCombosFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true
            };
            pnlComboCatalog.Controls.Add(pnlCombosFlow);

            pnlCombosFlow.Controls.Add(new Label
            {
                Text = "Bấm chuột trái để thêm, chuột phải để bớt combo. Có thể bán riêng bắp nước (không cần chọn ghế).",
                ForeColor = ColorTranslator.FromHtml("#94a3b8"),
                Font = new Font("Segoe UI", 8F),
                AutoSize = true,
                Margin = new Padding(6, 6, 6, 2)
            });
            pnlCombosFlow.SetFlowBreak(pnlCombosFlow.Controls[0], true);

            foreach (var combo in dbCombos)
            {
                int qty = selectedCombos.TryGetValue(combo.ComboId, out var q) ? q : 0;

                var pnlCBox = new Panel
                {
                    Size = new Size(S(160), S(65)),
                    Margin = new Padding(4),
                    Cursor = Cursors.Hand,
                    BackColor = qty > 0 ? ColorCardActive : ColorCardBg
                };

                var lblCName = new Label
                {
                    Text = qty > 0 ? $"{combo.ComboName}  x{qty}" : combo.ComboName,
                    UseMnemonic = false,
                    AutoEllipsis = true,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(S(6), S(6)),
                    Size = new Size(S(148), S(18))
                };
                var lblCPrice = new Label
                {
                    Text = $"{combo.Price:N0} đ",
                    Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                    ForeColor = ColorTranslator.FromHtml("#f59e0b"),
                    Location = new Point(S(6), S(26)),
                    Size = new Size(S(148), S(15))
                };
                var lblCDesc = new Label
                {
                    Text = combo.Description ?? string.Empty,
                    Font = new Font("Segoe UI", 6.5F),
                    ForeColor = ColorTranslator.FromHtml("#94a3b8"),
                    Location = new Point(S(6), S(43)),
                    Size = new Size(S(148), S(15))
                };
                pnlCBox.Controls.AddRange(new Control[] { lblCName, lblCPrice, lblCDesc });

                int comboId = combo.ComboId;
                MouseEventHandler comboClick = (_, e) =>
                {
                    int current = selectedCombos.TryGetValue(comboId, out var c) ? c : 0;
                    int next = e.Button == MouseButtons.Right ? current - 1 : current + 1;
                    next = Math.Clamp(next, 0, 20);
                    if (next == 0) selectedCombos.Remove(comboId);
                    else selectedCombos[comboId] = next;
                    BuildDynamicComboCatalog();
                    UpdateOrderItems();
                };

                pnlCBox.MouseClick += comboClick;
                lblCName.MouseClick += comboClick;
                lblCPrice.MouseClick += comboClick;
                lblCDesc.MouseClick += comboClick;

                pnlCombosFlow.Controls.Add(pnlCBox);
            }
        }

        private (decimal TicketTotal, decimal ComboTotal) CalculateRevenueSplit()
        {
            decimal ticketsTotal = seatMapControl?.SelectedTotal ?? 0m;
            decimal combosTotal = selectedCombos.Sum(kv =>
                (dbCombos.FirstOrDefault(c => c.ComboId == kv.Key)?.Price ?? 0m) * kv.Value);
            return (ticketsTotal, combosTotal);
        }

        private void UpdateOrderItems()
        {
            dgvOrderItems.Rows.Clear();
            var split = CalculateRevenueSplit();

            var seats = seatMapControl?.SelectedSeats ?? new List<SeatStatusDto>();
            var show = dbShowtimes.FirstOrDefault(s => s.ShowtimeId == selectedShowtimeId);

            if (seats.Count > 0 && show != null)
            {
                string seatsStr = string.Join(", ", seats.Select(s => s.SeatNumber));
                dgvOrderItems.Rows.Add(
                    $"{show.MovieTitle}\nGhế: {seatsStr} ({show.StartTime:HH:mm dd/MM} - {show.RoomName})",
                    seats.Count.ToString("D2"),
                    $"{split.TicketTotal:N0} đ");
            }

            foreach (var kv in selectedCombos)
            {
                var combo = dbCombos.FirstOrDefault(c => c.ComboId == kv.Key);
                if (combo == null) continue;
                dgvOrderItems.Rows.Add($"{combo.ComboName}\n{combo.Description}", kv.Value.ToString("D2"), $"{combo.Price * kv.Value:N0} đ");
            }

            dgvOrderItems.ClearSelection();
            currentTicketRevenue = split.TicketTotal;
            currentComboRevenue = split.ComboTotal;
            currentTotal = split.TicketTotal + split.ComboTotal;
            UpdateTotalDisplay();
        }

        private void SetupEvents()
        {
            StaffNavigator.ApplyRoleVisibility(lnkCombo, lnkUsers, lnkReports, lnkOccupancy);

            lnkPos.LinkClicked += (_, _) =>
            {
                ReloadCatalog();
                MessageBox.Show($"Bạn đang ở màn hình Bán vé Quầy POS. (Thu ngân: {AppSession.DisplayName})", "Thông Báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            lnkCombo.LinkClicked += (_, _) => StaffNavigator.Navigate<FrmAdminCombos>(this);
            lnkUsers.LinkClicked += (_, _) => StaffNavigator.Navigate<FrmAdminUsers>(this);
            lnkReports.LinkClicked += (_, _) => StaffNavigator.Navigate<FrmAdminReports>(this);
            if (lnkOccupancy != null)
                lnkOccupancy.LinkClicked += (_, _) => StaffNavigator.Navigate<FrmOccupancyReport>(this);

            VisibleChanged += (_, _) => { if (Visible) ReloadCatalog(); };

            btnCheckout.Click += (_, _) => Checkout();

            btnCancelOrder.Click += (_, _) =>
            {
                errorProvider.Clear();
                selectedCombos.Clear();
                seatMapControl.ReleaseAll();
                txtCashGiven.Clear();
                BuildDynamicComboCatalog();
                UpdateOrderItems();
            };

            txtCashGiven.TextChanged += (_, _) =>
            {
                errorProvider.SetError(txtCashGiven, string.Empty);
                UpdateTotalDisplay();
            };
        }

        private async void Checkout()
        {
            errorProvider.Clear();

            var seatIds = seatMapControl.SelectedSeatIds;
            bool hasCombos = selectedCombos.Values.Any(q => q > 0);

            if (seatIds.Count == 0 && !hasCombos)
            {
                errorProvider.SetError(btnCheckout, "Đơn hàng trống: chọn ghế hoặc combo bắp nước!");
                MessageBox.Show("Đơn hàng đang trống. Vui lòng chọn ghế ngồi hoặc ít nhất một combo bắp nước.",
                    "Lỗi Thanh Toán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (seatIds.Count > 0 && selectedShowtimeId <= 0)
            {
                errorProvider.SetError(btnCheckout, "Chưa chọn suất chiếu!");
                MessageBox.Show("Vui lòng chọn suất chiếu.", "Lỗi Thanh Toán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool comboOnly = seatIds.Count == 0;
            if (comboOnly &&
                MessageBox.Show($"Đơn này không có vé, chỉ bán bắp nước ({currentTotal:N0} đ).\nTiếp tục thanh toán?",
                    "Bán lẻ bắp nước", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            bool isQr = rdoQrMethod != null && rdoQrMethod.Checked;
            decimal cashGiven = 0;
            if (!isQr)
            {
                if (!ValidationHelper.TryParseMoney(txtCashGiven.Text, out cashGiven) || cashGiven <= 0)
                {
                    errorProvider.SetError(txtCashGiven, "Vui lòng nhập số tiền khách đưa (số dương)!");
                    txtCashGiven.Focus();
                    return;
                }
                if (cashGiven < currentTotal)
                {
                    errorProvider.SetError(txtCashGiven, $"Khách đưa thiếu {currentTotal - cashGiven:N0} đ!");
                    MessageBox.Show("Tiền khách đưa chưa đủ để thanh toán!", "Lỗi Thanh Toán", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCashGiven.Focus();
                    return;
                }
            }

            btnCheckout.Enabled = false;
            try
            {
                BookingReceipt receipt = await _bookingService.CreateBookingAsync(new BookingRequest
                {
                    ShowtimeId = comboOnly ? 0 : selectedShowtimeId,
                    SeatIds = seatIds,
                    SessionId = seatMapControl.SessionId,
                    Combos = selectedCombos.Select(kv => new ComboOrderLine { ComboId = kv.Key, Quantity = kv.Value }).ToList(),
                    Channel = SalesChannels.Pos,
                    PaymentMethod = isQr ? PaymentMethods.QrTransfer : PaymentMethods.Cash,
                    CashGiven = cashGiven,
                    UserId = AppSession.CurrentUser?.Id,
                    ExpectedTotal = currentTotal
                });

                using (var receiptForm = new FrmReceipt(receipt))
                {
                    receiptForm.ShowDialog(this);
                }

                selectedCombos.Clear();
                txtCashGiven.Clear();
                seatMapControl.StartNewSession();
                ReloadCatalog();
            }
            catch (BusinessException ex)
            {
                MessageBox.Show(ex.Message, "Không thể thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ReloadCatalog();
                seatMapControl.RefreshStates();
                UpdateOrderItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi nhận giao dịch vào CSDL: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCheckout.Enabled = true;
            }
        }

        private void UpdateTotalDisplay()
        {
            lblTotalAmount.Text = $"{currentTotal:N0} đ";

            bool isQr = rdoQrMethod != null && rdoQrMethod.Checked;
            if (isQr)
            {
                if (lblQrHint != null) lblQrHint.Text = $"Quét mã QR (mô phỏng) • {currentTotal:N0} đ";
                RefreshQrCodeImages(true);
                return;
            }

            if (!ValidationHelper.TryParseMoney(txtCashGiven.Text, out decimal cashGiven))
            {
                lblChangeDue.Text = "TIỀN THỐI LẠI: Nhập tiền khách đưa";
                lblChangeDue.ForeColor = ColorTranslator.FromHtml("#94a3b8");
                return;
            }

            decimal change = cashGiven - currentTotal;
            if (change >= 0)
            {
                lblChangeDue.Text = $"TIỀN THỐI LẠI (CHANGE DUE): {change:N0} đ";
                lblChangeDue.ForeColor = ColorTranslator.FromHtml("#38bdf8");
            }
            else
            {
                lblChangeDue.Text = $"TIỀN THỐI LẠI: Thiếu {-change:N0} đ";
                lblChangeDue.ForeColor = Color.FromArgb(239, 68, 68);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using CinemaManagement.Controls;
using CinemaManagement.Data;
using CinemaManagement.Models;

namespace CinemaManagement.Forms.Staff
{
    public partial class FrmStaffPOS : Form
    {
        private readonly Color ColorBg = ColorTranslator.FromHtml("#0b0f12");
        private readonly Color ColorSurface = ColorTranslator.FromHtml("#101418");
        private readonly Color ColorCardBg = ColorTranslator.FromHtml("#181c20");
        private readonly Color ColorCardActive = ColorTranslator.FromHtml("#0284c7");

        private int loggedInUserId;
        private string loggedInUserName;
        private decimal currentTotal = 0;
        private string selectedMovie = "";
        private string selectedShowtime = "";
        private decimal selectedTicketPrice = 110000;
        private int selectedShowtimeId = 101;
        private string connectionString = "Data Source=cinema.db;Version=3;";

        private List<MovieDetail> dbMovies = new List<MovieDetail>();
        private List<ShowtimeDetail> dbShowtimes = new List<ShowtimeDetail>();
        private Dictionary<string, SeatDetail> dbSeats = new Dictionary<string, SeatDetail>();
        private List<ComboDetail> dbCombos = new List<ComboDetail>();

        private List<string> selectedSeats = new List<string>();
        private List<ComboDetail> selectedCombos = new List<ComboDetail>();

        private FlowLayoutPanel pnlDynamicMovies;
        private FlowLayoutPanel pnlDynamicShowtimes;
        private SeatMapControl seatMapControl;

        private class MovieDetail
        {
            public int MovieId { get; set; }
            public string Title { get; set; }
            public string Duration { get; set; }
            public string Genre { get; set; }
            public string AgeRating { get; set; }
        }

        private class ShowtimeDetail
        {
            public int ShowtimeId { get; set; }
            public int MovieId { get; set; }
            public int RoomId { get; set; }
            public string StartTime { get; set; }
            public string RoomName { get; set; }
            public decimal Price { get; set; }
        }

        private class SeatDetail
        {
            public string SeatNumber { get; set; }
            public string SeatType { get; set; }
            public decimal Price { get; set; }
        }

        private class ComboDetail
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }
        }

        public FrmStaffPOS() : this(1, "Julian Vance")
        {
        }

        public FrmStaffPOS(int userId, string userName)
        {
            InitializeComponent();
            loggedInUserId = userId;
            loggedInUserName = userName;

            SetupCustomUI();
            SetupEvents();
            LoadFullMoviesFromDatabase();
            LoadShowtimesFromDatabase();
            LoadSeatsFromDatabase();
            LoadCombosFromDatabase();

            BuildDynamicMovieCatalog();
            BuildDynamicShowtimeCatalog();
            InitializeSeatMapControl();
            BuildDynamicComboCatalog();
            UpdateOrderItems();
            UpdateTotalDisplay();
        }

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

            if (pnlComboCatalog != null) pnlComboCatalog.Location = new Point(230, 500);
        }

        private void InitializeSeatMapControl()
        {
            if (seatMapControl == null)
            {
                seatMapControl = new SeatMapControl();
                seatMapControl.Location = new Point(230, 200);
                seatMapControl.Size = new Size(515, 290);
                this.Controls.Add(seatMapControl);
                seatMapControl.BringToFront();

                seatMapControl.OnSeatSelectionChanged += (seats) =>
                {
                    selectedSeats = new List<string>(seats);
                    UpdateOrderItems();
                };
            }

            seatMapControl.LoadSeatMap(selectedShowtimeId);
        }

        private void LoadFullMoviesFromDatabase()
        {
            dbMovies.Clear();
            try
            {
                string query = "SELECT MovieId, Title, Duration, Genre, AgeRating FROM Movies;";
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dbMovies.Add(new MovieDetail
                                {
                                    MovieId = Convert.ToInt32(reader["MovieId"]),
                                    Title = reader["Title"].ToString(),
                                    Duration = reader["Duration"]?.ToString() ?? "120",
                                    Genre = reader["Genre"]?.ToString() ?? "HD",
                                    AgeRating = reader["AgeRating"]?.ToString() ?? "T13"
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL Phim: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            if (dbMovies.Count == 0)
            {
                dbMovies.Add(new MovieDetail
                    { MovieId = 1, Title = "Dune: Part Two", Duration = "166", Genre = "Sci-Fi", AgeRating = "T16" });
                dbMovies.Add(new MovieDetail
                    { MovieId = 2, Title = "Godzilla x Kong", Duration = "115", Genre = "Action", AgeRating = "T13" });
            }

            if (dbMovies.Count > 0)
            {
                selectedMovie = dbMovies[0].Title;
            }
        }

        private void LoadShowtimesFromDatabase()
        {
            dbShowtimes.Clear();
            try
            {
                string query = @"SELECT s.ShowtimeId, s.MovieId, s.RoomId, s.StartTime, r.RoomName, s.Price 
                                 FROM Showtimes s 
                                 LEFT JOIN Rooms r ON s.RoomId = r.RoomId;";
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dbShowtimes.Add(new ShowtimeDetail
                                {
                                    ShowtimeId = Convert.ToInt32(reader["ShowtimeId"]),
                                    MovieId = Convert.ToInt32(reader["MovieId"]),
                                    RoomId = Convert.ToInt32(reader["RoomId"]),
                                    StartTime = reader["StartTime"].ToString(),
                                    RoomName = reader["RoomName"]?.ToString() ?? $"Phòng {reader["RoomId"]}",
                                    Price = Convert.ToDecimal(reader["Price"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL Suất Chiếu: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            if (dbShowtimes.Count == 0)
            {
                dbShowtimes.Add(new ShowtimeDetail { ShowtimeId = 101, MovieId = 1, RoomId = 1, StartTime = "2026-11-12 19:30:00", RoomName = "IMAX Laser", Price = 160000 });
                dbShowtimes.Add(new ShowtimeDetail { ShowtimeId = 102, MovieId = 1, RoomId = 2, StartTime = "2026-11-12 21:15:00", RoomName = "Phòng 02", Price = 120000 });
            }

            UpdateActiveShowtimeForSelectedMovie();
        }

        private void UpdateActiveShowtimeForSelectedMovie()
        {
            var currentMovieObj = dbMovies.Find(m => m.Title == selectedMovie);
            if (currentMovieObj != null)
            {
                DateTime now = DateTime.Now;
                var matchShow = dbShowtimes.Find(s => s.MovieId == currentMovieObj.MovieId && 
                                                      (DateTime.TryParse(s.StartTime, out var dt) ? dt >= now : true));

                if (matchShow != null)
                {
                    selectedShowtimeId = matchShow.ShowtimeId;
                    selectedShowtime = $"{matchShow.StartTime} • P.{matchShow.RoomId:D2} [{matchShow.RoomName}]";
                    selectedTicketPrice = matchShow.Price;
                    if (seatMapControl != null)
                    {
                        seatMapControl.LoadSeatMap(selectedShowtimeId);
                    }
                }
                else
                {
                    selectedShowtime = "Chưa có suất chiếu khả dụng";
                    selectedTicketPrice = 0;
                }
            }
        }

        private void LoadSeatsFromDatabase()
        {
            dbSeats.Clear();
            try
            {
                string query = "SELECT SeatNumber, SeatType, Price FROM Seats;";
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string seatNum = reader["SeatNumber"]?.ToString() ?? "";
                                decimal.TryParse(
                                    reader["Price"]?.ToString(),
                                    NumberStyles.Any,
                                    CultureInfo.InvariantCulture,
                                    out decimal seatPrice);

                                dbSeats[seatNum] = new SeatDetail
                                {
                                    SeatNumber = seatNum,
                                    SeatType = reader["SeatType"]?.ToString() ?? "Standard",
                                    Price = seatPrice
                                };
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void LoadCombosFromDatabase()
        {
            dbCombos.Clear();
            try
            {
                string query = "SELECT ComboName as Name, Price, ImagePath as Description FROM ComboItems;";
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dbCombos.Add(new ComboDetail
                                {
                                    Name = reader["Name"].ToString(),
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    Description = reader["Description"]?.ToString() ?? "Combo F&B"
                                });
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            if (dbCombos.Count == 0)
            {
                dbCombos.Add(new ComboDetail { Name = "Combo Solo", Price = 79000, Description = "1 Bắp + 1 Nước" });
                dbCombos.Add(new ComboDetail { Name = "Combo Couple", Price = 129000, Description = "1 Bắp + 2 Nước" });
                dbCombos.Add(new ComboDetail { Name = "Combo Family", Price = 179000, Description = "2 Bắp + 3 Nước" });
            }
        }

        private void BuildDynamicMovieCatalog()
        {
            if (pnlDynamicMovies == null)
            {
                pnlDynamicMovies = new FlowLayoutPanel();
                pnlDynamicMovies.Location = new Point(230, 10);
                pnlDynamicMovies.Size = new Size(515, 140);
                pnlDynamicMovies.BackColor = ColorBg;
                pnlDynamicMovies.FlowDirection = FlowDirection.LeftToRight;
                pnlDynamicMovies.WrapContents = true;
                this.Controls.Add(pnlDynamicMovies);
                pnlDynamicMovies.BringToFront();
            }

            pnlDynamicMovies.Controls.Clear();

            foreach (var movie in dbMovies)
            {
                Panel pnlCard = new Panel();
                pnlCard.Size = new Size(165, 62);
                pnlCard.Margin = new Padding(2);
                pnlCard.Cursor = Cursors.Hand;
                pnlCard.BackColor = (movie.Title == selectedMovie) ? ColorCardActive : ColorCardBg;

                Label lblBadge = new Label();
                lblBadge.Text = movie.AgeRating;
                lblBadge.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
                lblBadge.BackColor = Color.FromArgb(225, 29, 72);
                lblBadge.ForeColor = Color.White;
                lblBadge.Location = new Point(5, 4);
                lblBadge.Size = new Size(30, 15);
                lblBadge.TextAlign = ContentAlignment.MiddleCenter;
                pnlCard.Controls.Add(lblBadge);

                Label lblTitle = new Label();
                lblTitle.Text = movie.Title;
                lblTitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                lblTitle.ForeColor = Color.White;
                lblTitle.Location = new Point(5, 20);
                lblTitle.Size = new Size(155, 18);
                pnlCard.Controls.Add(lblTitle);

                Label lblSub = new Label();
                lblSub.Text = $"{movie.Duration} phút • {movie.Genre}";
                lblSub.Font = new Font("Segoe UI", 6.5F);
                lblSub.ForeColor = ColorTranslator.FromHtml("#94a3b8");
                lblSub.Location = new Point(5, 39);
                lblSub.Size = new Size(155, 15);
                pnlCard.Controls.Add(lblSub);

                var targetMovie = movie;
                EventHandler clickHandler = (s, e) =>
                {
                    selectedMovie = targetMovie.Title;
                    UpdateActiveShowtimeForSelectedMovie();
                    BuildDynamicMovieCatalog();
                    BuildDynamicShowtimeCatalog();
                    UpdateOrderItems();
                };

                pnlCard.Click += clickHandler;
                lblBadge.Click += clickHandler;
                lblTitle.Click += clickHandler;
                lblSub.Click += clickHandler;

                pnlDynamicMovies.Controls.Add(pnlCard);
            }
        }

        private void BuildDynamicShowtimeCatalog()
        {
            if (pnlDynamicShowtimes == null)
            {
                pnlDynamicShowtimes = new FlowLayoutPanel();
                pnlDynamicShowtimes.Location = new Point(230, 155);
                pnlDynamicShowtimes.Size = new Size(515, 40);
                pnlDynamicShowtimes.BackColor = ColorBg;
                pnlDynamicShowtimes.FlowDirection = FlowDirection.LeftToRight;
                pnlDynamicShowtimes.WrapContents = true;
                this.Controls.Add(pnlDynamicShowtimes);
                pnlDynamicShowtimes.BringToFront();
            }

            pnlDynamicShowtimes.Controls.Clear();
            var currentMovieObj = dbMovies.Find(m => m.Title == selectedMovie);
            if (currentMovieObj == null) return;

            DateTime now = DateTime.Now;

            var movieShows = dbShowtimes.FindAll(s => 
            {
                if (s.MovieId != currentMovieObj.MovieId) return false;
                if (DateTime.TryParse(s.StartTime, out DateTime showTimeParsed))
                {
                    return showTimeParsed >= now;
                }
                return true;
            });

            foreach (var show in movieShows)
            {
                Button btnShow = new Button();
                string showStr = $"{show.StartTime} • P.{show.RoomId:D2} [{show.RoomName}]";
                btnShow.Text = showStr;
                btnShow.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
                btnShow.Size = new Size(165, 32);
                btnShow.Margin = new Padding(2);
                btnShow.FlatStyle = FlatStyle.Flat;
                btnShow.FlatAppearance.BorderSize = 1;

                bool isSelected = selectedShowtime.Contains(show.StartTime);
                btnShow.BackColor = isSelected ? ColorCardActive : ColorCardBg;
                btnShow.ForeColor = Color.White;
                btnShow.FlatAppearance.BorderColor = isSelected ? Color.White : ColorTranslator.FromHtml("#1e242a");

                var targetShow = show;
                btnShow.Click += (s, e) =>
                {
                    selectedShowtimeId = targetShow.ShowtimeId;
                    selectedShowtime = $"{targetShow.StartTime} • P.{targetShow.RoomId:D2} [{targetShow.RoomName}]";
                    selectedTicketPrice = targetShow.Price;
                    if (seatMapControl != null)
                    {
                        seatMapControl.LoadSeatMap(selectedShowtimeId);
                    }
                    BuildDynamicShowtimeCatalog();
                    UpdateOrderItems();
                };

                pnlDynamicShowtimes.Controls.Add(btnShow);
            }
        }

        private void BuildDynamicComboCatalog()
        {
            if (pnlComboCatalog == null) return;
            pnlComboCatalog.Controls.Clear();
            pnlComboCatalog.AutoScroll = true;
            pnlComboCatalog.BackColor = ColorSurface;

            FlowLayoutPanel pnlCombosFlow = new FlowLayoutPanel();
            pnlCombosFlow.Dock = DockStyle.Fill;
            pnlCombosFlow.FlowDirection = FlowDirection.LeftToRight;
            pnlCombosFlow.WrapContents = true;
            pnlComboCatalog.Controls.Add(pnlCombosFlow);

            foreach (var combo in dbCombos)
            {
                Panel pnlCBox = new Panel();
                pnlCBox.Size = new Size(160, 65);
                pnlCBox.Margin = new Padding(4);
                pnlCBox.Cursor = Cursors.Hand;

                bool isSelected = selectedCombos.Contains(combo);
                pnlCBox.BackColor = isSelected ? ColorCardActive : ColorCardBg;

                Label lblCName = new Label();
                lblCName.Text = combo.Name;
                lblCName.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                lblCName.ForeColor = Color.White;
                lblCName.Location = new Point(6, 6);
                lblCName.Size = new Size(148, 18);
                pnlCBox.Controls.Add(lblCName);

                Label lblCPrice = new Label();
                lblCPrice.Text = $"{combo.Price:N0} đ";
                lblCPrice.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
                lblCPrice.ForeColor = ColorTranslator.FromHtml("#f59e0b");
                lblCPrice.Location = new Point(6, 26);
                lblCPrice.Size = new Size(148, 15);
                pnlCBox.Controls.Add(lblCPrice);

                Label lblCDesc = new Label();
                lblCDesc.Text = combo.Description;
                lblCDesc.Font = new Font("Segoe UI", 6.5F);
                lblCDesc.ForeColor = ColorTranslator.FromHtml("#94a3b8");
                lblCDesc.Location = new Point(6, 43);
                lblCDesc.Size = new Size(148, 15);
                pnlCBox.Controls.Add(lblCDesc);

                var targetCombo = combo;
                EventHandler comboClick = (s, e) =>
                {
                    if (selectedCombos.Contains(targetCombo)) selectedCombos.Remove(targetCombo);
                    else selectedCombos.Add(targetCombo);
                    BuildDynamicComboCatalog();
                    UpdateOrderItems();
                };

                pnlCBox.Click += comboClick;
                lblCName.Click += comboClick;
                lblCPrice.Click += comboClick;
                lblCDesc.Click += comboClick;

                pnlCombosFlow.Controls.Add(pnlCBox);
            }
        }

        private void UpdateOrderItems()
        {
            dgvOrderItems.Rows.Clear();
            decimal ticketsTotal = 0;
            decimal combosTotal = 0;

            if (selectedSeats.Count > 0)
            {
                string seatsStr = string.Join(", ", selectedSeats);
                foreach (var seat in selectedSeats)
                {
                    if (dbSeats.ContainsKey(seat)) ticketsTotal += dbSeats[seat].Price;
                    else ticketsTotal += selectedTicketPrice;
                }

                dgvOrderItems.Rows.Add(new object[]
                {
                    $"{selectedMovie}\nGhế: {seatsStr} ({selectedShowtime})", selectedSeats.Count.ToString("D2"),
                    $"{ticketsTotal:N0} đ"
                });
            }

            foreach (var combo in selectedCombos)
            {
                combosTotal += combo.Price;
                dgvOrderItems.Rows.Add(new object[]
                    { $"{combo.Name}\n{combo.Description}", "01", $"{combo.Price:N0} đ" });
            }

            dgvOrderItems.ClearSelection();
            currentTotal = ticketsTotal + combosTotal;
            UpdateTotalDisplay();
        }

        private void SetupEvents()
        {
            lnkPos.LinkClicked += (s, e) =>
            {
                MessageBox.Show($"Bạn đang ở màn hình Bán vé Quầy POS. (Thu ngân: {loggedInUserName})", "Thông Báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            lnkCombo.LinkClicked += (s, e) =>
            {
                FrmAdminCombos adminComboForm = new FrmAdminCombos();
                adminComboForm.Show();
                this.Hide();
            };

            lnkUsers.LinkClicked += (s, e) =>
            {
                FrmAdminUsers usersForm = new FrmAdminUsers();
                usersForm.Show();
                this.Hide();
            };

            btnCheckout.Click += (s, e) =>
            {
                decimal cashGiven = ParseMoney(txtCashGiven.Text);
                if (cashGiven < currentTotal)
                {
                    MessageBox.Show("Tiền khách đưa chưa đủ để thanh toán!", "Lỗi Thanh Toán", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                SaveTransactionToDatabase(currentTotal, cashGiven);
                MessageBox.Show("Đã thanh toán và lưu giao dịch thành công vào cơ sở dữ liệu!", "Hoàn Tất",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                selectedSeats.Clear();
                selectedCombos.Clear();
                UpdateOrderItems();
                if (seatMapControl != null)
                {
                    seatMapControl.LoadSeatMap(selectedShowtimeId);
                }
                BuildDynamicComboCatalog();
            };

            btnCancelOrder.Click += (s, e) =>
            {
                selectedSeats.Clear();
                selectedCombos.Clear();
                UpdateOrderItems();
                if (seatMapControl != null)
                {
                    seatMapControl.LoadSeatMap(selectedShowtimeId);
                }
                BuildDynamicComboCatalog();
            };

            txtCashGiven.TextChanged += (s, e) => UpdateTotalDisplay();
        }

        private void SaveTransactionToDatabase(decimal totalAmount, decimal cashGiven)
        {
            decimal changeReturned = cashGiven - totalAmount;

            try
            {
                using (var db = new CinemaDbContext())
                {
                    var booking = new Booking
                    {
                        MovieTitle = selectedMovie,
                        ShowtimeInfo = selectedShowtime,
                        TotalAmount = totalAmount,
                        CreatedAt = DateTime.Now
                    };
                    db.Bookings.Add(booking);
                    db.SaveChanges();
                    var payment = new Payment
                    {
                        BookingId = booking.Id,
                        Amount = totalAmount,
                        CashGiven = cashGiven,
                        ChangeReturned = changeReturned < 0 ? 0 : changeReturned,
                        PaymentMethod = "CASH",
                        Status = "SUCCESS",
                        PaymentDate = DateTime.Now
                    };
                    db.Payments.Add(payment);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi nhận giao dịch vào CSDL: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void UpdateTotalDisplay()
        {
            lblTotalAmount.Text = $"{currentTotal:N0} đ";
            decimal cashGiven = ParseMoney(txtCashGiven.Text);
            decimal change = cashGiven - currentTotal;

            if (change >= 0)
            {
                lblChangeDue.Text = $"TIỀN THỐI LẠI (CHANGE DUE): {change:N0} đ";
                lblChangeDue.ForeColor = ColorTranslator.FromHtml("#38bdf8");
            }
            else
            {
                lblChangeDue.Text = "TIỀN THỐI LẠI: Không đủ";
                lblChangeDue.ForeColor = Color.FromArgb(239, 68, 68);
            }
        }

        private decimal ParseMoney(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            string clean = text.Replace("đ", "").Replace(".", "").Replace(",", "").Trim();
            decimal.TryParse(clean, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal val);
            return val;
        }
    }
}
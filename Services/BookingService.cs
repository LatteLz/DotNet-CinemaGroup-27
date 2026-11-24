using CinemaManagement.Data;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class ComboOrderLine
    {
        public int ComboId { get; set; }
        public int Quantity { get; set; }
    }

    public class BookingRequest
    {
        public int ShowtimeId { get; set; }
        public List<int> SeatIds { get; set; } = new();
        public List<ComboOrderLine> Combos { get; set; } = new();

        public string SessionId { get; set; } = string.Empty;

        public string Channel { get; set; } = SalesChannels.Kiosk;
        public string PaymentMethod { get; set; } = PaymentMethods.QrTransfer;

        public decimal CashGiven { get; set; }

        public int? UserId { get; set; }

        public decimal? ExpectedTotal { get; set; }
    }

    public class ReceiptTicketLine
    {
        public string SeatNumber { get; set; } = string.Empty;
        public string SeatType { get; set; } = string.Empty;
        public string TicketCode { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class ReceiptComboLine
    {
        public string ComboName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => UnitPrice * Quantity;
    }

    public class BookingReceipt
    {
        public int BookingId { get; set; }
        public string BookingCode { get; set; } = string.Empty;
        public string MovieTitle { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public DateTime? StartTime { get; set; }
        public string Channel { get; set; } = string.Empty;

        public bool IsComboOnly => Tickets.Count == 0;
        public List<ReceiptTicketLine> Tickets { get; set; } = new();
        public List<ReceiptComboLine> Combos { get; set; } = new();
        public decimal TicketAmount { get; set; }
        public decimal ComboAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal CashGiven { get; set; }
        public decimal ChangeReturned { get; set; }
        public DateTime CreatedAt { get; set; }

        public string SeatList => string.Join(", ", Tickets.Select(t => t.SeatNumber));
        public string QrPayload => Helpers.QrHelper.BuildTicketPayload(BookingCode, string.Join(",", Tickets.Select(t => t.TicketCode)));
    }

    public class BookingService
    {
        private readonly PaymentService _paymentService = new();

        public const string ComboOnlyTitle = "Bán lẻ bắp nước";

        public async Task<BookingReceipt> CreateBookingAsync(BookingRequest request)
        {
            request.SeatIds ??= new List<int>();
            request.Combos ??= new List<ComboOrderLine>();
            if (request.SeatIds.Count == 0)
                return await CreateComboOnlyOrderAsync(request);

            if (request.SeatIds.Distinct().Count() != request.SeatIds.Count)
                throw new BusinessException("Danh sách ghế bị trùng.");
            if (request.SeatIds.Count > SeatHoldService.MaxSeatsPerOrder)
                throw new BusinessException($"Mỗi đơn chỉ được tối đa {SeatHoldService.MaxSeatsPerOrder} ghế.");
            if (request.Combos.Any(c => c.Quantity < 0))
                throw new BusinessException("Số lượng combo không hợp lệ.");

            await using var db = DbFactory.Create();
            await using var tx = await db.Database.BeginTransactionAsync();
            DateTime now = DateTime.Now;

            var showtime = await db.Showtimes
                               .Include(s => s.Movie)
                               .Include(s => s.Room)
                               .FirstOrDefaultAsync(s => s.ShowtimeId == request.ShowtimeId)
                           ?? throw new BusinessException("Suất chiếu không tồn tại.");
            if (!ShowtimeService.IsSellable(showtime.StartTime))
                throw new BusinessException("Suất chiếu đã quá giờ bán vé.");

            var seats = await db.Seats.Where(s => request.SeatIds.Contains(s.SeatId)).ToListAsync();
            if (seats.Count != request.SeatIds.Count || seats.Any(s => s.RoomId != showtime.RoomId))
                throw new BusinessException("Có ghế không thuộc phòng chiếu của suất này. Vui lòng chọn lại.");

            var soldSeatNumbers = await db.Tickets
                .Where(t => t.ShowtimeId == showtime.ShowtimeId && request.SeatIds.Contains(t.SeatId))
                .Select(t => t.Seat.SeatNumber)
                .ToListAsync();
            if (soldSeatNumbers.Count > 0)
                throw new BusinessException($"Ghế {string.Join(", ", soldSeatNumbers)} vừa được bán cho khách khác. Vui lòng chọn ghế khác.");

            var heldByOthers = await db.SeatHolds
                .Where(h => h.ShowtimeId == showtime.ShowtimeId
                            && request.SeatIds.Contains(h.SeatId)
                            && h.ExpiresAt > now
                            && h.SessionId != request.SessionId)
                .Select(h => h.Seat.SeatNumber)
                .ToListAsync();
            if (heldByOthers.Count > 0)
                throw new BusinessException($"Ghế {string.Join(", ", heldByOthers)} đang được khách khác giữ. Vui lòng chọn ghế khác.");

            var comboLines = request.Combos.Where(c => c.Quantity > 0).ToList();
            var comboIds = comboLines.Select(c => c.ComboId).Distinct().ToList();
            var comboItems = await db.ComboItems.Where(c => comboIds.Contains(c.ComboId)).ToListAsync();
            if (comboItems.Count != comboIds.Count || comboItems.Any(c => !c.IsActive))
                throw new BusinessException("Có combo không còn kinh doanh. Vui lòng chọn lại combo.");

            var orderedSeats = seats.OrderBy(s => s.RowIndex).ThenBy(s => s.ColIndex).ToList();
            decimal ticketAmount = orderedSeats.Sum(s => SeatTypes.GetTicketPrice(showtime.Price, s.SeatType));
            decimal comboAmount = comboLines.Sum(l => comboItems.First(c => c.ComboId == l.ComboId).Price * l.Quantity);
            decimal total = ticketAmount + comboAmount;

            if (request.ExpectedTotal.HasValue && request.ExpectedTotal.Value != total)
                throw new BusinessException(
                    $"Giá vé hoặc giá combo vừa được cập nhật. Tổng tiền mới là {total:N0} đ " +
                    $"(trước đó hiển thị {request.ExpectedTotal.Value:N0} đ). Vui lòng kiểm tra lại đơn hàng.");

            Payment payment = _paymentService.Process(total, request.PaymentMethod, request.CashGiven);

            string bookingCode = GenerateBookingCode(now);
            var booking = new Booking
            {
                BookingCode = bookingCode,
                ShowtimeId = showtime.ShowtimeId,
                Channel = request.Channel,
                CreatedByUserId = request.UserId,
                MovieTitle = showtime.Movie.Title,
                ShowtimeInfo = $"{showtime.Room.RoomName} | {showtime.StartTime:HH:mm dd/MM/yyyy}",
                TicketAmount = ticketAmount,
                ComboAmount = comboAmount,
                TotalAmount = total,
                CreatedAt = now
            };

            foreach (var seat in orderedSeats)
            {
                booking.Tickets.Add(new Ticket
                {
                    ShowtimeId = showtime.ShowtimeId,
                    SeatId = seat.SeatId,
                    Price = SeatTypes.GetTicketPrice(showtime.Price, seat.SeatType),
                    TicketCode = $"{bookingCode}-{seat.SeatNumber}"
                });
            }

            foreach (var line in comboLines)
            {
                var item = comboItems.First(c => c.ComboId == line.ComboId);
                booking.BookingCombos.Add(new BookingCombo
                {
                    ComboId = item.ComboId,
                    Quantity = line.Quantity,
                    UnitPrice = item.Price
                });
            }

            booking.Payments.Add(payment);
            db.Bookings.Add(booking);

            if (!string.IsNullOrWhiteSpace(request.SessionId))
                db.SeatHolds.RemoveRange(db.SeatHolds.Where(h => h.SessionId == request.SessionId));

            try
            {
                await db.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch (DbUpdateException)
            {
                throw new BusinessException("Một số ghế vừa được bán ở quầy/kiosk khác. Giao dịch đã được hủy, vui lòng chọn lại ghế.");
            }

            return new BookingReceipt
            {
                BookingId = booking.Id,
                BookingCode = bookingCode,
                MovieTitle = showtime.Movie.Title,
                RoomName = showtime.Room.RoomName,
                StartTime = showtime.StartTime,
                Channel = request.Channel,
                Tickets = orderedSeats.Select(s => new ReceiptTicketLine
                {
                    SeatNumber = s.SeatNumber,
                    SeatType = s.SeatType,
                    Price = SeatTypes.GetTicketPrice(showtime.Price, s.SeatType),
                    TicketCode = $"{bookingCode}-{s.SeatNumber}"
                }).ToList(),
                Combos = comboLines.Select(l =>
                {
                    var item = comboItems.First(c => c.ComboId == l.ComboId);
                    return new ReceiptComboLine { ComboName = item.ComboName, Quantity = l.Quantity, UnitPrice = item.Price };
                }).ToList(),
                TicketAmount = ticketAmount,
                ComboAmount = comboAmount,
                TotalAmount = total,
                PaymentMethod = payment.PaymentMethod,
                CashGiven = payment.CashGiven,
                ChangeReturned = payment.ChangeReturned,
                CreatedAt = now
            };
        }

        private async Task<BookingReceipt> CreateComboOnlyOrderAsync(BookingRequest request)
        {
            if (request.Channel != SalesChannels.Pos)
                throw new BusinessException("Vui lòng chọn ít nhất một ghế.");
            var comboLines = request.Combos.Where(c => c.Quantity > 0).ToList();
            if (comboLines.Count == 0)
                throw new BusinessException("Đơn hàng trống: vui lòng chọn ghế hoặc ít nhất một combo bắp nước.");
            if (request.Combos.Any(c => c.Quantity < 0) || comboLines.GroupBy(c => c.ComboId).Any(g => g.Count() > 1))
                throw new BusinessException("Số lượng combo không hợp lệ.");

            await using var db = DbFactory.Create();
            await using var tx = await db.Database.BeginTransactionAsync();
            DateTime now = DateTime.Now;

            var comboIds = comboLines.Select(c => c.ComboId).ToList();
            var comboItems = await db.ComboItems.Where(c => comboIds.Contains(c.ComboId)).ToListAsync();
            if (comboItems.Count != comboIds.Count || comboItems.Any(c => !c.IsActive))
                throw new BusinessException("Có combo không còn kinh doanh. Vui lòng chọn lại combo.");

            decimal comboAmount = comboLines.Sum(l => comboItems.First(c => c.ComboId == l.ComboId).Price * l.Quantity);
            if (request.ExpectedTotal.HasValue && request.ExpectedTotal.Value != comboAmount)
                throw new BusinessException(
                    $"Giá combo vừa được cập nhật. Tổng tiền mới là {comboAmount:N0} đ " +
                    $"(trước đó hiển thị {request.ExpectedTotal.Value:N0} đ). Vui lòng kiểm tra lại đơn hàng.");

            Payment payment = _paymentService.Process(comboAmount, request.PaymentMethod, request.CashGiven);

            string bookingCode = GenerateBookingCode(now);
            var booking = new Booking
            {
                BookingCode = bookingCode,
                ShowtimeId = null,
                Channel = request.Channel,
                CreatedByUserId = request.UserId,
                MovieTitle = ComboOnlyTitle,
                ShowtimeInfo = string.Empty,
                TicketAmount = 0,
                ComboAmount = comboAmount,
                TotalAmount = comboAmount,
                CreatedAt = now
            };
            foreach (var line in comboLines)
            {
                var item = comboItems.First(c => c.ComboId == line.ComboId);
                booking.BookingCombos.Add(new BookingCombo { ComboId = item.ComboId, Quantity = line.Quantity, UnitPrice = item.Price });
            }
            booking.Payments.Add(payment);
            db.Bookings.Add(booking);
            await db.SaveChangesAsync();
            await tx.CommitAsync();

            return new BookingReceipt
            {
                BookingId = booking.Id,
                BookingCode = bookingCode,
                MovieTitle = ComboOnlyTitle,
                RoomName = string.Empty,
                StartTime = null,
                Channel = request.Channel,
                Combos = comboLines.Select(l =>
                {
                    var item = comboItems.First(c => c.ComboId == l.ComboId);
                    return new ReceiptComboLine { ComboName = item.ComboName, Quantity = l.Quantity, UnitPrice = item.Price };
                }).ToList(),
                TicketAmount = 0,
                ComboAmount = comboAmount,
                TotalAmount = comboAmount,
                PaymentMethod = payment.PaymentMethod,
                CashGiven = payment.CashGiven,
                ChangeReturned = payment.ChangeReturned,
                CreatedAt = now
            };
        }

        public BookingReceipt? GetReceipt(int bookingId)
        {
            using var db = DbFactory.Create();
            var b = db.Bookings.AsNoTracking()
                .Include(x => x.Showtime).ThenInclude(s => s.Room)
                .Include(x => x.Tickets).ThenInclude(t => t.Seat)
                .Include(x => x.BookingCombos).ThenInclude(c => c.ComboItem)
                .Include(x => x.Payments)
                .FirstOrDefault(x => x.Id == bookingId);
            if (b == null) return null;

            var payment = b.Payments.OrderByDescending(p => p.PaymentDate).FirstOrDefault();
            return new BookingReceipt
            {
                BookingId = b.Id,
                BookingCode = b.BookingCode,
                MovieTitle = b.MovieTitle,
                RoomName = b.Showtime?.Room.RoomName ?? string.Empty,
                StartTime = b.Showtime?.StartTime,
                Channel = b.Channel,
                Tickets = b.Tickets.OrderBy(t => t.Seat.RowIndex).ThenBy(t => t.Seat.ColIndex)
                    .Select(t => new ReceiptTicketLine { SeatNumber = t.Seat.SeatNumber, SeatType = t.Seat.SeatType, TicketCode = t.TicketCode, Price = t.Price })
                    .ToList(),
                Combos = b.BookingCombos
                    .Select(c => new ReceiptComboLine { ComboName = c.ComboItem.ComboName, Quantity = c.Quantity, UnitPrice = c.UnitPrice })
                    .ToList(),
                TicketAmount = b.TicketAmount,
                ComboAmount = b.ComboAmount,
                TotalAmount = b.TotalAmount,
                PaymentMethod = payment?.PaymentMethod ?? "",
                CashGiven = payment?.CashGiven ?? 0,
                ChangeReturned = payment?.ChangeReturned ?? 0,
                CreatedAt = b.CreatedAt
            };
        }

        private static string GenerateBookingCode(DateTime now)
            => $"CLX{now:yyMMddHHmmssfff}{Random.Shared.Next(10, 100)}";
    }
}

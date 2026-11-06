using System;
using System.Collections.Generic;
using System.Linq;
using CinemaManagement.Data;
using CinemaManagement.Models;

namespace CinemaManagement.Services;

public class BookingService
{
    // tạo giao dịch đặt vé gồm booking , ticket , payment trong 1 transaction và dùng chung cho cả ki ot và POS
   public bool CreateBooking(int showtimeId , List<int> seatIds,decimal ticketTotalAmount, decimal comboTotalAmount, out string errorMessage)
   {
      errorMessage = string.Empty;
      using var db = new CinemaDbContext();
      using var transaction = db.Database.BeginTransaction();
      try
      {
          // kiểm tra lại để chống trùng ghế trong transaction
var isAnySeatSold = db.Tickets.Any(t=>t.ShowtimeId== showtimeId && seatIds.Contains(t.SeatId));
if (isAnySeatSold)
{
    errorMessage = " Một số ghế bạn chọn vừa được người khác thanh toán thành công";
    transaction.Rollback();
    return false;
}
// tạo đơn booking theo Booking.cs
var booking = new Booking
{
    ShowtimeId = showtimeId,
    BookingTime =  DateTime.Now,
    TotalAmount = ticketTotalAmount + comboTotalAmount,
    Status =  "Completed"
};
db.Bookings.Add(booking);
db.SaveChanges();
// tạo danh sách ticket chuẩn theo model ticket.cs
foreach(var seatId in seatIds)
{
    db.Tickets.Add(new Ticket
    {
        BookingId = booking.BookingId,
        ShowtimeId = showtimeId,
        SeatId = seatId,
        Price = seatIds.Count >0 ? ticketTotalAmount / seatIds.Count : 0,
    });
    
}
//giả lập payment thành công chuẩn theo model payment.cs
db.Payments.Add(new Payment
{
    BookingId  = booking.BookingId,
    Amount =  ticketTotalAmount + comboTotalAmount,
    PaymentTime =   DateTime.Now,
    PaymentMethod = " ki-ốt",
    Status =   "Completed"
});
// xóa trạng thái giữ ghế ( seathold) tương ứng
var holdsToRemove = db.SeatHolds
    .Where(h=>h.ShowtimeId == showtimeId && seatIds.Contains(h.SeatId))
    .ToList();
if  (holdsToRemove.Any())
{
    db.RemoveRange(holdsToRemove);
}
db.SaveChanges();
transaction.Commit();
return true;
      }
      catch (Exception ex)
      {
          transaction.Rollback();
          errorMessage = "Lỗi xử lý giao dịch:"  + ex.Message;
          return false;
      }
      }
          
      }
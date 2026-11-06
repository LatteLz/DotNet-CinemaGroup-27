using System;
using System.Collections.Generic;
using System.Linq;
using CinemaManagement.Data;
using CinemaManagement.Models;


namespace CinemaManagement.Services
{
public class SeatHoldService
{
    public void ReleaseExpiredHolds()   
    {
using var db = new CinemaDbContext();
var expiredHolds = db.SeatHolds
    .Where(h => h.ExpiresAt <= DateTime.Now)
    .ToList();
    if (expiredHolds.Any())
    {
        db.SeatHolds.RemoveRange(expiredHolds);
        db.SaveChanges();
        }
    }
    public bool HoldSeats(int showtimeId , List<int> seatIds , string sessionId)
    {
    using var db = new CinemaDbContext();
    ReleaseExpiredHolds();
    var isAnySeatUnavailable = db.SeatHolds.Any(h => h.ShowtimeId == showtimeId && seatIds.Contains(h.SeatId));
                               db.Tickets.Any(t=>t.ShowtimeId==showtimeId && seatIds.Contains(t.SeatId));
    if (isAnySeatUnavailable)
    {
        return false;
    }
 var expiresAt = DateTime.Now.AddMinutes(3);
 foreach (var seatId in seatIds)
 {
     db.SeatHolds.Add(new SeatHold
         {
             ShowtimeId = showtimeId,
             SeatId = seatId,
            ExpiresAt = expiresAt,
            SessionId = string.IsNullOrEmpty(sessionId) ? Guid.NewGuid().ToString() : sessionId
         });
     }
 db.SaveChanges();
 return true;
 }
    public List<int> GetHeldSeatIds(int showtimeId)
 {
     using var db = new CinemaDbContext();
     ReleaseExpiredHolds();
     return db.SeatHolds
         .Where(h=> h.ShowtimeId == showtimeId)
         .Select(h=> h.SeatId)
         .ToList();
 }
    }
}
    
    
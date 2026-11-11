using System;
using System.Collections.Generic;
using System.Linq;
using CinemaManagement.Data;
using CinemaManagement.Models;

namespace CinemaManagement.Services;

public class SeatHoldService
{
    private readonly CinemaDbContext _db;

    public SeatHoldService()
    {
        _db = new CinemaDbContext();
    }
    public bool HoldSeats(int showtimeId, List<string> seatCodes, int holdDurationMinutes = 3)
    {
        try
        {
            ReleaseExpiredHolds();

            DateTime expireTime = DateTime.Now.AddMinutes(holdDurationMinutes);

            foreach (var seatCode in seatCodes)
            {
                var seatObj = _db.Seats.FirstOrDefault(s => s.SeatNumber == seatCode);
                int seatId = seatObj?.SeatId ?? 1;
                bool isHeld = _db.SeatHolds.Any(h => h.ShowtimeId == showtimeId && h.SeatId == seatId && h.ExpiresAt > DateTime.Now);
                if (isHeld) return false;

                var hold = new SeatHold
                {
                    ShowtimeId = showtimeId,
                    SeatId = seatId,
                    ExpiresAt = expireTime,
                   SessionId = Guid.NewGuid().ToString(),
                };
                _db.SeatHolds.Add(hold);
            }

            _db.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
    public void ReleaseExpiredHolds()
    {
        try
        {
            var expired = _db.SeatHolds.Where(h => h.ExpiresAt <= DateTime.Now).ToList();
            if (expired.Any())
            {
                _db.SeatHolds.RemoveRange(expired);
                _db.SaveChanges();
            }
        }
        catch { }
    }
}
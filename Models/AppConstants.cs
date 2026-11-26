namespace CinemaManagement.Models
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Staff = "Staff";
    }

    public static class PaymentStatus
    {
        public const string Success = "Success";
        public const string Failed = "Failed";
    }

    public static class PaymentMethods
    {
        public const string Cash = "Cash";
        public const string QrTransfer = "QR";
        public const string Card = "Card";

        public static string ToDisplay(string method) => method switch
        {
            Cash => "Tiền mặt",
            QrTransfer => "Chuyển khoản / QR",
            Card => "Thẻ ngân hàng",
            _ => method
        };
    }

    public static class SalesChannels
    {
        public const string Kiosk = "Kiosk";
        public const string Pos = "POS";
    }

    public static class SeatTypes
    {
        public const string Standard = "Standard";
        public const string Vip = "VIP";
        public const string Couple = "Couple";

        public const decimal VipSurcharge = 5500m;
        public const decimal CoupleSurchargePerPerson = 26000m;

        public static decimal GetTicketPrice(decimal showtimePrice, string seatType) => seatType switch
        {
            Vip => showtimePrice + VipSurcharge,
            Couple => (showtimePrice + CoupleSurchargePerPerson) * 2,
            _ => showtimePrice
        };

        public static string ToDisplay(string seatType) => seatType switch
        {
            Vip => "VIP",
            Couple => "Sweetbox",
            _ => "Thường"
        };
    }

    public static class RoomTypes
    {
        public const string Standard = "Standard 2D";
        public const string Imax = "IMAX Laser";
        public const string ScreenX = "ScreenX";
        public const string Vip = "VIP Suite";

        public static readonly string[] All = { Standard, Imax, ScreenX, Vip };

        public static decimal BasePrice(string? roomType, DateTime showDate)
        {
            DayType day = DayTypes.Of(showDate);
            return roomType switch
            {
                Imax => day == DayType.Weekend ? 183000m : 153000m,
                ScreenX => day == DayType.Weekend ? 173000m : 143000m,
                _ => day switch
                {
                    DayType.Wednesday => 75000m,
                    DayType.Weekend => 105000m,
                    _ => 79000m
                }
            };
        }
    }

    public static class AudienceTypes
    {
        public const string Adult = "Adult";
        public const string U22 = "U22";
        public const string Child = "Child";
        public const string Senior = "Senior";

        public static readonly string[] All = { Adult, U22, Child, Senior };

        public static string Normalize(string? audience) => All.Contains(audience) ? audience! : Adult;

        public static string ToDisplay(string? audience) => audience switch
        {
            U22 => "U22 (12–22 tuổi)",
            Child => "Trẻ em",
            Senior => "Người cao tuổi",
            _ => "Người lớn"
        };

        public static string ShortDisplay(string? audience) => audience switch
        {
            U22 => "U22",
            Child => "Trẻ em",
            Senior => "Cao tuổi",
            _ => "Người lớn"
        };

        public static string Requirement(string? audience) => audience switch
        {
            U22 => "Từ 12 đến 22 tuổi, xuất trình thẻ thành viên hoặc CCCD",
            Child => "Trên 2 tuổi và cao dưới 1m30",
            Senior => "Trên 55 tuổi, xuất trình giấy tờ tùy thân",
            _ => "Giá mặc định"
        };

        public static bool WaivesVipSurcharge(string? audience) => audience is U22 or Child;
    }

    public static class TicketPricing
    {
        public static bool IsPremiumFormat(string? roomType) => roomType is RoomTypes.Imax or RoomTypes.ScreenX;

        public static decimal AudienceBase(decimal showtimePrice, string? roomType, DateTime start, string? audience)
        {
            audience = AudienceTypes.Normalize(audience);
            if (IsPremiumFormat(roomType) || audience == AudienceTypes.Adult) return showtimePrice;

            decimal tablePrice = audience == AudienceTypes.U22 || DayTypes.Of(start) == DayType.Weekend ? 70000m : 58000m;
            return Math.Min(showtimePrice, tablePrice);
        }

        public static decimal TicketPrice(decimal showtimePrice, string? roomType, DateTime start, string seatType, string? audience)
        {
            if (IsPremiumFormat(roomType)) audience = AudienceTypes.Adult;
            decimal basePrice = AudienceBase(showtimePrice, roomType, start, audience);
            return seatType switch
            {
                SeatTypes.Vip => basePrice + (AudienceTypes.WaivesVipSurcharge(audience) ? 0 : SeatTypes.VipSurcharge),
                SeatTypes.Couple => (basePrice + SeatTypes.CoupleSurchargePerPerson) * 2,
                _ => basePrice
            };
        }
    }

    public enum DayType
    {
        Weekday,
        Wednesday,
        Weekend
    }

    public static class DayTypes
    {
        public static DayType Of(DateTime date) => date.DayOfWeek switch
        {
            DayOfWeek.Wednesday => DayType.Wednesday,
            DayOfWeek.Friday or DayOfWeek.Saturday or DayOfWeek.Sunday => DayType.Weekend,
            _ => DayType.Weekday
        };

        public static string ToDisplay(DayType type) => type switch
        {
            DayType.Wednesday => "Thứ Tư vui vẻ",
            DayType.Weekend => "Cuối tuần",
            _ => "Ngày thường"
        };
    }

    public enum SeatState
    {
        Available,
        HeldByOther,
        HeldByMe,
        Sold
    }
}

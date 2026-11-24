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

        public const decimal VipSurcharge = 20000m;

        public static decimal GetTicketPrice(decimal showtimePrice, string seatType) => seatType switch
        {
            Vip => showtimePrice + VipSurcharge,
            Couple => showtimePrice * 2,
            _ => showtimePrice
        };

        public static string ToDisplay(string seatType) => seatType switch
        {
            Vip => "VIP",
            Couple => "Đôi",
            _ => "Thường"
        };
    }

    public static class RoomTypes
    {
        public const string Standard = "Standard 2D";
        public const string Imax = "IMAX Laser";
        public const string Vip = "VIP Suite";
        public const string FourDx = "4DX";

        public static readonly string[] All = { Standard, Imax, Vip, FourDx };

        public static decimal SuggestedPrice(string? roomType) => roomType switch
        {
            Imax => 120000m,
            Vip => 150000m,
            FourDx => 130000m,
            _ => 90000m
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

using CinemaManagement.Services;

namespace CinemaManagement.Forms.Kiosk
{
    public class KioskComboSelection
    {
        public int ComboId { get; set; }
        public string ComboName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class KioskOrder
    {
        public int ShowtimeId { get; init; }
        public int MovieId { get; init; }

        public string SessionId { get; init; } = string.Empty;

        public List<int> SeatIds { get; set; } = new();
        public List<string> SeatNumbers { get; set; } = new();
        public decimal TicketTotal { get; set; }
        public List<KioskComboSelection> Combos { get; set; } = new();

        public decimal ComboTotal => Combos.Sum(c => c.UnitPrice * c.Quantity);
        public decimal GrandTotal => TicketTotal + ComboTotal;
    }

    public static class KioskFlow
    {
        public static void ReturnHome(Form? caller = null)
        {
            CloseOrderForms(caller);

            var home = Application.OpenForms.OfType<FrmKioskHome>().FirstOrDefault();
            if (home == null)
            {
                home = new FrmKioskHome();
                home.Show();
            }
            else
            {
                home.ReloadMovies();
                home.Show();
            }
            home.Activate();

            if (caller != null && !caller.IsDisposed && caller is not FrmKioskHome)
                caller.Close();
        }

        public static void CompleteOrder(Form successForm) => CloseOrderForms(successForm);

        public static void ExitKiosk(Form? caller = null)
        {
            CloseOrderForms(caller);
            foreach (var home in Application.OpenForms.OfType<FrmKioskHome>().ToList())
            {
                if (home != caller) home.Close();
            }
            if (caller != null && !caller.IsDisposed) caller.Close();
        }

        public static void HandleHoldExpired(string sessionId, Form caller)
        {
            try { new SeatHoldService().ReleaseSession(sessionId); } catch { }

            MessageBox.Show(
                "Đã hết 3 phút giữ ghế nên các ghế bạn chọn đã được nhả. Vui lòng chọn lại.",
                "Hết thời gian giữ ghế", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            ReturnHome(caller);
        }

        private static void CloseOrderForms(Form? except)
        {
            var forms = Application.OpenForms.Cast<Form>()
                .Where(f => f != except &&
                            (f is FrmKioskShowtime || f is FrmKioskSeat || f is FrmKioskCombo ||
                             f is FrmKioskPayment || f is FrmKioskSuccess))
                .OrderBy(f => f is FrmKioskShowtime ? 0 : 1)
                .ToList();

            foreach (var f in forms)
            {
                if (!f.IsDisposed) f.Close();
            }
        }
    }
}

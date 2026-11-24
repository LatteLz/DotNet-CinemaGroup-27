using QRCoder;

namespace CinemaManagement.Helpers
{
    public static class QrHelper
    {
        public static Bitmap Generate(string payload, int pixelsPerModule = 10)
        {
            using var generator = new QRCodeGenerator();
            using QRCodeData data = generator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            var png = new PngByteQRCode(data);
            byte[] bytes = png.GetGraphic(pixelsPerModule);

            using var ms = new MemoryStream(bytes);
            using var temp = Image.FromStream(ms);
            return new Bitmap(temp);
        }

        public static string BuildMockPaymentPayload(string reference, decimal amount)
            => $"CINELUXE-PAY|REF:{reference}|AMOUNT:{amount:0}|DEMO-OFFLINE";

        public static string BuildTicketPayload(string bookingCode, string ticketCodes)
            => $"CINELUXE-TICKET|{bookingCode}|{ticketCodes}";
    }
}

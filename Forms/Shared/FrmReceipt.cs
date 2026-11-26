using System.Text;
using CinemaManagement.Helpers;
using CinemaManagement.Models;
using CinemaManagement.Services;

namespace CinemaManagement.Forms.Shared
{
    public class FrmReceipt : Form
    {
        private readonly BookingReceipt _receipt;

        public FrmReceipt(BookingReceipt receipt)
        {
            _receipt = receipt;

            Text = $"Hóa đơn {receipt.BookingCode}";
            Size = new Size(560, 640);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = ColorTranslator.FromHtml("#181c20");
            ForeColor = Color.White;

            var pic = new PictureBox
            {
                Size = new Size(200, 200),
                Location = new Point(170, 16),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };
            try { pic.Image = QrHelper.Generate(receipt.QrPayload, 6); } catch { }

            var txt = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Location = new Point(20, 230),
                Size = new Size(505, 300),
                BackColor = ColorTranslator.FromHtml("#101418"),
                ForeColor = Color.White,
                Font = new Font("Consolas", 9.5F),
                Text = BuildText()
            };

            var btnSave = new Button
            {
                Text = "💾 Lưu hóa đơn (.txt)",
                Location = new Point(20, 545),
                Size = new Size(200, 38),
                BackColor = ColorTranslator.FromHtml("#0284c7"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (_, _) => SaveToFile();

            var btnClose = new Button
            {
                Text = "Đóng",
                DialogResult = DialogResult.OK,
                Location = new Point(405, 545),
                Size = new Size(120, 38),
                BackColor = ColorTranslator.FromHtml("#27303a"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;

            Controls.AddRange(new Control[] { pic, txt, btnSave, btnClose });
            AcceptButton = btnClose;
        }

        private string BuildText()
        {
            var sb = new StringBuilder();
            sb.AppendLine("            CINELUXE CINEMA");
            sb.AppendLine("        HÓA ĐƠN BÁN VÉ XEM PHIM");
            sb.AppendLine("----------------------------------------");
            sb.AppendLine($"Mã hóa đơn : {_receipt.BookingCode}");
            sb.AppendLine($"Thời gian  : {_receipt.CreatedAt:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"Kênh bán   : {_receipt.Channel}");
            if (AppSession.CurrentUser != null)
                sb.AppendLine($"Thu ngân   : {AppSession.CurrentUser.Fullname}");
            sb.AppendLine("----------------------------------------");
            if (_receipt.IsComboOnly)
            {
                sb.AppendLine("Đơn bán lẻ bắp nước (không kèm vé)");
            }
            else
            {
                sb.AppendLine($"Phim  : {_receipt.MovieTitle}");
                sb.AppendLine($"Suất  : {_receipt.StartTime:HH:mm dd/MM/yyyy} - {_receipt.RoomName}");
            }
            sb.AppendLine();
            foreach (var t in _receipt.Tickets)
                sb.AppendLine($"Vé {t.SeatNumber,-5} {SeatTypes.ToDisplay(t.SeatType),-8} {AudienceTypes.ShortDisplay(t.AudienceType),-9} {t.Price,10:N0} đ  [{t.TicketCode}]");
            foreach (var c in _receipt.Combos)
                sb.AppendLine($"{c.ComboName} x{c.Quantity,-3} {c.LineTotal,18:N0} đ");
            sb.AppendLine("----------------------------------------");
            sb.AppendLine($"Tiền vé    : {_receipt.TicketAmount,15:N0} đ");
            sb.AppendLine($"Bắp nước   : {_receipt.ComboAmount,15:N0} đ");
            sb.AppendLine($"TỔNG CỘNG  : {_receipt.TotalAmount,15:N0} đ");
            sb.AppendLine($"Thanh toán : {PaymentMethods.ToDisplay(_receipt.PaymentMethod)}");
            if (_receipt.PaymentMethod == PaymentMethods.Cash)
            {
                sb.AppendLine($"Khách đưa  : {_receipt.CashGiven,15:N0} đ");
                sb.AppendLine($"Tiền thừa  : {_receipt.ChangeReturned,15:N0} đ");
            }
            sb.AppendLine("----------------------------------------");
            sb.AppendLine("     Cảm ơn quý khách - Hẹn gặp lại!");
            return sb.ToString();
        }

        private void SaveToFile()
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt",
                FileName = $"HoaDon_{_receipt.BookingCode}.txt"
            };
            if (sfd.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                File.WriteAllText(sfd.FileName, BuildText(), Encoding.UTF8);
                MessageBox.Show("Đã lưu hóa đơn: " + sfd.FileName, "Xuất hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                MessageBox.Show("Không lưu được hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

using CinemaManagement.Helpers;
using CinemaManagement.Models;
using CinemaManagement.Services;

namespace CinemaManagement.Forms.Kiosk;

public partial class FrmKioskCombo : Form
{
    private readonly KioskOrder _order;
    private readonly ComboService _comboService = new();
    private readonly Dictionary<int, KioskComboSelection> _selections = new();
    private System.Windows.Forms.Timer _timer = null!;

    public FrmKioskCombo(KioskOrder order)
    {
        _order = order;
        InitializeComponent();
        InitializeComboCards();
        RegisterEvents();
        UpdateSummary();
        WindowLayout.ScaleToScreen(this);
    }

    private void InitializeComboCards()
    {
        var flow = new FlowLayoutPanel
        {
            Location = new Point(40, 170),
            Size = new Size(1840, 800),
            AutoScroll = true,
            BackColor = BackColor
        };
        Controls.Add(flow);

        List<ComboItem> combos;
        try
        {
            combos = _comboService.GetActiveCombos();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không tải được danh sách combo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            combos = new List<ComboItem>();
        }

        if (combos.Count == 0)
        {
            flow.Controls.Add(new Label
            {
                Text = "Hiện chưa có combo bắp nước. Bạn có thể bấm Tiếp tục để thanh toán vé.",
                ForeColor = ColorTranslator.FromHtml("#94A3B8"),
                Font = new Font("Segoe UI", 12F),
                AutoSize = true
            });
        }

        foreach (var c in _order.Combos) _selections[c.ComboId] = c;

        foreach (var combo in combos)
        {
            flow.Controls.Add(CreateComboCard(combo));
        }
    }

    private void RegisterEvents()
    {
        btnBack.Click += (_, _) =>
        {
            _timer.Stop();
            var seatForm = Application.OpenForms.OfType<FrmKioskSeat>().FirstOrDefault();
            if (seatForm != null)
            {
                seatForm.Show();
                seatForm.Activate();
            }
            Close();
        };

        btnExit.Click += (_, _) =>
        {
            _timer.Stop();
            KioskFlow.ExitKiosk(this);
        };

        btnContinuePayment.Click += (_, _) =>
        {
            _order.Combos = _selections.Values.Where(s => s.Quantity > 0).ToList();
            var frmPayment = new FrmKioskPayment(_order) { TopMost = true };
            frmPayment.Show();
            Hide();
        };

        _timer = new System.Windows.Forms.Timer { Interval = 1000 };
        _timer.Tick += (_, _) =>
        {
            if (lblClock != null && !lblClock.IsDisposed)
                lblClock.Text = DateTime.Now.ToString("HH:mm:ss");
        };
        _timer.Start();

        FormClosed += (_, _) => _timer.Dispose();
    }

    private Panel CreateComboCard(ComboItem combo)
    {
        var pnl = new Panel
        {
            Size = new Size(420, 360),
            Margin = new Padding(0, 0, 30, 30),
            BackColor = ColorTranslator.FromHtml("#1c2024")
        };

        var pic = new PictureBox
        {
            Size = new Size(380, 150),
            Location = new Point(20, 16),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = ColorTranslator.FromHtml("#262a2f"),
            Image = ImageHelper.Load(combo.ImagePath)
        };

        var lblName = new Label { Text = combo.ComboName.ToUpper(), Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(20, 176), AutoSize = true };
        var lblDesc = new Label { Text = combo.Description ?? string.Empty, Font = new Font("Segoe UI", 10F), ForeColor = ColorTranslator.FromHtml("#94A3B8"), Location = new Point(20, 208), Size = new Size(380, 44) };
        var lblPrice = new Label { Text = $"{combo.Price:N0} đ", Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#ffb961"), Location = new Point(20, 256), AutoSize = true };

        int currentQty = _selections.TryGetValue(combo.ComboId, out var existing) ? existing.Quantity : 0;
        var lblQty = new Label { Text = currentQty.ToString(), Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(200, 308), AutoSize = true };
        var btnMinus = new Button { Text = "-", Size = new Size(40, 40), Location = new Point(140, 300), BackColor = ColorTranslator.FromHtml("#334155"), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
        var btnPlus = new Button { Text = "+", Size = new Size(40, 40), Location = new Point(240, 300), BackColor = ColorTranslator.FromHtml("#38bdf8"), ForeColor = ColorTranslator.FromHtml("#101418"), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };

        void SetQty(int qty)
        {
            currentQty = Math.Clamp(qty, 0, 20);
            lblQty.Text = currentQty.ToString();
            _selections[combo.ComboId] = new KioskComboSelection
            {
                ComboId = combo.ComboId,
                ComboName = combo.ComboName,
                Quantity = currentQty,
                UnitPrice = combo.Price
            };
            UpdateSummary();
        }

        btnMinus.Click += (_, _) => SetQty(currentQty - 1);
        btnPlus.Click += (_, _) => SetQty(currentQty + 1);

        pnl.Controls.AddRange(new Control[] { pic, lblName, lblDesc, lblPrice, btnMinus, lblQty, btnPlus });
        return pnl;
    }

    private void UpdateSummary()
    {
        int totalCombo = _selections.Values.Sum(s => s.Quantity);
        decimal comboTotal = _selections.Values.Sum(s => s.Quantity * s.UnitPrice);
        lblComboSummary.Text = $"Ghế: {string.Join(", ", _order.SeatNumbers)} | Bắp nước đã chọn: {totalCombo} phần";
        lblTotalPrice.Text = $"TỔNG TIỀN: {_order.TicketTotal + comboTotal:N0} đ";
    }
}

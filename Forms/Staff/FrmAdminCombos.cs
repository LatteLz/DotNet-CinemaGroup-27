using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CinemaManagement.Forms.Staff
{
    public partial class FrmAdminCombos : Form
    {
        private readonly Color ColorBg = ColorTranslator.FromHtml("#101418");
        private readonly Color ColorSurfaceContainer = ColorTranslator.FromHtml("#181C20");
        private readonly Color ColorBorder = ColorTranslator.FromHtml("#27303A");
        private readonly Color ColorPrimaryBlue = ColorTranslator.FromHtml("#0284C7");
        private readonly Color ColorTextWhite = ColorTranslator.FromHtml("#FFFFFF");
        private readonly Color ColorTextMuted = ColorTranslator.FromHtml("#94A3B8");

        private readonly Font FontInterBold = new Font("Segoe UI", 9F, FontStyle.Bold);
        private readonly Font FontInterMedium = new Font("Segoe UI", 9F, FontStyle.Regular);
        private readonly Font FontInterSmall = new Font("Segoe UI", 8F, FontStyle.Regular);
        private readonly Font FontMono = new Font("Consolas", 8.5F, FontStyle.Regular);

        public FrmAdminCombos()
        {
            InitializeComponent();
            SetupCustomUI();
            SetupButtonEvents();
            LoadMockData();
        }

        private void SetupCustomUI()
        {
            this.BackColor = ColorBg;
            this.ForeColor = ColorTextWhite;
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            dgvCombos.BackgroundColor = ColorBg;
            dgvCombos.GridColor = ColorBorder;
            dgvCombos.DefaultCellStyle.BackColor = ColorBg;
            dgvCombos.DefaultCellStyle.ForeColor = ColorTextWhite;
            dgvCombos.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#1E293B");
            dgvCombos.DefaultCellStyle.SelectionForeColor = ColorPrimaryBlue;
            dgvCombos.DefaultCellStyle.Font = FontInterMedium;
            dgvCombos.ColumnHeadersDefaultCellStyle.BackColor = ColorSurfaceContainer;
            dgvCombos.ColumnHeadersDefaultCellStyle.ForeColor = ColorTextMuted;
            dgvCombos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            dgvCombos.RowTemplate.Height = 52;

            dgvCombos.CellPainting += DgvCombos_CellPainting;
        }

        private void SetupButtonEvents()
        {
            btnAddComboTop.Click += BtnAddComboTop_Click;
            btnUpdatePrice.Click += BtnUpdatePrice_Click;
            btnImportStore.Click += BtnImportStore_Click;
            btnExportExcel.Click += BtnExportExcel_Click;
        }

        private void BtnAddComboTop_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mở form thêm mới mặt hàng F&B / Combo bắp nước.", "Thêm Combo Mới", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnUpdatePrice_Click(object sender, EventArgs e)
        {
            if (dgvCombos.SelectedRows.Count > 0)
            {
                string sku = dgvCombos.SelectedRows[0].Cells[0].Value?.ToString() ?? "";
                MessageBox.Show($"Mở giao diện cập nhật giá sỉ/lẻ cho mã mặt hàng: {sku}", "Cập Nhật Giá", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng sản phẩm trên bảng để cập nhật giá.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnImportStore_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mở phân hệ Nhập kho nguyên liệu / F&B vào hệ thống.", "Nhập Kho F&B", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đã xuất dữ liệu danh mục Combo & Kho F&B ra file Excel thành công!", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadMockData()
        {
            dgvCombos.Rows.Clear();
            AddComboRow("SKU-CB-001", "Combo Couple IMAX Gold", "1 Bắp ngọt 85oz + 2 Pepsi 32oz", "Combo tiết kiệm", "#0F2738", "#38BDF8", "38.000 đ", "145.000 đ", "30");
            AddComboRow("SKU-CB-002", "Combo Solo Gold", "1 Bắp mặn 64oz + 1 Coca 22oz", "Combo tiết kiệm", "#0F2738", "#38BDF8", "24.500 đ", "95.000 đ", "18");
            AddComboRow("SKU-PC-003", "Bắp Phô Mai Savory 46oz", "Bột phô mai nhập khẩu New Zealand", "Bắp rang bơ", "#2A2012", "#FBBF24", "15.200 đ", "65.000 đ", "42");
            AddComboRow("SKU-PC-004", "Bắp Caramel Royal 64oz", "Caramel thắng nâu hạt tròn Mỹ", "Bắp rang bơ", "#2A2012", "#FBBF24", "19.800 đ", "79.000 đ", "25");
            AddComboRow("SKU-DK-005", "Coca-Cola Tươi 32oz Lớn", "Post-mix CO2 tiêu chuẩn 4.2 Bar", "Nước giải khát", "#0D2826", "#2DD4BF", "6.500 đ", "39.000 đ", "110");
            AddComboRow("SKU-DK-006", "Sprite Hương Chanh 32oz", "Post-mix CO2 tiêu chuẩn 4.0 Bar", "Nước giải khát", "#0D2826", "#2DD4BF", "6.500 đ", "39.000 đ", "95");
            AddComboRow("SKU-SN-007", "Snack Khoai Tây Nắm Truffle 110g", "Gói đóng sẵn nguyên seal nhập khẩu", "Snack đóng gói", "#221C2E", "#C084FC", "28.800 đ", "55.000 đ", "12");
            AddComboRow("SKU-CB-008", "Combo Suite VIP Premier", "Dành riêng cho cụm rạp Gold Lounge", "Combo tiết kiệm", "#0F2738", "#38BDF8", "62.000 đ", "220.000 đ", "8");
        }

        private void AddComboRow(string sku, string name, string desc, string category, string badgeBgHex, string badgeFgHex, string costPrice, string listPrice, string stock)
        {
            dgvCombos.Rows.Add(sku, null, name + "|" + desc, category + "|" + badgeBgHex + "|" + badgeFgHex, costPrice, listPrice, stock);
        }

        private void DgvCombos_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == 0)
            {
                e.PaintBackground(e.CellBounds, true);
                TextRenderer.DrawText(e.Graphics, e.Value?.ToString(), FontMono, e.CellBounds, ColorPrimaryBlue, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
                e.Handled = true;
            }
            else if (e.ColumnIndex == 2 && e.Value != null)
            {
                e.PaintBackground(e.CellBounds, true);
                string[] parts = e.Value.ToString().Split('|');
                if (parts.Length >= 2)
                {
                    Rectangle rectName = new Rectangle(e.CellBounds.X + 6, e.CellBounds.Y + 6, e.CellBounds.Width - 12, 20);
                    Rectangle rectDesc = new Rectangle(e.CellBounds.X + 6, e.CellBounds.Y + 26, e.CellBounds.Width - 12, 18);

                    TextRenderer.DrawText(e.Graphics, parts[0], FontInterBold, rectName, ColorTextWhite, TextFormatFlags.Left);
                    TextRenderer.DrawText(e.Graphics, parts[1], FontInterSmall, rectDesc, ColorTextMuted, TextFormatFlags.Left);
                }
                e.Handled = true;
            }
            else if (e.ColumnIndex == 3 && e.Value != null)
            {
                e.PaintBackground(e.CellBounds, true);
                string[] catParts = e.Value.ToString().Split('|');
                string catName = catParts[0];
                Color bgCol = ColorTranslator.FromHtml(catParts[1]);
                Color fgCol = ColorTranslator.FromHtml(catParts[2]);

                Size textSize = TextRenderer.MeasureText(catName, FontInterSmall);
                Rectangle badgeRect = new Rectangle(e.CellBounds.X + 6, e.CellBounds.Y + (e.CellBounds.Height - 24) / 2, textSize.Width + 16, 24);

                using (GraphicsPath path = GetRoundedRect(badgeRect, 6))
                {
                    using (Brush b = new SolidBrush(bgCol)) 
                    { 
                        e.Graphics.FillPath(b, path); 
                    }
                    using (Pen p = new Pen(fgCol, 1)) 
                    { 
                        e.Graphics.DrawPath(p, path); 
                    }
                }
                TextRenderer.DrawText(e.Graphics, catName, FontInterSmall, badgeRect, fgCol, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }
        }

        private GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();
            if (radius == 0) 
            { 
                path.AddRectangle(bounds); 
                return path; 
            }
            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;

            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;

            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
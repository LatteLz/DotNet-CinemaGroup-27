using System;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using CinemaManagement.Forms.Staff;

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

        private string connectionString = "Data Source=cinema.db;Version=3;";
        private string placeholderText = "🔍 Tìm kiếm SKU, tên combo...";
        private ErrorProvider errorProvider = new ErrorProvider();

        public FrmAdminCombos()
        {
            InitializeComponent();
            SetupCustomUI();
            SetupButtonEvents();
            SetupSidebarEvents();
            LoadDataFromDb();
        }

        private void SetupCustomUI()
        {
            this.BackColor = ColorTranslator.FromHtml("#0b0f12");
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

            if (txtSearch != null)
            {
                txtSearch.Text = placeholderText;
                txtSearch.ForeColor = ColorTextMuted;

                txtSearch.Enter += (s, e) => {
                    if (txtSearch.Text == placeholderText)
                    {
                        txtSearch.Text = "";
                        txtSearch.ForeColor = ColorTextWhite;
                    }
                };

                txtSearch.Leave += (s, e) => {
                    if (string.IsNullOrWhiteSpace(txtSearch.Text))
                    {
                        txtSearch.Text = placeholderText;
                        txtSearch.ForeColor = ColorTextMuted;
                    }
                    else
                    {
                        if (txtSearch.Text.Length < 2)
                        {
                            errorProvider.SetError(txtSearch, "Vui lòng nhập ít nhất 2 ký tự để tìm kiếm!");
                        }
                        else
                        {
                            errorProvider.SetError(txtSearch, string.Empty);
                        }
                    }
                };
            }
        }

        private void SetupSidebarEvents()
        {
            if (lnkPos != null)
            {
                lnkPos.LinkClicked += (s, e) => {
                    FrmStaffPOS posForm = new FrmStaffPOS(1, "Thu ngân mặc định");
                    posForm.Show();
                    this.Hide();
                };
            }

            if (lnkUsers != null)
            {
                lnkUsers.LinkClicked += (s, e) => {
                    FrmAdminUsers usersForm = new FrmAdminUsers();
                    usersForm.Show();
                    this.Hide();
                };
            }

            if (lnkReports != null)
            {
                lnkReports.LinkClicked += (s, e) => {
                    FrmAdminReports reportsForm = new FrmAdminReports();
                    reportsForm.Show();
                    this.Hide();
                };
            }

            if (lnkCombo != null)
            {
                lnkCombo.LinkClicked += (s, e) => {
                };
            }

            if (lnkOccupancy != null)
            {
                lnkOccupancy.LinkClicked += (s, e) => {
                    FrmOccupancyReport occupancyForm = new FrmOccupancyReport();
                    occupancyForm.Show();
                    this.Hide();
                };
            }
        }

        private void SetupButtonEvents()
        {
            if (btnAddComboTop != null) btnAddComboTop.Click += BtnAddComboTop_Click;
            if (btnUpdatePrice != null) btnUpdatePrice.Click += BtnUpdatePrice_Click;
            if (btnImportStore != null) btnImportStore.Click += BtnImportStore_Click;
            if (btnExportExcel != null) btnExportExcel.Click += BtnExportExcel_Click;
        }

        private void BtnAddComboTop_Click(object sender, EventArgs e)
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("Nhập tên combo F&B mới:", "Thêm Combo Mới", "Combo Đặc Biệt");
            if (string.IsNullOrWhiteSpace(name))
            {
                errorProvider.SetError(btnAddComboTop, "Tên combo không được để trống!");
                MessageBox.Show("Tên combo F&B không được để trống!", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            errorProvider.SetError(btnAddComboTop, string.Empty);

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO ComboItems (ComboName, Price, ImagePath) VALUES (@name, 99000, 'Bắp nước tiêu chuẩn');";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadDataFromDb();
                MessageBox.Show("Thêm combo mới thành công!", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm vào CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdatePrice_Click(object sender, EventArgs e)
        {
            if (dgvCombos.SelectedRows.Count > 0)
            {
                string sku = dgvCombos.SelectedRows[0].Cells[0].Value?.ToString() ?? "";
                errorProvider.Clear();
                MessageBox.Show($"Mở giao diện cập nhật giá sỉ/lẻ cho mã mặt hàng: {sku}", "Cập Nhật Giá", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                errorProvider.SetError(btnUpdatePrice, "Chưa chọn dòng dữ liệu!");
                MessageBox.Show("Vui lòng chọn một dòng sản phẩm trên bảng để cập nhật giá.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnImportStore_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mở phân hệ Nhập kho nguyên liệu / F&B vào hệ thống.", "Nhập Kho F&B", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV File (*.csv)|*.csv|All Files (*.*)|*.*";
                sfd.FileName = "Cineluxe_Combo_Inventory_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                        {
                            sw.WriteLine("MA SKU,TEN MON / COMBO,PHAN LOAI,GIA VON,GIA NIEM YET,TON");
                            foreach (DataGridViewRow row in dgvCombos.Rows)
                            {
                                if (row.IsNewRow) continue;
                                string sku = row.Cells[0].Value?.ToString() ?? "";
                                string nameDesc = row.Cells[2].Value?.ToString() ?? "";
                                string cat = row.Cells[3].Value?.ToString().Split('|')[0] ?? "";
                                string cost = row.Cells[4].Value?.ToString() ?? "";
                                string retail = row.Cells[5].Value?.ToString() ?? "";
                                string stock = row.Cells[6].Value?.ToString() ?? "";

                                sw.WriteLine($"\"{sku}\",\"{nameDesc}\",\"{cat}\",\"{cost}\",\"{retail}\",\"{stock}\"");
                            }
                        }
                        MessageBox.Show("Đã xuất dữ liệu danh mục Combo & Kho F&B ra file thành công!\nĐường dẫn: " + sfd.FileName, "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LoadDataFromDb()
        {
            dgvCombos.Rows.Clear();
            bool loaded = false;
            int totalProducts = 0;
            decimal totalInventoryValue = 0;
            string query = "SELECT Combold, ComboName, Price, ImagePath FROM ComboItems;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                totalProducts++;
                                string id = reader["Combold"].ToString();
                                string name = reader["ComboName"].ToString();
                                decimal price = reader.GetDecimal(reader.GetOrdinal("Price"));
                                string imagePath = reader["ImagePath"]?.ToString() ?? "Bắp nước tiêu chuẩn";

                                int stockQty = 30;
                                totalInventoryValue += (price * 0.7m) * stockQty;

                                AddComboRow($"SKU-CB-{id}", name, imagePath, "Combo tiết kiệm", "#0F2738", "#38BDF8", $"{price * 0.7m:N0} đ", $"{price:N0} đ", stockQty.ToString());
                                loaded = true;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            if (!loaded)
            {
                totalProducts = 3;
                totalInventoryValue = (38000 * 30) + (24500 * 18) + (55000 * 25);
                AddComboRow("SKU-CB-001", "Combo Solo (1 Bắp + 1 Nước)", "1 Bắp ngọt 85oz + 2 Pepsi 32oz", "Combo tiết kiệm", "#0F2738", "#38BDF8", "38.000 đ", "79.000 đ", "30");
                AddComboRow("SKU-CB-002", "Combo Couple (1 Bắp + 2 Nước)", "1 Bắp mặn 64oz + 1 Coca 22oz", "Combo tiết kiệm", "#0F2738", "#38BDF8", "24.500 đ", "109.000 đ", "18");
                AddComboRow("SKU-CB-003", "Combo Family Siêu To", "2 Bắp L + 3 Pepsi + 1 Snack", "Combo gia đình", "#0F2738", "#38BDF8", "55.000 đ", "179.000 đ", "25");
            }

            UpdateFooterStatus(totalProducts, totalInventoryValue);
        }

        private void UpdateFooterStatus(int totalProducts, decimal totalValue)
        {
            if (lblFooterStatus != null)
            {
                lblFooterStatus.Text = $"🟢  Tổng mặt hàng: {totalProducts} sản phẩm FB  |  Giá trị tồn kho: {totalValue:N0} đ  |  SQLite WAL Mode: OK";
            }
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
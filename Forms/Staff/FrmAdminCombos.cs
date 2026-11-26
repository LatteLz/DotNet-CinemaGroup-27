using System.Drawing.Drawing2D;
using CinemaManagement.Forms.Shared;
using CinemaManagement.Helpers;
using CinemaManagement.Models;
using CinemaManagement.Services;
using static CinemaManagement.Helpers.UiHelper;

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

        private readonly ComboService _comboService = new();
        private List<ComboItem> _combos = new();
        private string placeholderText = "🔍 Tìm kiếm SKU, tên combo...";
        private ErrorProvider errorProvider = new ErrorProvider();

        public FrmAdminCombos()
        {
            InitializeComponent();
            AppSession.RequireAdmin(this);
            SetupCustomUI();
            SetupButtonEvents();
            SetupSidebarEvents();
            LoadDataFromDb();
            WindowLayout.ScaleToStandard(this);
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
            dgvCombos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCombos.MultiSelect = false;
            dgvCombos.ReadOnly = true;
            dgvCombos.AllowUserToAddRows = false;

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

                txtSearch.TextChanged += (s, e) => RenderGrid();

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
            if (lnkPos != null) lnkPos.LinkClicked += (s, e) => StaffNavigator.Navigate<FrmStaffPOS>(this);
            if (lnkUsers != null) lnkUsers.LinkClicked += (s, e) => StaffNavigator.Navigate<FrmAdminUsers>(this);
            if (lnkReports != null) lnkReports.LinkClicked += (s, e) => StaffNavigator.Navigate<FrmAdminReports>(this);
            if (lnkOccupancy != null) lnkOccupancy.LinkClicked += (s, e) => StaffNavigator.Navigate<FrmOccupancyReport>(this);
        }

        private void SetupButtonEvents()
        {
            if (btnAddComboTop != null) btnAddComboTop.Click += BtnAddComboTop_Click;
            if (btnEditCombo != null) btnEditCombo.Click += BtnEditCombo_Click;
            if (btnDeleteCombo != null) btnDeleteCombo.Click += BtnDeleteCombo_Click;
            if (dgvCombos != null) dgvCombos.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnEditCombo_Click(s!, e); };
            if (btnExportExcel != null) btnExportExcel.Click += BtnExportExcel_Click;
        }

        private bool ShowAddComboDialog(out string comboName, out decimal price, out string description,
            out string? newImageFile, out bool removeImage, ComboItem? initial = null)
        {
            comboName = "";
            price = 0m;
            description = "";
            newImageFile = null;
            removeImage = false;
            string? pickedFile = null;
            bool cleared = false;

            using (Form dlg = new Form())
            {
                dlg.Text = initial == null ? "Thêm Combo Bắp Nước Mới" : $"Sửa Combo #{initial.ComboId}";
                dlg.Size = new Size(420, 330);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.BackColor = ColorSurfaceContainer;
                dlg.ForeColor = ColorTextWhite;

                Label lblName = new Label
                {
                    Text = "Tên combo bắp nước:",
                    Location = new Point(20, 18),
                    Size = new Size(360, 20),
                    Font = FontInterBold,
                    ForeColor = ColorTextMuted
                };
                TextBox txtName = new TextBox
                {
                    Location = new Point(20, 40),
                    Size = new Size(365, 26),
                    BackColor = ColorBg,
                    ForeColor = ColorTextWhite,
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = initial?.ComboName ?? ""
                };

                Label lblPrice = new Label
                {
                    Text = "Giá niêm yết (VNĐ):",
                    Location = new Point(20, 74),
                    Size = new Size(360, 20),
                    Font = FontInterBold,
                    ForeColor = ColorTextMuted
                };
                TextBox txtPrice = new TextBox
                {
                    Location = new Point(20, 96),
                    Size = new Size(365, 26),
                    BackColor = ColorBg,
                    ForeColor = ColorTextWhite,
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = initial != null ? initial.Price.ToString("0") : ""
                };

                Label lblDesc = new Label
                {
                    Text = "Mô tả / Bắp nước đi kèm:",
                    Location = new Point(20, 130),
                    Size = new Size(360, 20),
                    Font = FontInterBold,
                    ForeColor = ColorTextMuted
                };
                TextBox txtDesc = new TextBox
                {
                    Location = new Point(20, 152),
                    Size = new Size(365, 26),
                    BackColor = ColorBg,
                    ForeColor = ColorTextWhite,
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = initial?.Description ?? ""
                };

                Button btnOk = new Button
                {
                    Text = "Lưu vào CSDL",
                    DialogResult = DialogResult.OK,
                    Location = new Point(195, 250),
                    Size = new Size(105, 30),
                    BackColor = ColorPrimaryBlue,
                    ForeColor = ColorTextWhite,
                    FlatStyle = FlatStyle.Flat
                };
                btnOk.FlatAppearance.BorderSize = 0;

                Button btnCancel = new Button
                {
                    Text = "Hủy",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(308, 250),
                    Size = new Size(77, 30),
                    BackColor = ColorBorder,
                    ForeColor = ColorTextWhite,
                    FlatStyle = FlatStyle.Flat
                };
                btnCancel.FlatAppearance.BorderSize = 0;

                Label lblImage = new Label
                {
                    Text = "Ảnh combo (không bắt buộc):",
                    Location = new Point(20, 186),
                    Size = new Size(360, 20),
                    Font = FontInterBold,
                    ForeColor = ColorTextMuted
                };
                Label lblImageFile = new Label
                {
                    Location = new Point(214, 214),
                    Size = new Size(172, 20),
                    AutoEllipsis = true,
                    ForeColor = ColorTextWhite,
                    Text = string.IsNullOrWhiteSpace(initial?.ImagePath) ? "(chưa có ảnh)" : System.IO.Path.GetFileName(initial!.ImagePath)
                };
                Button btnPickImage = new Button
                {
                    Text = "Chọn ảnh...",
                    Location = new Point(20, 208),
                    Size = new Size(110, 30),
                    BackColor = ColorBorder,
                    ForeColor = ColorTextWhite,
                    FlatStyle = FlatStyle.Flat
                };
                btnPickImage.FlatAppearance.BorderSize = 0;
                Button btnClearImage = new Button
                {
                    Text = "Bỏ ảnh",
                    Location = new Point(136, 208),
                    Size = new Size(70, 30),
                    BackColor = ColorBorder,
                    ForeColor = ColorTextWhite,
                    FlatStyle = FlatStyle.Flat
                };
                btnClearImage.FlatAppearance.BorderSize = 0;

                btnPickImage.Click += (_, _) =>
                {
                    using var ofd = new OpenFileDialog
                    {
                        Title = "Chọn ảnh combo",
                        Filter = "Ảnh (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp"
                    };
                    if (ofd.ShowDialog(dlg) != DialogResult.OK) return;
                    using var test = ImageHelper.Load(ofd.FileName);
                    if (test == null)
                    {
                        MessageBox.Show("File được chọn không phải ảnh hợp lệ.", "Ảnh không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    pickedFile = ofd.FileName;
                    cleared = false;
                    lblImageFile.Text = System.IO.Path.GetFileName(ofd.FileName);
                };
                btnClearImage.Click += (_, _) =>
                {
                    pickedFile = null;
                    cleared = true;
                    lblImageFile.Text = "(chưa có ảnh)";
                };

                dlg.Controls.AddRange(new Control[] { lblName, txtName, lblPrice, txtPrice, lblDesc, txtDesc,
                    lblImage, btnPickImage, btnClearImage, lblImageFile, btnOk, btnCancel });
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCancel;

                using var dlgErrors = new ErrorProvider { ContainerControl = dlg, BlinkStyle = ErrorBlinkStyle.NeverBlink };
                while (true)
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK)
                    {
                        return false;
                    }

                    dlgErrors.Clear();
                    string inputName = txtName.Text.Trim();
                    string inputDesc = txtDesc.Text.Trim();

                    if (string.IsNullOrWhiteSpace(inputName))
                    {
                        dlgErrors.SetError(txtName, "Tên combo bắp nước không được để trống");
                        txtName.Focus();
                        continue;
                    }

                    if (!ValidationHelper.IsPositiveMoney(txtPrice.Text, out decimal parsedPrice))
                    {
                        dlgErrors.SetError(txtPrice, "Giá niêm yết phải là số lớn hơn 0");
                        txtPrice.Focus();
                        continue;
                    }

                    comboName = inputName;
                    price = parsedPrice;
                    description = inputDesc;
                    newImageFile = pickedFile;
                    removeImage = cleared;
                    return true;
                }
            }
        }

        private void BtnAddComboTop_Click(object sender, EventArgs e)
        {
            if (!ShowAddComboDialog(out string name, out decimal price, out string description, out string? newImage, out _))
                return;

            errorProvider.SetError(btnAddComboTop, string.Empty);
            string? imagePath = null;
            bool saved = false;
            try
            {
                if (newImage != null) imagePath = ImageHelper.SaveImageCopy(newImage, "combo");
                _comboService.AddCombo(new ComboItem { ComboName = name, Price = price, Description = description, ImagePath = imagePath });
                saved = true;
                LoadDataFromDb();
                MessageBox.Show("Đã thêm combo bắp nước thành công!", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (BusinessException ex)
            {
                MessageBox.Show(ex.Message, "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm vào CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!saved) ImageHelper.DeleteUploadedPoster(imagePath);
            }
        }

        private ComboItem? GetSelectedCombo()
        {
            var row = dgvCombos.SelectedRows.Count > 0 ? dgvCombos.SelectedRows[0] : dgvCombos.CurrentRow;
            if (row == null || row.Tag is not int comboId)
            {
                errorProvider.SetError(btnEditCombo, "Chưa chọn dòng dữ liệu!");
                MessageBox.Show("Vui lòng chọn một combo trên bảng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            errorProvider.Clear();
            return _combos.FirstOrDefault(c => c.ComboId == comboId);
        }

        private void BtnEditCombo_Click(object sender, EventArgs e)
        {
            var combo = GetSelectedCombo();
            if (combo == null) return;

            if (!ShowAddComboDialog(out string name, out decimal price, out string description, out string? newImage, out bool removeImage, combo))
                return;

            string? copiedImage = null;
            bool saved = false;
            try
            {
                if (newImage != null) copiedImage = ImageHelper.SaveImageCopy(newImage, "combo");
                string? imagePath = copiedImage ?? (removeImage ? null : combo.ImagePath);
                _comboService.UpdateCombo(new ComboItem
                {
                    ComboId = combo.ComboId,
                    ComboName = name,
                    Price = price,
                    Description = description,
                    ImagePath = imagePath,
                    IsActive = combo.IsActive
                });
                saved = true;
                if (imagePath != combo.ImagePath) ImageHelper.DeleteUploadedPoster(combo.ImagePath);
                LoadDataFromDb();
                MessageBox.Show("Đã cập nhật combo.", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (BusinessException ex)
            {
                MessageBox.Show(ex.Message, "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!saved) ImageHelper.DeleteUploadedPoster(copiedImage);
            }
        }

        private void BtnDeleteCombo_Click(object sender, EventArgs e)
        {
            var combo = GetSelectedCombo();
            if (combo == null) return;

            if (MessageBox.Show($"Xóa combo '{combo.ComboName}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                bool deleted = _comboService.DeleteCombo(combo.ComboId);
                LoadDataFromDb();
                MessageBox.Show(deleted
                        ? "Đã xóa combo."
                        : "Combo đã có trong hóa đơn nên được chuyển sang 'Ngừng bán' (không xóa để giữ số liệu doanh thu).",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (BusinessException ex)
            {
                MessageBox.Show(ex.Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa combo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "CSV File (*.csv)|*.csv|All Files (*.*)|*.*",
                FileName = "Cineluxe_Combo_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv"
            };
            if (sfd.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                var sb = new System.Text.StringBuilder();
                sb.AppendLine("Mã combo,Tên combo,Mô tả,Trạng thái,Giá bán,File ảnh");
                foreach (var c in _combos)
                {
                    sb.AppendLine(string.Join(",",
                        Csv($"CB-{c.ComboId:D3}"), Csv(c.ComboName), Csv(c.Description), Csv(c.IsActive ? "Đang bán" : "Ngừng bán"),
                        c.Price.ToString("0"), Csv(string.IsNullOrWhiteSpace(c.ImagePath) ? "" : System.IO.Path.GetFileName(c.ImagePath))));
                }
                System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), new System.Text.UTF8Encoding(true));
                MessageBox.Show("Đã xuất danh mục combo ra file:\n" + sfd.FileName, "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) when (ex is System.IO.IOException or UnauthorizedAccessException)
            {
                MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDataFromDb()
        {
            try
            {
                _combos = _comboService.GetAllCombos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách combo: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _combos = new List<ComboItem>();
            }

            RenderGrid();
        }

        private void RenderGrid()
        {
            dgvCombos.Rows.Clear();

            string keyword = txtSearch == null || txtSearch.Text == placeholderText ? "" : txtSearch.Text.Trim();
            var list = _combos.Where(c => keyword.Length < 2
                                          || c.ComboName.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                          || $"CB-{c.ComboId:D3}".Contains(keyword, StringComparison.OrdinalIgnoreCase))
                              .ToList();

            foreach (var c in list)
            {
                string status = c.IsActive ? "Đang bán" : "Ngừng bán";
                string bg = c.IsActive ? "#0F2738" : "#3B1D1D";
                string fg = c.IsActive ? "#38BDF8" : "#F87171";
                int rowIndex = AddComboRow($"CB-{c.ComboId:D3}", c.ComboName, c.Description ?? "", status, bg, fg,
                    string.IsNullOrWhiteSpace(c.ImagePath) ? "—" : System.IO.Path.GetFileName(c.ImagePath),
                    $"{c.Price:N0} đ");
                dgvCombos.Rows[rowIndex].Tag = c.ComboId;
            }

            if (lblFooterStatus != null)
            {
                lblFooterStatus.Text = $"🟢  Tổng: {_combos.Count} combo  |  Đang bán: {_combos.Count(c => c.IsActive)}  |  Hiển thị: {list.Count}";
            }
        }

        private int AddComboRow(string code, string name, string desc, string status, string badgeBgHex, string badgeFgHex, string imageFile, string price)
        {
            return dgvCombos.Rows.Add(code, null, name + "|" + desc, status + "|" + badgeBgHex + "|" + badgeFgHex, imageFile, price);
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

                using (GraphicsPath path = RoundedRect(badgeRect, 6))
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
    }
}
using System;
using System.Drawing;
using System.Windows.Forms;
using CinemaManagement.Forms.Staff;
using CinemaManagement.Forms.Admin;
using CinemaManagement.Forms.Kiosk;

namespace CinemaManagement.Forms.Admin
{
    public partial class FrmRoleSelection : Form
    {
        private Panel? pnlSidebar;
        private Panel? pnlHeader;
        private Panel? pnlFooter;
        private Label? lblTitle;
        private Label? lblSubtitle;
        
        private Panel? cardAdmin;
        private Panel? cardKiosk;
        private Panel? cardPos;

        public FrmRoleSelection()
        {
            InitializeComponent();
            SetupCustomUI();
        }

        private void SetupCustomUI()
        {
            this.Text = "CineLuxe Cinema Management System - .NET 10.0 CLR [x64]";
            this.Size = new Size(1280, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(16, 20, 24); 
            this.ForeColor = Color.White;

            pnlSidebar = new Panel();
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Width = 240;
            pnlSidebar.BackColor = Color.FromArgb(22, 27, 34);
            
            Label lblMenuTitle = new Label();
            lblMenuTitle.Text = "TRUNG TÂM VẬN HÀNH";
            lblMenuTitle.ForeColor = Color.Gray;
            lblMenuTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMenuTitle.Location = new Point(16, 20);
            lblMenuTitle.AutoSize = true;
            pnlSidebar.Controls.Add(lblMenuTitle);

            string[] menuItems = { "Chọn vai trò", "Bảng điều khiển", "Quản lý danh mục phim", "Chi tiết phim", "Cấu hình phòng chiếu", "Lịch chiếu phim" };
            for (int i = 0; i < menuItems.Length; i++)
            {
                Button btnMenu = new Button();
                btnMenu.Text = "  " + menuItems[i];
                btnMenu.TextAlign = ContentAlignment.MiddleLeft;
                btnMenu.FlatStyle = FlatStyle.Flat;
                btnMenu.FlatAppearance.BorderSize = 0;
                btnMenu.BackColor = (i == 0) ? Color.FromArgb(2, 132, 199) : Color.Transparent;
                btnMenu.ForeColor = Color.White;
                btnMenu.Font = new Font("Segoe UI", 10F);
                btnMenu.Size = new Size(220, 40);
                btnMenu.Location = new Point(10, 50 + (i * 45));
                pnlSidebar.Controls.Add(btnMenu);
            }
            this.Controls.Add(pnlSidebar);

            pnlHeader = new Panel();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 130;
            pnlHeader.BackColor = Color.FromArgb(16, 20, 24);

            lblTitle = new Label();
            lblTitle.Text = "Chào mừng đến với Hệ thống Quản trị & Vận hành Rạp phim ";
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(260, 20);
            lblTitle.AutoSize = true;
            pnlHeader.Controls.Add(lblTitle);

            lblSubtitle = new Label();
            lblSubtitle.Text = "Vui lòng chọn cổng truy cập tương ứng với vai trò của bạn.";
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.DarkGray;
            lblSubtitle.Location = new Point(263, 65);
            lblSubtitle.AutoSize = true;
            pnlHeader.Controls.Add(lblSubtitle);

            cardAdmin = CreateCard("Hệ thống Quản trị Admin", "DIRECTOR & OPS SUITE", 
                "Quản lý phim, phòng chiếu, lịch chiếu, thống kê doanh thu và cấu hình hệ thống chuyên sâu cho quản lý rạp.", 
                "Truy cập Admin Console →", Color.FromArgb(2, 132, 199), 270, (sender, e) => {
                    FrmLogin loginForm = new FrmLogin();
                    this.Hide();
                    loginForm.ShowDialog();
                    this.Show();
                });
            this.Controls.Add(cardAdmin);

            cardKiosk = CreateCard("Ki-ốt Khách hàng Tự phục vụ", "CUSTOMER SELF-SERVICE", 
                "Dành cho khách hàng tra cứu phim, chọn suất chiếu, đặt ghế và tự thanh toán in vé tự động nhanh chóng.", 
                "Khởi chạy Chế độ Ki-ốt →", Color.FromArgb(217, 119, 6), 610, (sender, e) => {
                    FrmKioskHome kioskHome = new FrmKioskHome();
                    this.Hide();
                    kioskHome.ShowDialog();
                    this.Show();
                });
            this.Controls.Add(cardKiosk);

            cardPos = CreateCard("Quầy POS Nhân viên Thu ngân", "CASHIER FRONT DESK", 
                "Dành cho nhân viên bán vé trực tiếp tại quầy, xuất hóa đơn nhanh và quản lý combo bắp nước đi kèm.", 
                "Mở Giao diện POS →", Color.FromArgb(2, 132, 199), 950, (sender, e) => {
                    FrmLogin loginForm = new FrmLogin();
                    this.Hide();
                    loginForm.ShowDialog();
                    this.Show();
                });
            this.Controls.Add(cardPos);

            pnlFooter = new Panel();
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 50;
            pnlFooter.BackColor = Color.FromArgb(22, 27, 34);

            Label lblStatus = new Label();
            lblStatus.Text = "  MÁY CHỦ NỘI BỘ: 123.0.0.4:5000 (ONLINE)  ";
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.DarkGray;
            lblStatus.Location = new Point(250, 15);
            lblStatus.AutoSize = true;
            pnlFooter.Controls.Add(lblStatus);

            this.Controls.Add(pnlFooter);
        }

        private Panel CreateCard(string title, string badge, string desc, string buttonText, Color btnColor, int leftPos, EventHandler onClickAction)
        {
            Panel card = new Panel();
            card.Size = new Size(310, 480);
            card.Location = new Point(leftPos, 160);
            card.BackColor = Color.FromArgb(22, 27, 34);
            card.BorderStyle = BorderStyle.FixedSingle;

            Label lblBadge = new Label();
            lblBadge.Text = badge;
            lblBadge.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblBadge.ForeColor = btnColor;
            lblBadge.Location = new Point(20, 25);
            lblBadge.AutoSize = true;
            card.Controls.Add(lblBadge);

            Label lblCardTitle = new Label();
            lblCardTitle.Text = title;
            lblCardTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblCardTitle.ForeColor = Color.White;
            lblCardTitle.Location = new Point(20, 55);
            lblCardTitle.Size = new Size(270, 70);
            card.Controls.Add(lblCardTitle);

            Label lblDesc = new Label();
            lblDesc.Text = desc;
            lblDesc.Font = new Font("Segoe UI", 10F);
            lblDesc.ForeColor = Color.DarkGray;
            lblDesc.Location = new Point(20, 145);
            lblDesc.Size = new Size(270, 100);
            card.Controls.Add(lblDesc);

            Button btnAction = new Button();
            btnAction.Text = buttonText;
            btnAction.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAction.BackColor = btnColor;
            btnAction.ForeColor = Color.White;
            btnAction.FlatStyle = FlatStyle.Flat;
            btnAction.FlatAppearance.BorderSize = 0;
            btnAction.Size = new Size(270, 45);
            btnAction.Location = new Point(20, 400);
            btnAction.Cursor = Cursors.Hand;
            btnAction.Click += onClickAction;
            card.Controls.Add(btnAction);

            return card;
        }
    }
}
using System;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.Forms.Admin
{
    public partial class FrmAdminDashboard : Form
    {
        private Panel pnlSidebar;
        private Panel pnlHeader;
        private Panel pnlContent;
        private Panel pnlFooter;

        public FrmAdminDashboard()
        {
            InitializeComponent();
            SetupDashboardLayout();
        }

        private void SetupDashboardLayout()
        {
            this.Width = 1280;
            this.Height = 720;
            this.StartPosition=FormStartPosition.CenterScreen;
            this.Text = "Cinema Management System";
            this.BackColor=ColorTranslator.FromHtml("#101418");
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            
            pnlSidebar=new Panel();
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Width = 256;
            pnlSidebar.BackColor= ColorTranslator.FromHtml("#0B0F12");
            this.Controls.Add(pnlSidebar);

            AddSidebarMenuButton("Quản lý danh mục phim ", 100);
            AddSidebarMenuButton("Chi tiết phim", 150);
            AddSidebarMenuButton("Cấu hình phòng chiếu", 200);
            AddSidebarMenuButton("Lịch chiếu phim", 250);

            pnlHeader = new Panel();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 48;
            pnlHeader.BackColor = ColorTranslator.FromHtml("#0B0F12");
            this.Controls.Add(pnlHeader);
            
            Label lblTitle = new Label();
            lblTitle.Text = "ADMIN_DASHBOARD.CS";
            lblTitle.ForeColor=ColorTranslator.FromHtml("#CBD5E1");
            lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitle.Location = new Point(20, 12);
            lblTitle.AutoSize = true;
            pnlHeader.Controls.Add(lblTitle);
            
            pnlFooter = new Panel();
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 32;
            pnlFooter.BackColor = ColorTranslator.FromHtml("#0B0F12");
            this.Controls.Add(pnlFooter);

            Label lblFooterStatus = new Label();
            lblFooterStatus.Text = "PROJECTION ENGINE:  READY   |   SQLite Connected: cineluxe_core.db (v3.42)";
            lblFooterStatus.ForeColor = ColorTranslator.FromHtml("#94A3B8");
            lblFooterStatus.Font = new Font("JetBrains Mono", 9F, FontStyle.Regular);
            lblFooterStatus.Location = new Point(15, 8);
            lblFooterStatus.AutoSize = true;
            pnlFooter.Controls.Add(lblFooterStatus);
            pnlContent = new Panel();
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.BackColor = ColorTranslator.FromHtml("#101418");
            pnlContent.AutoScroll = true;
            this.Controls.Add(pnlContent);
            CreateKpiCards();
        }

        private void AddSidebarMenuButton(string text, int topPosition)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Width = 220;
            btn.Height = 40;
            btn.Location = new Point(18, topPosition);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.ForeColor = ColorTranslator.FromHtml("#CBD5E1");
            btn.BackColor = ColorTranslator.FromHtml("#181C20");
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.MouseEnter += (s, e) => btn.BackColor = ColorTranslator.FromHtml("#282E35");
            btn.MouseLeave += (s, e) => btn.BackColor = ColorTranslator.FromHtml("#181C20");

            pnlSidebar.Controls.Add(btn);
        }

        private void CreateKpiCards()
        {
            Panel card1 = CreateMetricCard("DOANH THU HÔM NAY", "48.750.000 đ", "+18.4% vs hôm qua", 24, 20);
            pnlContent.Controls.Add(card1);
            Panel card2 = CreateMetricCard("SỐ VÉ ĐÃ BÁN", "582 / 650 VÉ", "Tiến độ ngày: 89.5%", 314, 20);
            pnlContent.Controls.Add(card2);
            Panel card3 = CreateMetricCard("TỶ LỆ LẤP ĐẦY RẠP", "78.5%", "+6.2% chu kỳ tuần", 604, 20);
            pnlContent.Controls.Add(card3);
            Panel card4 = CreateMetricCard("SUẤT CHIẾU HOẠT ĐỘNG", "24 / 26 SUẤT", "2 suất chờ khởi chiếu", 894, 20);
            pnlContent.Controls.Add(card4);
        }

        private Panel CreateMetricCard(string title, string value, string subtitle, int left, int top)
        {
            Panel panel = new Panel();
            panel.Width = 266;
            panel.Height = 110;
            panel.Location = new Point(left, top);
            panel.BackColor = ColorTranslator.FromHtml("#181C20");
            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.ForeColor = ColorTranslator.FromHtml("#94A3B8");
            lblTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTitle.Location = new Point(16, 14);
            lblTitle.AutoSize = true;
            panel.Controls.Add(lblTitle);
            Label lblValue = new Label();
            lblValue.Text = value;
            lblValue.ForeColor = ColorTranslator.FromHtml("#F39C12"); 
            lblValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblValue.Location = new Point(14, 38);
            lblValue.AutoSize = true;
            panel.Controls.Add(lblValue);
            Label lblSub = new Label();
            lblSub.Text = subtitle;
            lblSub.ForeColor = ColorTranslator.FromHtml("#CBD5E1");
            lblSub.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            lblSub.Location = new Point(16, 78);
            lblSub.AutoSize = true;
            panel.Controls.Add(lblSub);

            return panel;
        }
    }
}
        
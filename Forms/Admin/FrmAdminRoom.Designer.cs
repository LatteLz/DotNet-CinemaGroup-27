using System.ComponentModel;

namespace CinemaManagement.Forms.Admin
{
    partial class FrmAdminRooms
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 650);
            this.Text = "CINELUXE - Quản Lý Phòng Chiếu & Tự Động Sinh Ghế";
        }

        #endregion
    }
}
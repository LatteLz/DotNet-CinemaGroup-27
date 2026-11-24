using CinemaManagement.Data;
using CinemaManagement.Forms.Admin;

namespace CinemaManagement
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            try
            {
                DbInitializer.Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không khởi tạo được cơ sở dữ liệu cinema.db:\n" + ex.Message,
                    "Lỗi khởi động", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new FrmRoleSelection());
        }
    }
}

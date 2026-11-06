using CinemaManagement.Data;
using CinemaManagement.Forms.Admin;

namespace CinemaManagement
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            using (var context = new CinemaDbContext())
            {
                DbInitializer.Initialize(context);
            }

            Application.Run(new FrmRoleSelection());
        }
    }
}
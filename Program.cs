using System;
using System.Windows.Forms;
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
            
            using (var context = new CinemaDbContext())
            {
                DbInitializer.Initialize(context);
            }

            Application.Run(new FrmRoleSelection());
        }
    }
}
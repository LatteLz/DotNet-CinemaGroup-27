using CinemaManagement.Helpers;

namespace CinemaManagement.Forms.Shared
{
    public static class StaffNavigator
    {
        public static void Navigate<T>(Form current) where T : Form, new()
        {
            if (current is T) return;

            var existing = Application.OpenForms.OfType<T>().FirstOrDefault(f => f != current);
            if (existing != null)
            {
                current.Close();
                return;
            }

            bool currentClosed = false;
            void OnCurrentClosed(object? s, FormClosedEventArgs e) => currentClosed = true;
            current.FormClosed += OnCurrentClosed;

            current.Hide();
            using (var target = new T())
            {
                target.StartPosition = FormStartPosition.CenterScreen;
                target.ShowDialog();
            }

            current.FormClosed -= OnCurrentClosed;
            if (!currentClosed && !current.IsDisposed) current.Show();
        }

        public static void ReturnToRoleSelection(Form current)
        {
            var forms = Application.OpenForms.Cast<Form>()
                .Where(f => f is not Admin.FrmRoleSelection && f is not Staff.FrmLogin && f is not Kiosk.FrmKioskHome
                            && f.GetType().Namespace != typeof(Kiosk.KioskFlow).Namespace)
                .Reverse()
                .ToList();

            foreach (var f in forms)
            {
                if (!f.IsDisposed) f.Close();
            }
        }

        public static void ApplyRoleVisibility(params Control?[] adminOnlyControls)
        {
            foreach (var c in adminOnlyControls)
            {
                if (c != null) c.Visible = AppSession.IsAdmin;
            }
        }
    }
}

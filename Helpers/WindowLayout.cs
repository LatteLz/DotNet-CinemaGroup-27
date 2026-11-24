namespace CinemaManagement.Helpers
{
    public static class WindowLayout
    {
        public static readonly Size StandardClientSize = new(1440, 900);

        public static readonly Size KioskDesignSize = new(1920, 1080);

        public static Size FitToScreen(Size desiredClientSize)
        {
            Rectangle work = Screen.PrimaryScreen?.WorkingArea ?? new Rectangle(0, 0, 1920, 1040);
            const int frameWidth = 16, frameHeight = 39;
            return new Size(
                Math.Min(desiredClientSize.Width, work.Width - frameWidth),
                Math.Min(desiredClientSize.Height, work.Height - frameHeight));
        }

        public static void ApplyStandardSize(Form form)
        {
            form.MinimumSize = Size.Empty;
            form.ClientSize = FitToScreen(StandardClientSize);
            form.StartPosition = FormStartPosition.CenterScreen;
        }

        public static void ScaleToStandard(Form form)
        {
            ScaleContent(form, FitToScreen(StandardClientSize));
            form.StartPosition = FormStartPosition.CenterScreen;
        }

        public static void ScaleToScreen(Form kioskForm)
        {
            Rectangle bounds = Screen.PrimaryScreen?.Bounds ?? new Rectangle(Point.Empty, KioskDesignSize);
            ScaleContent(kioskForm, bounds.Size);
            kioskForm.StartPosition = FormStartPosition.Manual;
            kioskForm.Location = bounds.Location;
        }

        private static void ScaleContent(Form form, Size target)
        {
            Size design = form.ClientSize;
            if (design.Width <= 0 || design.Height <= 0 || design == target) return;

            float sx = target.Width / (float)design.Width;
            float sy = target.Height / (float)design.Height;
            float fontFactor = Math.Min(sx, sy);

            form.SuspendLayout();
            form.MinimumSize = Size.Empty;

            var originalFonts = new List<(Control Control, Font Font)>();
            CollectFonts(form, originalFonts);

            foreach (Control child in form.Controls)
            {
                child.Scale(new SizeF(sx, sy));
            }

            if (Math.Abs(fontFactor - 1f) > 0.03f)
            {
                foreach (var (control, font) in originalFonts)
                {
                    control.Font = new Font(font.FontFamily, font.Size * fontFactor, font.Style, font.Unit);
                }
            }

            form.ClientSize = target;
            form.ResumeLayout(true);
        }

        private static void CollectFonts(Control parent, List<(Control, Font)> fonts)
        {
            foreach (Control child in parent.Controls)
            {
                fonts.Add((child, child.Font));
                CollectFonts(child, fonts);
            }
        }
    }
}

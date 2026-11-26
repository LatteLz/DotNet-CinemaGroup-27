namespace CinemaManagement.Helpers
{
    public static class ImageHelper
    {
        public static string? ResolvePath(string? relativeOrAbsolutePath)
        {
            if (string.IsNullOrWhiteSpace(relativeOrAbsolutePath)) return null;

            string path = relativeOrAbsolutePath.Trim();
            if (Path.IsPathRooted(path))
                return File.Exists(path) ? path : null;

            string clean = path.TrimStart('\\', '/');
            string[] candidates =
            {
                Path.Combine(Application.StartupPath, clean),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", clean),
                Path.Combine(Directory.GetCurrentDirectory(), clean)
            };

            foreach (var c in candidates)
            {
                if (File.Exists(c)) return Path.GetFullPath(c);
            }
            return null;
        }

        public static string SavePosterCopy(string sourceFile) => SaveImageCopy(sourceFile, "poster");

        public static string SaveImageCopy(string sourceFile, string prefix)
        {
            string targetDir = Path.Combine(Application.StartupPath, "Image");
            Directory.CreateDirectory(targetDir);

            if (!IsUploadedFileName(Path.GetFileName(sourceFile))
                && string.Equals(Path.GetDirectoryName(Path.GetFullPath(sourceFile)), Path.GetFullPath(targetDir), StringComparison.OrdinalIgnoreCase))
                return "Image/" + Path.GetFileName(sourceFile);

            string fileName = $"{prefix}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N")[..6]}{Path.GetExtension(sourceFile)}";
            File.Copy(sourceFile, Path.Combine(targetDir, fileName));
            return "Image/" + fileName;
        }

        public static void DeleteUploadedPoster(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return;
            string fileName = Path.GetFileName(relativePath);
            if (!IsUploadedFileName(fileName)) return;

            string fullPath = Path.Combine(Application.StartupPath, "Image", fileName);
            try
            {
                if (File.Exists(fullPath)) File.Delete(fullPath);
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }

        private static bool IsUploadedFileName(string fileName)
            => fileName.StartsWith("poster_", StringComparison.OrdinalIgnoreCase)
               || fileName.StartsWith("combo_", StringComparison.OrdinalIgnoreCase);

        public static Image? Load(string? relativeOrAbsolutePath)
        {
            string? fullPath = ResolvePath(relativeOrAbsolutePath);
            if (fullPath == null) return null;

            try
            {
                using var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                using var temp = Image.FromStream(fs);
                return new Bitmap(temp);
            }
            catch (Exception ex) when (ex is IOException or ArgumentException or OutOfMemoryException)
            {
                return null;
            }
        }
    }
}

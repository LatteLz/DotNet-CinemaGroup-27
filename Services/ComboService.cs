using CinemaManagement.Data;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class ComboService
    {
        public List<ComboItem> GetActiveCombos()
        {
            using var db = DbFactory.Create();
            return db.ComboItems.AsNoTracking().Where(c => c.IsActive).OrderBy(c => c.ComboId).ToList();
        }

        public List<ComboItem> GetAllCombos()
        {
            using var db = DbFactory.Create();
            return db.ComboItems.AsNoTracking().OrderBy(c => c.ComboId).ToList();
        }

        public void AddCombo(ComboItem combo)
        {
            Validate(combo);
            using var db = DbFactory.Create();
            string lowerName = combo.ComboName.ToLower();
            if (db.ComboItems.Any(c => c.ComboName.ToLower() == lowerName))
                throw new BusinessException($"Đã có combo tên '{combo.ComboName}'.");
            combo.IsActive = true;
            db.ComboItems.Add(combo);
            db.SaveChanges();
        }

        public void UpdateCombo(ComboItem combo)
        {
            Validate(combo);
            using var db = DbFactory.Create();
            var existing = db.ComboItems.Find(combo.ComboId)
                           ?? throw new BusinessException("Không tìm thấy combo cần cập nhật.");
            string lowerName = combo.ComboName.ToLower();
            if (db.ComboItems.Any(c => c.ComboName.ToLower() == lowerName && c.ComboId != combo.ComboId))
                throw new BusinessException($"Đã có combo tên '{combo.ComboName}'.");

            existing.ComboName = combo.ComboName;
            existing.Description = combo.Description;
            existing.Price = combo.Price;
            existing.ImagePath = combo.ImagePath;
            existing.IsActive = combo.IsActive;
            db.SaveChanges();
        }

        public bool DeleteCombo(int comboId)
        {
            using var db = DbFactory.Create();
            var combo = db.ComboItems.Find(comboId) ?? throw new BusinessException("Không tìm thấy combo.");

            if (db.BookingCombos.Any(bc => bc.ComboId == comboId))
            {
                combo.IsActive = false;
                db.SaveChanges();
                return false;
            }

            db.ComboItems.Remove(combo);
            db.SaveChanges();
            return true;
        }

        private static void Validate(ComboItem combo)
        {
            combo.ComboName = (combo.ComboName ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(combo.ComboName))
                throw new BusinessException("Tên combo không được để trống.");
            if (combo.ComboName.Length > 100)
                throw new BusinessException("Tên combo tối đa 100 ký tự.");
            if (combo.Price <= 0)
                throw new BusinessException("Giá combo phải lớn hơn 0.");
        }
    }
}

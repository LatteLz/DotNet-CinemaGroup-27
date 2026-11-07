using System.Collections.Generic;
using System.Linq;
using CinemaManagement.Data;
using CinemaManagement.Models;

namespace CinemaManagement.Services
{
    public class ComboService
    {
        public List<ComboItem> GetAllCombos()
        {
            using var context = new CinemaDbContext(); 
            return context.ComboItems.ToList();
        }
        
        public void AddCombo(ComboItem combo)
        {
            using var context = new CinemaDbContext();
            context.ComboItems.Add(combo);
            context.SaveChanges();
        }
        
        public void UpdateCombo(ComboItem combo)
        {
            using var context = new CinemaDbContext();
            context.ComboItems.Update(combo);
            context.SaveChanges();
        }

        public void DeleteCombo(int comboId)
        {
            using var context = new CinemaDbContext();
            var combo = context.ComboItems.Find(comboId);
            if (combo != null)
            {
                context.ComboItems.Remove(combo);
                context.SaveChanges();
            }
        }
    }
}
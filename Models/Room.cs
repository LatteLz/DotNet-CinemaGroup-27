using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int RowCount { get; set; }
        public int ColCount { get; set; }
    }
}
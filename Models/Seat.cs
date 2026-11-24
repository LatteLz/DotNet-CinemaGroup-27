using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }

        public int RoomId { get; set; }

        [ForeignKey(nameof(RoomId))]
        public virtual Room Room { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string SeatNumber { get; set; } = string.Empty;

        public int RowIndex { get; set; }

        public int ColIndex { get; set; }

        [Required]
        [StringLength(50)]
        public string SeatType { get; set; } = SeatTypes.Standard;

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        public virtual ICollection<SeatHold> SeatHolds { get; set; } = new List<SeatHold>();
    }
}

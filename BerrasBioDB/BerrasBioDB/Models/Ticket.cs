using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBioDB.Models
{
    public class Ticket : IComparable<Ticket>
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [ForeignKey("SeatId")]
        public virtual Seat Seat { get; set; }
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public int CompareTo(Ticket ticket)
        {
            return StartTime.CompareTo(ticket.StartTime);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }

        [ForeignKey("SeatId")]
        public virtual Seat Seat { get; set; }
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}

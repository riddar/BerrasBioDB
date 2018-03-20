using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBioDB.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public virtual Venue Venue { get; set; }
        public virtual IList<Ticket> Tickets { get; set; }
    }
}

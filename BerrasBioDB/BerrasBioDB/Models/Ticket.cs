using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBioDB.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public virtual Seat Seat { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual Customer Customer { get; set; }
    }
}

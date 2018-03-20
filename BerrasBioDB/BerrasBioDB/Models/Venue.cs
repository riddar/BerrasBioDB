using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBioDB.Models
{
    public class Venue
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public int MaxSeats { get; set; }
        public virtual IList<Seat> Seats { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBio.Models
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [ForeignKey("VenueId")]
        public virtual Venue Venue { get; set; }
        public virtual IList<Ticket> Tickets { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POEPart1_VenueBookingSystem.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public required string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public required string Description { get; set; }

        public int VenueId { get; set; }

        //[ForeignKey("VenueId")]
        public Venue? Venue { get; set; }

    }
}

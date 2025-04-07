using System.ComponentModel.DataAnnotations.Schema;

namespace POEPart1_VenueBookingSystem.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int EventId { get; set; }
        public Event? Events { get; set; }

        public int VenueId { get; set; }
        public Venue? Venues { get; set; }

        public DateTime BookingDate { get; set; }
    }
}

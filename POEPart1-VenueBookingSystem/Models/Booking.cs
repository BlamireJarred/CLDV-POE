using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POEPart1_VenueBookingSystem.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Display(Name = "Event")]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        [Display(Name = "Venue")]
        public int VenueId { get; set; }

        [ForeignKey("VenueId")]
        public Venue? Venue { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; }

    }
}

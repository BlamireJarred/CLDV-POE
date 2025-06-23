using System;
using System.ComponentModel.DataAnnotations;

namespace POEPart1_VenueBookingSystem.Models
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }

        [Display(Name = "Event")]
        public string EventName { get; set; }

        [Display(Name = "Venue")]
        public string VenueName { get; set; }

        [Display(Name = "Venue Location")]
        public string VenueLocation { get; set; }

        [Display(Name = "Booking Date")]
        [DataType(DataType.DateTime)]
        public DateTime BookingDate { get; set; }

        [Display(Name = "Event Date")]
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; }

        public string EventType { get; set; }
        public bool IsVenueAvailable { get; set; }  // Set manually in controller logic
    }
}
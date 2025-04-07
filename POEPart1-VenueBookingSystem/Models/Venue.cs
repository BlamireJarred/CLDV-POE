using System.ComponentModel.DataAnnotations;

namespace POEPart1_VenueBookingSystem.Models
{
    public class Venue
    {
        public int VenueId { get; set; }
        //[StringLength(50)]
        public required string VenueName { get; set; }
        //[StringLength(50)]
        public required string Location { get; set; }
        //[Range(1, 100000)]
        public int Capacity { get; set; }
        //[StringLength(50)]
        public required string ImageUrl { get; set; } = "https://via.placeholder.com/150"; // Sample placeholder image

        //public ICollection<Event> Events { get; set; }
        //public ICollection<Booking> Bookings { get; set; }
        //public List<Venue> VenueData { get; set; } = new();
    }
}

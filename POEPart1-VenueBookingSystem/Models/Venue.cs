using System.ComponentModel.DataAnnotations;

namespace POEPart1_VenueBookingSystem.Models
{
    public class Venue
    {
        [Key]
        public int VenueId { get; set; }

        [StringLength(100)]
        public required string VenueName { get; set; }

        [StringLength(200)]
        public required string Location { get; set; }
        public int Capacity { get; set; }
        public required string ImageUrl { get; set; }
        public ICollection<Event>? Event { get; set; }
    }
}

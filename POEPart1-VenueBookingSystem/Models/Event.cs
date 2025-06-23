using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POEPart1_VenueBookingSystem.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [StringLength(100)]
        public required string EventName { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Event Date")]
        [UniqueVenueBooking]
        public DateTime EventDate { get; set; }

        [StringLength(500)]
        public required string Description { get; set; }

        [Display(Name = "Venue")]
        public int VenueId { get; set; }

        [ForeignKey("VenueId")]
        public Venue? Venue { get; set; }

        [Required]
        [StringLength(50)]
        public string EventType { get; set; } = "General"; // Default value



        public ICollection<Booking>? Booking { get; set; }

        public static ValidationResult ValidateEventDate(DateTime date, ValidationContext context)
        {
            var dbContext = (AppDBContext)context.GetService(typeof(AppDBContext));
            var instance = (Event)context.ObjectInstance;

            var conflictingEvent = dbContext.Event
                .FirstOrDefault(e => e.VenueId == instance.VenueId &&
                                   e.EventDate.Date == date.Date &&
                                   e.EventId != instance.EventId);

            if (conflictingEvent != null)
            {
                return new ValidationResult("This venue is already booked for the selected date");
            }

            return ValidationResult.Success;
        }

        public class UniqueVenueBookingAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var dbContext = (AppDBContext)validationContext.GetService(typeof(AppDBContext));
                var currentEvent = (Event)validationContext.ObjectInstance;

                // Check if another event exists at the same venue and time
                var conflictingEvent = dbContext.Event
                    .Any(e => e.VenueId == currentEvent.VenueId &&
                             e.EventDate == currentEvent.EventDate &&
                             e.EventId != currentEvent.EventId); // Exclude current event for edits

                return conflictingEvent
                    ? new ValidationResult("This venue is already booked at the selected time")
                    : ValidationResult.Success;
            }
        }
    }
}

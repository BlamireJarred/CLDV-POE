using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POEPart1_VenueBookingSystem.Models;

namespace POEPart1_VenueBookingSystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly AppDBContext _context;

        private List<string> EventTypes = new List<string> { "General", "Conference", "Wedding", "Concert", "Workshop" };

        public BookingsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index(
            string searchString, 
            string eventType, 
            DateTime? startDate, 
            DateTime? endDate, 
            bool? isVenueAvailable, 
            int? pageNumber)
        {
            int pageSize = 10; // Set your desired page size

            var bookingsQuery = _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .Select(b => new BookingViewModel
                {
                    BookingId = b.BookingId,
                    EventName = b.Event.EventName,
                    EventType = b.Event.EventType,
                    VenueName = b.Venue.VenueName,
                    VenueLocation = b.Venue.Location,
                    BookingDate = b.BookingDate,
                    EventDate = b.Event.EventDate,

                    IsVenueAvailable = !_context.Event.Any(e =>
                        e.VenueId == b.Venue.VenueId &&
                        e.EventDate.Date == b.Event.EventDate.Date &&
                        e.EventId != b.Event.EventId)
                });

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                bookingsQuery = bookingsQuery.Where(b =>
                    b.BookingId.ToString().Contains(searchString) ||
                    b.EventName.ToLower().Contains(searchString));
            }

            if (!string.IsNullOrEmpty(eventType))
            {
                bookingsQuery = bookingsQuery.Where(b => b.EventType == eventType);
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b =>
                    b.EventDate >= startDate && b.EventDate <= endDate);
            }

            if (isVenueAvailable.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.IsVenueAvailable == isVenueAvailable.Value);
            }

            ViewBag.EventTypes = new SelectList(EventTypes);

            return View(await PaginatedList<BookingViewModel>.CreateAsync(
                bookingsQuery.AsNoTracking(),
                pageNumber ?? 1,
                pageSize));
        }

        /* old filter 
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            int pageSize = 10; // Set your desired page size

            var bookingsQuery = _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .Select(b => new BookingViewModel
                {
                    BookingId = b.BookingId,
                    EventName = b.Event.EventName,
                    VenueName = b.Venue.VenueName,
                    VenueLocation = b.Venue.Location,
                    BookingDate = b.BookingDate,
                    EventDate = b.Event.EventDate
                });

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                bookingsQuery = bookingsQuery.Where(b =>
                    b.BookingId.ToString().Contains(searchString) ||
                    b.EventName.ToLower().Contains(searchString));
            }

            return View(await PaginatedList<BookingViewModel>.CreateAsync(
                bookingsQuery.AsNoTracking(),
                pageNumber ?? 1, // If null, default to page 1
                pageSize));
        }
        */
        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Event)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            // Get events with their venues to ensure valid combinations
            var eventsWithVenues = _context.Event
                .Include(e => e.Venue)
                .Where(e => e.EventDate > DateTime.Now) // Only future events
                .ToList();

            ViewData["EventId"] = new SelectList(eventsWithVenues, "EventId", "EventName");
            // Use Distinct to avoid duplicate venues
            ViewData["VenueId"] = new SelectList(
                eventsWithVenues.Select(e => e.Venue).Distinct(),
                "VenueId",
                "VenueName");
            ViewBag.EventTypes = new SelectList(EventTypes);

            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,EventId,VenueId,BookingDate")] Booking booking)
        {
            // Validate event-venue combination
            var selectedEvent = await _context.Event
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(e => e.EventId == booking.EventId);

            if (selectedEvent == null || selectedEvent.VenueId != booking.VenueId)
            {
                ModelState.AddModelError("", "Invalid event-venue combination");
            }

            if (ModelState.IsValid)
            {
                booking.BookingDate = DateTime.Now;
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns if validation fails
            var eventsWithVenues = _context.Event
                .Include(e => e.Venue)
                //.Where(e => e.BookingDate > DateTime.Now)
                .ToList();

            ViewData["EventId"] = new SelectList(eventsWithVenues, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(
                eventsWithVenues.Select(e => e.Venue).Distinct(),
                "VenueId",
                "VenueName",
                booking.VenueId);

            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Event)
                .ThenInclude(e => e.Venue)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            var validEvents = await _context.Event
                .Include(e => e.Venue)
                //.Where(e => e.BookingDate > DateTime.Now)
                .ToListAsync();

            ViewData["EventId"] = new SelectList(validEvents, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(
                validEvents.Select(e => e.Venue).Distinct(),
                "VenueId",
                "VenueName",
                booking.VenueId);
            ViewBag.EventTypes = new SelectList(EventTypes);

            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,EventId,VenueId,BookingDate")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            // Validate event-venue combination
            var selectedEvent = await _context.Event
                .FirstOrDefaultAsync(e => e.EventId == booking.EventId);

            if (selectedEvent == null || selectedEvent.VenueId != booking.VenueId)
            {
                ModelState.AddModelError("", "Invalid event-venue combination");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Preserve original booking date
                    var existingBooking = await _context.Booking.AsNoTracking()
                        .FirstOrDefaultAsync(b => b.BookingId == id);

                    if (existingBooking != null)
                    {
                        booking.BookingDate = existingBooking.BookingDate;
                    }

                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns if validation fails
            var validEvents = await _context.Event
                .Include(e => e.Venue)
                //.Where(e => e.BookingDate > DateTime.Now)
                .ToListAsync();

            ViewData["EventId"] = new SelectList(validEvents, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(
                validEvents.Select(e => e.Venue).Distinct(),
                "VenueId",
                "VenueName",
                booking.VenueId);

            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Event)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }
    }
}
    



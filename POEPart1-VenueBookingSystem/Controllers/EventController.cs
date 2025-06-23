using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POEPart1_VenueBookingSystem.Models;

namespace POEPart1_VenueBookingSystem.Controllers
{
    public class EventsController : Controller
    {
        private readonly AppDBContext _context;

        private List<string> EventTypes = new List<string> { "General", "Conference", "Wedding", "Concert", "Workshop" };

        public EventsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var events = await _context.Event.Include(e => e.Venue).ToListAsync();
            return View(events);
        }


        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewBag.VenueId = new SelectList(_context.Venue, "VenueId","VenueName");
            ViewBag.EventTypes = new SelectList(EventTypes);
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,EventName,EventDate,Description,VenueId,EventType")] Event @event)
        {
            var isVenueAvailable = !_context.Event
                .Any(e => e.VenueId == @event.VenueId &&
                    e.EventDate == @event.EventDate);

            if (!isVenueAvailable)
            {
                ModelState.AddModelError("Date", "This venue is already booked at the selected time");
            }

            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var venues = _context.Venue.ToList();
            ViewData["Venue"] = venues.Any()
                ? new SelectList(venues, "VenueId", "VenueName", @event.VenueId)
                : new SelectList(new List<Venue>(), "VenueId", "VenueName");
            ViewBag.EventTypes = new SelectList(EventTypes, @event.EventType);

            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(e => e.Venue)  // Important for dropdown
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (@event == null)
            {
                return NotFound();
            }

            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", @event.VenueId);
            ViewBag.EventTypes = new SelectList(EventTypes);
            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,EventDate,Description,VenueId,EventType")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
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
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", @event.VenueId);
            ViewBag.EventTypes = new SelectList(EventTypes, @event.EventType);

            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Event.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            // Check for associated bookings
            var hasBookings = await _context.Booking.AnyAsync(b => b.EventId == id);

            if (hasBookings)
            {
                TempData["ErrorMessage"] = "Cannot delete event with active bookings";
                return RedirectToAction(nameof(Index));
            }

            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id);

        }
    }
}
    

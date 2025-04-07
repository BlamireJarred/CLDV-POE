using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POEPart1_VenueBookingSystem.Models;

namespace POEPart1_VenueBookingSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDBContext _context;

        public BookingController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var book = await _context.Booking
                .Include(i => i.Venues)
                .Include(i => i.Events)
                .ToListAsync();

            return View(book);
        }

        public IActionResult Create()
        {
            ViewBag.Events = _context.Event.ToList();//foreign key
            ViewBag.Venues= _context.Venue.ToList();//foreign key
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Booking book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Events = _context.Event.ToList();//foreign key
            ViewBag.Venues = _context.Venue.ToList();//foreign key
            return View(book);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Booking
                .Include(b => b.Events)
                .Include(b => b.Venues)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var book = await _context.Booking.FirstOrDefaultAsync(m => m.BookingId == id);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Booking.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Events = _context.Event.ToList();//foreign key
            ViewBag.Venues = _context.Venue.ToList();//foreign key
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Booking book)
        {
            if (id != book.BookingId)
            {
                return NotFound();
            }

;           if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewBag.EventId = new SelectList(_context.Event.ToList(), "EventId", "EventName");
            ViewBag.VenueId = new SelectList(_context.Venue.ToList(), "VenueId", "VenueName");
            return View(book);
        }
    }   /**/
}
    



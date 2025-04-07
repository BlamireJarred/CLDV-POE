using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POEPart1_VenueBookingSystem.Models;

namespace POEPart1_VenueBookingSystem.Controllers
{
    public class EventController : Controller
    {

        private readonly AppDBContext _context;
        public EventController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()

        {
            var events = await _context.Event
                .Include(i=> i.Venue)
                .ToListAsync();
            return View(events);
        }
        
        public IActionResult Create()
        {
            ViewBag.Venue = _context.Venue.ToList();//foreign key
            return View();
        }

        [HttpPost]
         public async Task<IActionResult> Create(Event events)
         {
             if (ModelState.IsValid)
             {
                _context.Add(events);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
             }
             ViewBag.Venue = _context.Venue.ToList();//foreign key
             return View(events);
         }

        public async Task<IActionResult> Details(int? id)
        {
            var events = await _context.Event.FirstOrDefaultAsync(m => m.EventId == id);

            if (events == null)
            {
                return NotFound();
            }
            return View(events);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var events = await _context.Event.FirstOrDefaultAsync(m => m.EventId == id); 

            if (events == null)
            {
                return NotFound();
            }
            return View(events);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var events = await _context.Event.FindAsync(id);
            _context.Event.Remove(events);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Event.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Venue = _context.Venue.ToList();//foreign key
            return View(events);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Event events)
        {
            if (id != events.EventId)
            {
                return NotFound();
            }

;           if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(events);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(events.EventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewBag.Venue = _context.Venue.ToList();//foreign key
            return View(events);
        }
    }   /**/
}

    

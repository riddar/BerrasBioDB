using BerrasBioDB.App_Data;
using BerrasBioDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BerrasBioDB.Controllers
{
    public class BerraController : Controller
    {
        private readonly Context _context;

        public BerraController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder)
        {
            var tempContext = await _context.Tickets.Include(t => t.Movie).Include(t => t.Seat).Include(t => t.Customer).Include(t => t.Seat.Venue).ToListAsync();

            ViewBag.StartTime = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.MovieTitle = sortOrder == "Title" ? "Title desc" : "Title";
            ViewBag.SeatName = sortOrder == "Seat" ? "Seat desc" : "Seat";
            ViewBag.VenueName = sortOrder == "Venue" ? "Venue desc" : "Venue";
            ViewBag.CustomerName = sortOrder == "Customer" ? "Customer desc" : "Customer";

            switch (sortOrder)
            {
                case "Date":
                    tempContext = tempContext.OrderBy(t => t.StartTime).ToList();
                    break;
                case "date_desc":
                    tempContext = tempContext.OrderByDescending(t => t.StartTime).ToList();
                    break;
                case "Title":
                    tempContext = tempContext.OrderBy(t => t.Movie.Title).ToList();
                    break;
                case "Title_desc":
                    tempContext = tempContext.OrderByDescending(t => t.Movie.Title).ToList();
                    break;
                case "Seat":
                    tempContext = tempContext.OrderBy(t => t.Seat.Name).ToList();
                    break;
                case "Seat_Desc":
                    tempContext = tempContext.OrderByDescending(t => t.Seat.Name).ToList();
                    break;
                case "Venue":
                    tempContext = tempContext.OrderBy(t => t.Seat.Venue.Name).ToList();
                    break;
                case "Venue_Desc":
                    tempContext = tempContext.OrderByDescending(t => t.Seat.Venue.Name).ToList();
                    break;
                case "Customer":
                    tempContext = tempContext.OrderBy(t => t.Customer.Name).ToList();
                    break;
                case "Customer_desc":
                    tempContext = tempContext.OrderByDescending(t => t.Customer.Name).ToList();
                    break;
                default:
                    break;
            }

            
            return View(tempContext);
        }

        [HttpGet]
        public async Task<IActionResult> Confirmed(Ticket _ticket)
        {
            if (_ticket == null)
                return NotFound();

            var ticket = await _context.Tickets.Include(t => t.Movie).Include(t => t.Seat).Include(t => t.Customer).SingleOrDefaultAsync(m => m.Id == _ticket.Id);
            if (ticket == null)
                return NotFound();

            return View(ticket);
        }

        [HttpGet]
        public async Task<IActionResult> TooMany(Ticket _ticket)
        {
            if (_ticket == null)
                return NotFound();

            var ticket = await _context.Tickets.Include(t => t.Movie).Include(t => t.Seat).Include(t => t.Customer).SingleOrDefaultAsync(m => m.Id == _ticket.Id);
            if (ticket == null)
                return NotFound();

            return View(ticket);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Ticket _ticket)
        {
            if (_ticket == null)
                return NotFound();

            var ticket = await _context.Tickets.Include(t => t.Movie).Include(t => t.Seat).Include(t => t.Customer).SingleOrDefaultAsync(m => m.Id == _ticket.Id);
            if (ticket == null)
                return NotFound();

            return View(ticket);
        }

        [HttpGet]
        public async Task<IActionResult> Booking()
        {
            ViewBag.Venues = await _context.Venues.ToListAsync();
            return View();
        }

        [HttpPost, ActionName("Booking")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Booking(Ticket _ticket)
        {
            if (_ticket == null)
                return NotFound();

            var tickets = await _context.Tickets.Include(t => t.Movie).Include(t => t.Seat).Include(t => t.Customer).ToListAsync();

            if (!tickets.Exists(x => x.Seat == _ticket.Seat && x.StartTime == _ticket.StartTime))
            {
                var ticket = new Ticket { StartTime = _ticket.StartTime, Movie = null, Seat = null, Customer = null };
                ticket.Seat = await _context.Seats.Where(s => s.Id == _ticket.Seat.Id).FirstOrDefaultAsync();
                ticket.Movie = await _context.Movies.Where(m => m.Id == _ticket.Movie.Id).FirstOrDefaultAsync();

                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
                return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> BookMultiple()
        {
            var tempContext = await _context.Tickets.Include(t => t.Movie).Include(t => t.Seat).Include(t => t.Customer).ToListAsync();
            return View(tempContext);
        }

        [HttpPost, ActionName("BookMultiple")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookMultipleConfirmed(IEnumerable<Ticket> _tickets)
        {
            if (_tickets == null)
                return NotFound();

            var tickets = await _context.Tickets.Include(t => t.Movie).Include(t => t.Seat).Include(t => t.Customer).ToListAsync();

            foreach (var _ticket in _tickets)
            {
                if (!tickets.Exists(x => x.Seat == _ticket.Seat && x.StartTime == _ticket.StartTime))
                {
                    var ticket = new Ticket { StartTime = _ticket.StartTime, Movie = null, Seat = null, Customer = null };
                    ticket.Seat = await _context.Seats.Where(s => s.Id == _ticket.Seat.Id).FirstOrDefaultAsync();
                    ticket.Movie = await _context.Movies.Where(m => m.Id == _ticket.Movie.Id).FirstOrDefaultAsync();

                    _context.Tickets.Add(ticket);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                    return NotFound();
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Ticket _ticket)
        {
            if (_ticket == null)
                return NotFound();

            var ticket = await _context.Tickets.Include(t => t.Movie).Include(t => t.Seat).Include(t => t.Customer).SingleOrDefaultAsync(m => m.Id == _ticket.Id);

            if(ticket == null)
                return NotFound();

            return View(ticket);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id, Ticket _ticket)
        {
            if(_ticket != null)
            {
                try
                {
                    _ticket.Seat = await _context.Seats.Where(s => s.Id == _ticket.Seat.Id).FirstOrDefaultAsync();
                    _ticket.Movie = await _context.Movies.Where(m => m.Id == _ticket.Movie.Id).FirstOrDefaultAsync();
                    _ticket.Customer = await _context.Customers.Where(c => c.Id == _ticket.Customer.Id).FirstOrDefaultAsync();
                    _context.Update(_ticket);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(_ticket);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Ticket _ticket)
        {
            if (_ticket == null)
                return NotFound();

            var ticket = await _context.Tickets.Include(t => t.Movie).Include(t => t.Seat).Include(t => t.Customer).SingleOrDefaultAsync(m => m.Id == _ticket.Id);

            if (ticket == null)
                return NotFound();

            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Ticket _ticket)
        {
            var ticket = await _context.Tickets.Include(t => t.Movie).Include(t => t.Seat).Include(t => t.Customer).SingleOrDefaultAsync(m => m.Id == _ticket.Id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AddCustomer(Ticket _ticket)
        {
            if (_ticket == null)
                return NotFound();

            var ticket = await _context.Tickets.Include(t => t.Movie).Include(t => t.Seat).Include(t => t.Customer).SingleOrDefaultAsync(m => m.Id == _ticket.Id);

            if (ticket == null)
                return NotFound();

            return View(ticket);
        }

        [HttpPost, ActionName("AddCustomer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCustomerConfirmed(int id, Ticket _ticket)
        {
            if (_ticket != null)
            {
                try
                {
                    var tickets = await _context.Tickets.Where(t => t.Customer.Id == _ticket.Customer.Id).ToListAsync();

                    if (tickets.Count >= 12)
                        return View("TooMany", _ticket);

                    _ticket.Customer = await _context.Customers.Where(c => c.Id == _ticket.Customer.Id).FirstOrDefaultAsync();
                    _context.Update(_ticket);
                    await _context.SaveChangesAsync();
                    return View("Confirmed", _ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(_ticket);
        }

        [HttpGet]
        public IActionResult AddVenue()
        {
            return View();
        }

        [HttpPost, ActionName("AddVenue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVenue(Venue _venue)
        {
            if (_venue == null)
                return NotFound();

            var venues = await _context.Venues.Include(v => v.Seats).ToListAsync();

            if (!venues.Exists(v => v.Name == _venue.Name))
            {
                await _context.Venues.AddAsync(_venue);
                for(int i=1; i<=_venue.MaxSeats; i++)
                {
                    var seat = new Seat { Name = i.ToString(), Venue = _venue };
                    await _context.Seats.AddAsync(seat);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
                return NotFound();
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }

    }
}

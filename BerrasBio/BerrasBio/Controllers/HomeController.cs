using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BerrasBio.Models;
using BerrasBio.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BerrasBio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(ApplicationDbContext _context, UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }

        [HttpGet]
        public IActionResult Index(string sortOrder)
        {
            ViewBag.Id = sortOrder == "Id" ? "Id_desc" : "Id";
            ViewBag.StartTime = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.MovieTitle = sortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.SeatName = sortOrder == "Seat" ? "Seat_desc" : "Seat";
            ViewBag.VenueName = sortOrder == "Venue" ? "Venue_desc" : "Venue";
            ViewBag.User = sortOrder == "User" ? "User_desc" : "User";

            var tickets = context.Tickets
                .Include(t => t.User)
                .Include(t => t.Movie)
                .Include(t => t.Seat)
                .ThenInclude(s => s.Venue).ToList();

            switch (sortOrder)
            {
                case "Id":
                    tickets = tickets.OrderBy(t => t.Id).ToList();
                    break;
                case "Id_desc":
                    tickets = tickets.OrderByDescending(t => t.Id).ToList();
                    break;
                case "Date":
                    tickets = tickets.OrderBy(t => t.StartTime).ToList();
                    break;
                case "date_desc":
                    tickets = tickets.OrderByDescending(t => t.StartTime).ToList();
                    break;
                case "Title":
                    tickets = tickets.OrderBy(t => t.Movie.Name).ToList();
                    break;
                case "Title_desc":
                    tickets = tickets.OrderByDescending(t => t.Movie.Name).ToList();
                    break;
                case "Seat":
                    tickets = tickets.OrderBy(t => t.Seat.Name).ToList();
                    break;
                case "Seat_Desc":
                    tickets = tickets.OrderByDescending(t => t.Seat.Name).ToList();
                    break;
                case "Venue":
                    tickets = tickets.OrderBy(t => t.Seat.Venue.Name).ToList();
                    break;
                case "Venue_Desc":
                    tickets = tickets.OrderByDescending(t => t.Seat.Venue.Name).ToList();
                    break;
                case "Customer":
                    tickets = tickets.OrderBy(t => t.User.UserName).ToList();
                    break;
                case "Customer_desc":
                    tickets = tickets.OrderByDescending(t => t.User.UserName).ToList();
                    break;
                default:
                    break;
            }

            return View(tickets);
        }

        [HttpGet]
        public async Task<IActionResult> BuyTicket(int Id)
        {
            var ticket = await context.Tickets
                 .Include(t => t.User)
                 .Include(t => t.Movie)
                 .Include(t => t.Seat)
                 .ThenInclude(s => s.Venue)
                 .Where(t => t.Id == Id)
                 .FirstOrDefaultAsync();

            return View(ticket);
        }

        [HttpPost, ActionName("BuyTicket")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyTicketPost(int? id)
        {
            if (id == null)
                return NotFound();

            var ticket = await context.Tickets
                .Include(t => t.User)
                .Include(t => t.Movie)
                .Include(t => t.Seat)
                .ThenInclude(s => s.Venue)
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            var user = await userManager.GetUserAsync(HttpContext.User);

            ticket.User = user;

            user = await userManager.GetUserAsync(HttpContext.User);

            var tickets = await context.Tickets
                .Include(t => t.Movie)
                .Include(t => t.Seat)
                .Include(t => t.User)
                .Where(t => t.User == user)
                .ToListAsync();

            if (tickets.Count() >= 12)
                return RedirectToAction(nameof(TooMany), ticket);

            context.Update(ticket);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AddTicket(int? id)
        {
            if (id == null)
                return View();

            var ticket = await context.Tickets
                    .Include(t => t.User)
                    .Include(t => t.Movie)
                    .Include(t => t.Seat)
                    .Where(t => t.Id == id)
                    .FirstOrDefaultAsync();

            var user = await userManager.GetUserAsync(HttpContext.User);

            ticket.User = user;

            var tickets = await context.Tickets
                .Include(t => t.Movie)
                .Include(t => t.Seat)
                .ThenInclude(s => s.Venue)
                .Include(t => t.User)
                .Where(t => t.User == user)
                .ToListAsync();

            if (tickets.Count() >= 12)
                return RedirectToAction(nameof(TooMany), ticket);

            var tempTickets = await context.Tickets
                .Include(t => t.Movie)
                .Include(t => t.Seat)
                .ThenInclude(s => s.Venue)
                .Include(t => t.User)
                .Where(t => t.Movie == ticket.Movie)
                .ToListAsync();

            context.Update(ticket);
            await context.SaveChangesAsync();

            return View("MovieIndex", tempTickets);
            //return RedirectToAction("MovieIndex", tickets);
        }

        [HttpGet]
        public async Task<IActionResult> Confirmed(Ticket ticket)
        {
            if (ticket == null)
                return NotFound();

            ticket = await context.Tickets
                .Include(t => t.User)
                .Include(t => t.Movie)
                .Include(t => t.Seat)
                .ThenInclude(s => s.Venue)
                .Where(t => t.Id == ticket.Id)
                .FirstOrDefaultAsync();

            return View(ticket);
        }

        [HttpGet]
        public async Task<IActionResult> TooMany(Ticket ticket)
        {
            if (ticket == null)
                return NotFound();

            ticket = await context.Tickets
                .Include(t => t.User)
                .Include(t => t.Movie)
                .Include(t => t.Seat)
                .ThenInclude(s => s.Venue)
                .Where(t => t.Id == ticket.Id)
                .FirstOrDefaultAsync();

            return View(ticket);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var ticket = await context.Tickets
                .Include(t => t.User)
                .Include(t => t.Movie)
                .Include(t => t.Seat)
                .ThenInclude(s => s.Venue)
                .Where(t => t.Id == Id)
                .FirstOrDefaultAsync();

            return View(ticket);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int? Id)
        {
            if (Id == null)
                return NotFound();

            var ticket = await context.Tickets
                .Include(t => t.User)
                .Include(t => t.Movie)
                .Include(t => t.Seat)
                .ThenInclude(s => s.Venue)
                .Where(t => t.Id == Id)
                .FirstOrDefaultAsync();

            ticket.User = null;
            context.Update(ticket);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Movies()
        {
            var movies = context.Movies.Include(m => m.Tickets).ToList();
            var tickets = context.Tickets.Include(t => t.Movie).Include(t => t.User).Include(t => t.Seat).ToList();
            return View(movies);
        }

        [HttpGet]
        public IActionResult MovieIndex(Movie movie)
        {
            if(movie == null)
                return NotFound();

            var tickets = context.Tickets
                .Include(t => t.User)
                .Include(t => t.Movie)
                .Include(t => t.Seat)
                .ThenInclude(s => s.Venue)
                .Where(t => t.Movie == movie)
                .ToList();

            return View(tickets);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

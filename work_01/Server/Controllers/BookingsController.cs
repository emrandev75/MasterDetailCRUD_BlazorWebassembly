using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using work_01.Shared.DTO;
using work_01.Shared.Models;

namespace work_01.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly TourDbContext _context;

        public BookingsController(TourDbContext context)
        {
            _context = context;
        }

      
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            if (_context.Bookings == null)
            {
                return NotFound();
            }
            return await _context.Bookings.ToListAsync();
        }
        [HttpGet("DTO")]
        public async Task<ActionResult<IEnumerable<BookingViewDTO>>> GetBookingDTOs()
        {
            if (_context.Bookings == null)
            {
                return NotFound();
            }
            return await _context.Bookings
                .Include(o => o.Customer)
                .Include(o => o.BookingItems).ThenInclude(oi => oi.Spot)
                .Select(o =>
                    new BookingViewDTO
                    {
                        BookingID = o.BookingID,
                        StartDate = o.StartDate,
                        EndDate = o.EndDate,
                        CustomerName = o.Customer.CustomerName,
                        Status = o.Status,
                        ItemCount = o.BookingItems.Count,
                        BookingValue = o.BookingItems.Sum(x => x.Spot.Price * x.Travelers)

                    })
                .ToListAsync();
        }
     
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            if (_context.Bookings == null)
            {
                return NotFound();
            }
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }
        [HttpGet("DTO/{id}")]
        public async Task<ActionResult<BookingViewDTO>> GetBookingViewDTO(int id)
        {
            if (_context.Bookings == null)
            {
                return NotFound();
            }
            var booking = await _context.Bookings.Include(o => o.Customer)
                .Include(o => o.BookingItems).ThenInclude(oi => oi.Spot).FirstOrDefaultAsync(o => o.BookingID == id);

            if (booking == null)
            {
                return NotFound();
            }

            return new BookingViewDTO
            {
                BookingID = booking.BookingID,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                CustomerName = booking.Customer.CustomerName,
                Status = booking.Status,
                ItemCount = booking.BookingItems.Count,
                BookingValue = booking.BookingItems.Sum(x => x.Spot.Price * x.Travelers)

            };
        }
        [HttpGet("Items/{id}")]
        public async Task<ActionResult<IEnumerable<BookingItemViewDTO>>> GetBookingItems(int id)
        {
            if (_context.BookingItems == null)
            {
                return NotFound();
            }
            var bookingitems = await _context.BookingItems.Include(x => x.Spot).Where(oi => oi.BookingID == id).ToListAsync();

            if (bookingitems == null)
            {
                return NotFound();
            }

            return bookingitems.Select(oi => new BookingItemViewDTO { BookingID = oi.BookingID, SpotName = oi.Spot.SpotName, Price = oi.Spot.Price, Travelers = oi.Travelers }).ToList();
        }
        [HttpGet("OI/{id}")]
        public async Task<ActionResult<IEnumerable<BookingItem>>> GetBookingItemsOf(int id)
        {
            if (_context.BookingItems == null)
            {
                return NotFound();
            }
            var bookingitems = await _context.BookingItems.Where(oi => oi.BookingID == id).ToListAsync();

            if (bookingitems == null)
            {
                return NotFound();
            }

            return bookingitems;
        }
      
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.BookingID)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpPut("DTO/{id}")]

        public async Task<IActionResult> PutBookingWithBookingItem(int id, BookingEditDTO booking)
        {
            if (id != booking.BookingID)
            {
                return BadRequest();
            }
            var existing = await _context.Bookings.Include(x => x.BookingItems).FirstAsync(o => o.BookingID == id);
            _context.BookingItems.RemoveRange(existing.BookingItems);
            existing.BookingID = booking.BookingID;
            existing.StartDate = booking.StartDate;
            existing.EndDate = booking.EndDate;
            existing.CustomerID = booking.CustomerID;
            existing.Status = booking.Status;
            foreach (var item in booking.BookingItems)
            {
                _context.BookingItems.Add(new BookingItem { BookingID = booking.BookingID, SpotID = item.SpotID, Travelers = item.Travelers });
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException?.Message);

            }

            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'TourDbContext.Bookings'  is null.");
            }
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.BookingID }, booking);
        }

        [HttpPost("DTO")]
        public async Task<ActionResult<Booking>> PostBookingDTO(BookingDTO dto)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'TourDbContext.Bookings'  is null.");
            }
            var booking = new Booking { CustomerID = dto.CustomerID, StartDate = dto.StartDate, EndDate = dto.EndDate, Status = dto.Status };
            foreach (var oi in dto.BookingItems)
            {
                booking.BookingItems.Add(new BookingItem { SpotID = oi.SpotID, Travelers = oi.Travelers });
            }
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return booking;
        }    
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (_context.Bookings == null)
            {
                return NotFound();
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return (_context.Bookings?.Any(e => e.BookingID == id)).GetValueOrDefault();
        }
    }
}

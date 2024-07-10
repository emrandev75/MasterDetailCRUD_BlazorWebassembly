using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using work_01.Shared.Models;

namespace work_01.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingItemsController : ControllerBase
    {
        private readonly TourDbContext _context;

        public BookingItemsController(TourDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingItem>>> GetBookingItems()
        {
            if (_context.BookingItems == null)
            {
                return NotFound();
            }
            return await _context.BookingItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingItem>> GetBookingItem(int id)
        {
            if (_context.BookingItems == null)
            {
                return NotFound();
            }
            var bookingItem = await _context.BookingItems.FindAsync(id);

            if (bookingItem == null)
            {
                return NotFound();
            }

            return bookingItem;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookingItem(int id, BookingItem bookingItem)
        {
            if (id != bookingItem.BookingID)
            {
                return BadRequest();
            }

            _context.Entry(bookingItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingItemExists(id))
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

        [HttpPost]
        public async Task<ActionResult<BookingItem>> PostOrderItem(BookingItem bookingItem)
        {
            if (_context.BookingItems == null)
            {
                return Problem("Entity set 'TourDbContext.BookingItems'  is null.");
            }
            _context.BookingItems.Add(bookingItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookingItemExists(bookingItem.BookingID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBookingItem", new { id = bookingItem.BookingID }, bookingItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookingItem(int id)
        {
            if (_context.BookingItems == null)
            {
                return NotFound();
            }
            var bookingItem = await _context.BookingItems.FindAsync(id);
            if (bookingItem == null)
            {
                return NotFound();
            }

            _context.BookingItems.Remove(bookingItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingItemExists(int id)
        {
            return (_context.BookingItems?.Any(e => e.BookingID == id)).GetValueOrDefault();
        }
    }
}

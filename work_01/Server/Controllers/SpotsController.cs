using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using work_01.Shared.DTO;
using work_01.Shared.Models;

namespace work_01.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotsController : ControllerBase
    {
        private readonly TourDbContext _context;
        private readonly IWebHostEnvironment env;
        public SpotsController(TourDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Spot>>> GetSpots()
        {
            if (_context.Spots == null)
            {
                return NotFound();
            }
            return await _context.Spots.ToListAsync();
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<Spot>> GetSpot(int id)
        {
            if (_context.Spots == null)
            {
                return NotFound();
            }
            var spot = await _context.Spots.FindAsync(id);

            if (spot == null)
            {
                return NotFound();
            }

            return spot;
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpot(int id, Spot spot)
        {
            if (id != spot.SpotID)
            {
                return BadRequest();
            }

            _context.Entry(spot).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpotExists(id))
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
        public async Task<ActionResult<Spot>> PostSpot(Spot spot)
        {
            if (_context.Spots == null)
            {
                return Problem("Entity set 'TourDbContext.Spots'  is null.");
            }
            _context.Spots.Add(spot);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpot", new { id = spot.SpotID }, spot);
        }

        [HttpPost("Upload")]
        public async Task<ImageUploadResponse> Upload(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName);
            var randomFileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var storedFileName = randomFileName + ext;
            using FileStream fs = new FileStream(Path.Combine(env.WebRootPath, "Uploads", storedFileName), FileMode.Create);
            await file.CopyToAsync(fs);
            fs.Close();
            return new ImageUploadResponse { FileName = file.FileName, StoredFileName = storedFileName };
        }
   
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpot(int id)
        {
            if (_context.Spots == null)
            {
                return NotFound();
            }
            var spot = await _context.Spots.FindAsync(id);
            if (_context.BookingItems.Any(x => x.SpotID == id)) return BadRequest("Entity has child data");
            if (spot == null)
            {
                return NotFound();
            }

            _context.Spots.Remove(spot);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SpotExists(int id)
        {
            return (_context.Spots?.Any(e => e.SpotID == id)).GetValueOrDefault();
        }
    }
}

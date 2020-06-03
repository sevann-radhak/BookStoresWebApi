using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoresWebApi.Models;

namespace BookStoresWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly BookStoresDBContext _context;

        public PublishersController(BookStoresDBContext context)
        {
            _context = context;
        }

        // GET: api/Publishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublishers()
        {
            return await _context.Publishers.ToListAsync();
        }

        // GET: api/Publishers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetPublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return NotFound();
            }

            return publisher;
        }

        // GET: api/Publishers/5
        [HttpGet("GetPublisherDetails/{id}")]
        public ActionResult<Publisher> GetPublisherDetails(int id)
        {
            var publisher = _context.Publishers
                .Include(p => p.Books)
                .ThenInclude(b => b.BookAuthors)
                .Include(p => p.Books)
                .ThenInclude(b => b.Sales)
                .Include(p => p.Users)
                .ThenInclude(u => u.Job)
                .Where(p => p.PubId == id)
                .FirstOrDefault();

            if (publisher == null)
            {
                return NotFound();
            }

            return publisher;
        }

        // GET: api/Publishers/5
        [HttpGet("GetPublisherExplicit/{id}")]
        public async Task<ActionResult<Publisher>> GetPublisherExplicit(int id)
        {

            var publisher = await _context.Publishers.SingleAsync(p => p.PubId == id);

            _context.Entry(publisher)
                .Collection(p => p.Users)
                .Query()
                .Where(u => u.FirstName.Contains("Karin"))
                .Load();

            _context.Entry(publisher)
                .Collection(p => p.Books)
                .Query()
                .Include(b => b.Sales)
                .Load();

            if (publisher == null)
            {
                return NotFound();
            }

            return publisher;
        }

        // PUT: api/Publishers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublisher(int id, Publisher publisher)
        {
            if (id != publisher.PubId)
            {
                return BadRequest();
            }

            _context.Entry(publisher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(id))
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

        // POST: api/Publishers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Publisher>> PostPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPublisher", new { id = publisher.PubId }, publisher);
        }

        // DELETE: api/Publishers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Publisher>> DeletePublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return publisher;
        }

        private bool PublisherExists(int id)
        {
            return _context.Publishers.Any(e => e.PubId == id);
        }
    }
}

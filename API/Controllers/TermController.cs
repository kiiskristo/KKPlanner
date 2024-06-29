using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KKPlanner.API.Db;

/*
 * curl -X POST http://localhost:5192/tables/term \
   -H "Content-Type: application/json" \
   -d '{
         "title": "Term 1",
         "startDate": "2023-01-01T00:00:00",
         "endDate": "2023-06-30T00:00:00"
       }'
 */

namespace KKPlanner.API.Controllers
{
    [Route("tables/term")]
    [ApiController]
    public class TermController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TermController(AppDbContext context)
        {
            _context = context;
        }

        // GET: tables/term
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Term>>> GetTerms()
        {
            // Adjusted to include Courses
            return await _context.Terms.Include(t => t.Courses).ToListAsync();
        }

        // GET: tables/term/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Term>> GetTerm(int id)
        {
            // Adjusted to include Courses
            var term = await _context.Terms.Include(t => t.Courses)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (term == null)
            {
                return NotFound();
            }

            return term;
        }

        // POST: tables/term
        [HttpPost]
        public async Task<ActionResult<Term>> PostTerm(Term term)
        {
            _context.Terms.Add(term);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTerm), new { id = term.Id }, term);
        }

        // PUT: tables/term/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTerm(int id, Term term)
        {
            if (id != term.Id)
            {
                return BadRequest();
            }

            _context.Entry(term).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TermExists(id))
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

        // DELETE: tables/term/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTerm(int id)
        {
            var term = await _context.Terms.FindAsync(id);
            if (term == null)
            {
                return NotFound();
            }

            _context.Terms.Remove(term);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TermExists(int id)
        {
            return _context.Terms.Any(e => e.Id == id);
        }
    }
}

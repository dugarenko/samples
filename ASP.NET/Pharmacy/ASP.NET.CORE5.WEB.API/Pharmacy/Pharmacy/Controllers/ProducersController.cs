using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pharmacy.Domain;
using Pharmacy.Domain.Models;
using Pharmacy.Domain.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProducersController : ControllerBase
    {
        private readonly ILogger<MedicamentsController> _logger;
        private readonly ApplicationDbContext _context;

        public ProducersController(ApplicationDbContext context, ILogger<MedicamentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: producers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producer>>> GetAll([FromQuery] QueryProducers filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                return await _context.Producers.Where(x => x.Name == filter.Name).ToListAsync();
            }
            return await _context.Producers.ToListAsync();
        }

        // GET: producers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producer>> Get(int id)
        {
            var producer = await _context.Producers.FindAsync(id);

            if (producer == null)
            {
                return NotFound();
            }

            return producer;
        }

        // PUT: producers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Producer producer)
        {
            if (id != producer.IdProducer)
            {
                return BadRequest();
            }

            _context.Entry(producer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Producers.Any(x => x.IdProducer == id))
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

        // POST: producers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Producer>> Post(Producer producer)
        {
            _context.Producers.Add(producer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = producer.IdProducer }, producer);
        }

        // DELETE: producers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var producer = await _context.Producers.FindAsync(id);
            if (producer == null)
            {
                return NotFound();
            }

            _context.Producers.Remove(producer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

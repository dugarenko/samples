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
    public class MedicamentsController : ControllerBase
    {
        private readonly ILogger<MedicamentsController> _logger;
        private readonly ApplicationDbContext _context;

        public MedicamentsController(ApplicationDbContext context, ILogger<MedicamentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: medicaments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicament>>> GetAll([FromQuery] QueryMedicaments filter)
        {
            if (filter.IdProducer.HasValue && filter.Price.HasValue)
            {
                return await _context.Medicaments.Where(x =>
                        x.IdProducer == filter.IdProducer.Value &&
                        x.Price == filter.Price.Value
                    ).ToListAsync();
            }
            else if (filter.IdProducer.HasValue)
            {
                return await _context.Medicaments.Where(x =>
                        x.IdProducer == filter.IdProducer.Value
                    ).ToListAsync();
            }
            else if (filter.Price.HasValue)
            {
                return await _context.Medicaments.Where(x =>
                        x.Price == filter.Price.Value
                    ).ToListAsync();
            }
            return await _context.Medicaments.ToListAsync();
        }

        // GET: medicaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Medicament>> Get(int id)
        {
            var medicament = await _context.Medicaments.FindAsync(id);

            if (medicament == null)
            {
                return NotFound();
            }

            return medicament;
        }

        // PUT: medicaments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Medicament medicament)
        {
            if (id != medicament.IdMedicament)
            {
                return BadRequest();
            }

            _context.Entry(medicament).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Medicaments.Any(x => x.IdMedicament == id))
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

        // POST: medicaments
        [HttpPost]
        public async Task<ActionResult<Medicament>> Post(Medicament medicament)
        {
            _context.Medicaments.Add(medicament);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = medicament.IdMedicament }, medicament);
        }

        // DELETE: medicaments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Medicament>> Delete(int id)
        {
            var medicament = await _context.Medicaments.FindAsync(id);
            if (medicament == null)
            {
                return NotFound();
            }

            _context.Medicaments.Remove(medicament);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

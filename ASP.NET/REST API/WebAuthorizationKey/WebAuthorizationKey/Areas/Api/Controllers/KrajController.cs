using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAuthorizationKey.Areas.Api.Data;
using WebAuthorizationKey.Areas.Api.Models;
using WebAuthorizationKey.Areas.Attributes;

namespace WebAuthorizationKey.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [AuthorizationKey]
    public class KrajController : ControllerBase
    {
        private readonly ILogger<KrajController> _logger;
        private ApplicationDbContext _dbContext;

        public KrajController(ILogger<KrajController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // GET: api/<KrajController>
        /// <summary>
        /// Zwraca wskazaną ilość elementów.
        /// </summary>
        /// <param name="offset">Liczba elementów do pominięcia przed zwróceniem pozostałych elementów.</param>
        /// <param name="limit">Liczba elementów do zwrócenia.</param>
        /// <response code="200">Jeśli żądanie pobrania elementów wykonało się poprawnie.</response>
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Kraj>>> Get(int offset, int limit)
        {
            if (limit < 1)
            {
                limit = 10;
            }
            if (offset < 0)
            {
                offset = 0;
            }
            return await _dbContext.Krajs.Skip(offset).Take(limit).ToListAsync();
        }

        // GET: api/<KrajController>/5
        /// <summary>
        /// Zwraca element na podstawie wskazanego identyfikatora.
        /// </summary>
        /// <param name="id">Identyfikator elementu do zwrócenia.</param>
        /// <response code="200">Jeśli żądanie pobrania elementów wykonało się poprawnie.</response>
        /// <response code="404">Jeśli element o wskazanym identyfikatorze nie został znaleziony.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Kraj>> Get(int id)
        {
            var kraj = await _dbContext.Krajs.FindAsync(id);
            if (kraj == null)
            {
                return NotFound();
            }
            return kraj;
        }

        // POST: api/<KrajController>
        /// <summary>
        /// Tworzy nowy element.
        /// </summary>
        /// <param name="value">Dane do utworzenia nowego elementu.</param>
        /// <response code="201">Jeśli zwrócony zostanie nowoutworzony element.</response>
        /// <response code="400">Jeśli właściwość 'Nazwa' przyjmuje wartość null lub pustą.</response>
        [HttpPost(Name = "Create")]
        public async Task<ActionResult<Kraj>> Post([FromBody] Kraj value)
        {
            if (string.IsNullOrEmpty(value.Nazwa))
            {
                return BadRequest();
            }

            _dbContext.Krajs.Add(value);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = value.Id, value });
        }

        // PUT: api/<KrajController>/5
        /// <summary>
        /// Aktualizuje podany element na podstawie wskazanego identyfikatora.
        /// </summary>
        /// <param name="id">Identyfikator elementu do aktualizacji.</param>
        /// <param name="value">Dane elementu do aktualizacji.</param>
        /// <response code="204">Jeśli aktualizacja podane elementu wykonała się poprawnie.</response>
        /// <response code="400">Jeśli właściwość 'Nazwa' przyjmuje wartość null lub pustą.</response>
        /// <response code="404">Jeśli element o wskazanym identyfikatorze nie został znaleziony.</response>
        [HttpPut("{id}", Name = "Update")]
        public async Task<IActionResult> Put(int id, [FromBody] Kraj value)
        {
            if (string.IsNullOrEmpty(value.Nazwa))
            {
                return BadRequest();
            }

            var kraj = await _dbContext.Krajs.FindAsync(id);
            if (kraj == null)
            {
                return NotFound();
            }

            kraj.Nazwa = value.Nazwa;
            kraj.ISO2 = value.ISO2;
            kraj.ISO3 = value.ISO3;

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/<KrajController>/5
        /// <summary>
        /// Usuwa element na podstawie wskazanego identyfikatora.
        /// </summary>
        /// <param name="id">Identyfikator elementu do usunięcia.</param>
        /// <response code="200">Jeśli element o wskazanym identyfikatorze został poprawnie usunięty.</response>
        /// <response code="400">Jeśli wartość parametru 'id' jest mniejsza od 1.</response>
        /// <response code="404">Jeśli element o wskazanym identyfikatorze nie został znaleziony.</response>
        [HttpDelete("{id}", Name = "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var kraj = await _dbContext.Krajs.FindAsync(id);
            if (kraj == null)
            {
                return NotFound();
            }

            _dbContext.Krajs.Remove(kraj);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}

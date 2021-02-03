using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pharmacy.Domain;
using Pharmacy.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Controllers
{
    [Authorize]
    public class MedicamentsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MedicamentsController> _logger;

        public MedicamentsController(ApplicationDbContext context, ILogger<MedicamentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Medicaments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Medicaments.Include(m => m.IdProducerNavigation);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Medicaments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicament = await _context.Medicaments
                .Include(m => m.IdProducerNavigation)
                .FirstOrDefaultAsync(m => m.IdMedicament == id);
            if (medicament == null)
            {
                return NotFound();
            }

            return View(medicament);
        }

        // GET: Medicaments/Create
        public IActionResult Create()
        {
            ViewData["IdProducer"] = new SelectList(_context.Producers, "IdProducer", "Name");
            return View();
        }

        // POST: Medicaments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMedicament,IdProducer,Name,Price,Description")] Medicament medicament)
        {
            TranslateDecimal(medicament, nameof(Medicament.Price));
            if (ModelState.IsValid)
            {
                _context.Add(medicament);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProducer"] = new SelectList(_context.Producers, "IdProducer", "Name", medicament.IdProducer);
            return View(medicament);
        }

        // GET: Medicaments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicament = await _context.Medicaments.FindAsync(id);
            if (medicament == null)
            {
                return NotFound();
            }
            ViewData["IdProducer"] = new SelectList(_context.Producers, "IdProducer", "Name", medicament.IdProducer);
            return View(medicament);
        }

        // POST: Medicaments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMedicament,IdProducer,Name,Price,Description")] Medicament medicament)
        {
            if (id != medicament.IdMedicament)
            {
                return NotFound();
            }

            TranslateDecimal(medicament, nameof(Medicament.Price));
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicament);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicamentExists(medicament.IdMedicament))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProducer"] = new SelectList(_context.Producers, "IdProducer", "Name", medicament.IdProducer);
            return View(medicament);
        }

        // GET: Medicaments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicament = await _context.Medicaments
                .Include(m => m.IdProducerNavigation)
                .FirstOrDefaultAsync(m => m.IdMedicament == id);
            if (medicament == null)
            {
                return NotFound();
            }

            return View(medicament);
        }

        // POST: Medicaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicament = await _context.Medicaments.FindAsync(id);
            _context.Medicaments.Remove(medicament);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicamentExists(int id)
        {
            return _context.Medicaments.Any(e => e.IdMedicament == id);
        }
    }
}

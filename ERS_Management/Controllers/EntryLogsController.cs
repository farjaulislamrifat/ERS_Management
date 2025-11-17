using ERS_Management.Data;
using ERS_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERS_Management.Controllers
{
    public class EntryLogsController : Controller
    {
        private readonly ERS_ManagementContext _context;

        public EntryLogsController(ERS_ManagementContext context)
        {
            _context = context;
        }

        // GET: EntryLogs
        public async Task<IActionResult> Index()
        {
            return View(await _context.EntryLog.ToListAsync());
        }

        // GET: EntryLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entryLog = await _context.EntryLog
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entryLog == null)
            {
                return NotFound();
            }

            return View(entryLog);
        }

        // GET: EntryLogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EntryLogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EntryTime,EnteredBy,logAction")] EntryLog entryLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entryLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entryLog);
        }

        // GET: EntryLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entryLog = await _context.EntryLog.FindAsync(id);
            if (entryLog == null)
            {
                return NotFound();
            }
            return View(entryLog);
        }

        // POST: EntryLogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EntryTime,EnteredBy,logAction")] EntryLog entryLog)
        {
            if (id != entryLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entryLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntryLogExists(entryLog.Id))
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
            return View(entryLog);
        }

        // GET: EntryLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entryLog = await _context.EntryLog
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entryLog == null)
            {
                return NotFound();
            }

            return View(entryLog);
        }

        // POST: EntryLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entryLog = await _context.EntryLog.FindAsync(id);
            if (entryLog != null)
            {
                _context.EntryLog.Remove(entryLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntryLogExists(int id)
        {
            return _context.EntryLog.Any(e => e.Id == id);
        }
    }
}

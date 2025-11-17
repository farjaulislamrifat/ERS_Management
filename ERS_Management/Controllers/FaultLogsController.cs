using ERS_Management.Data;
using ERS_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERS_Management.Controllers
{
    public class FaultLogsController : Controller
    {
        private readonly ERS_ManagementContext _context;

        public FaultLogsController(ERS_ManagementContext context)
        {
            _context = context;
        }

        // GET: FaultLogs
        public async Task<IActionResult> Index()
        {
            return View(await _context.FaultLog.ToListAsync());
        }

        // GET: FaultLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faultLog = await _context.FaultLog
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faultLog == null)
            {
                return NotFound();
            }

            return View(faultLog);
        }

        // GET: FaultLogs/Create
        public IActionResult Create()
        {
            return View();
        }



        [HttpGet("{faultNo}")]
        public async Task<IActionResult> GetLogs(int faultNo)
        {
            var logs = await _context.FaultLog
                .Where(l => l.FaultId == faultNo)
                .OrderByDescending(l => l.EntryTime)
                .Select(l => new
                {
                    l.EntryTime,
                    l.EnteredBy,
                    l.logAction

                })
                .ToListAsync();

            return Ok(logs);
        }



        // POST: FaultLogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EntryTime,EnteredBy,logAction")] FaultLog faultLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(faultLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faultLog);
        }

        // GET: FaultLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faultLog = await _context.FaultLog.FindAsync(id);
            if (faultLog == null)
            {
                return NotFound();
            }
            return View(faultLog);
        }

        // POST: FaultLogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EntryTime,EnteredBy,logAction")] FaultLog faultLog)
        {
            if (id != faultLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faultLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaultLogExists(faultLog.Id))
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
            return View(faultLog);
        }

        // GET: FaultLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faultLog = await _context.FaultLog
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faultLog == null)
            {
                return NotFound();
            }

            return View(faultLog);
        }

        // POST: FaultLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faultLog = await _context.FaultLog.FindAsync(id);
            if (faultLog != null)
            {
                _context.FaultLog.Remove(faultLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FaultLogExists(int id)
        {
            return _context.FaultLog.Any(e => e.Id == id);
        }
    }
}

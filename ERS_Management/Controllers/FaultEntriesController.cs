using ERS_Management.Data;
using ERS_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ERS_Management.Controllers
{
    public class FaultEntriesController : Controller
    {
        private readonly ERS_ManagementContext _context;

        public FaultEntriesController(ERS_ManagementContext context)
        {
            _context = context;
        }

        // GET: FaultEntries
        public async Task<IActionResult> Index()
        {
            return View(await _context.FaultEntry.ToListAsync());
        }



        public void LoadDropdowns()
        {
            // Example for loading a dropdown list from the database
            ViewBag.Sites = new SelectList(_context.FaultEntry.Select(c => c.Site).Distinct().ToList());
            ViewBag.Locations = new SelectList(_context.FaultEntry.Select(c => c.Location).Distinct().ToList());
            ViewBag.Categories = new SelectList(_context.FaultEntry.Select(c => c.Category).Distinct().ToList());
        }


        // GET: FaultEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faultEntry = await _context.FaultEntry
                .FirstOrDefaultAsync(m => m.No == id);
            if (faultEntry == null)
            {
                return NotFound();
            }

            return View(faultEntry);
        }

        // GET: FaultEntries/Create
        public IActionResult Create()
        {

            LoadDropdowns();
            return View();
        }

        // POST: FaultEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("No,FaultTime,ReportedBy,Site,Location,Description,Category,Resolution,ResolvedBy,ResolvedTime,Duration,Remarks")] FaultEntry faultEntry)
        {
            if (ModelState.IsValid)
            {
                faultEntry.Username = User.Identity.Name;
                _context.Add(faultEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faultEntry);
        }

        // GET: FaultEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faultEntry = await _context.FaultEntry.FindAsync(id);
            if (faultEntry == null)
            {
                return NotFound();
            }




            if (faultEntry == null) return NotFound();

            LoadDropdowns();

            return View(faultEntry);
        }

        // POST: FaultEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("No,FaultTime,ReportedBy,Site,Location,Description,Category,Resolution,ResolvedBy,ResolvedTime,Duration,Remarks,Username")] FaultEntry faultEntry)
        {
            if (id != faultEntry.No)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faultEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaultEntryExists(faultEntry.No))
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
            return View(faultEntry);
        }

        // GET: FaultEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faultEntry = await _context.FaultEntry
                .FirstOrDefaultAsync(m => m.No == id);
            if (faultEntry == null)
            {
                return NotFound();
            }

            return View(faultEntry);
        }

        // POST: FaultEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faultEntry = await _context.FaultEntry.FindAsync(id);
            if (faultEntry != null)
            {
                _context.FaultEntry.Remove(faultEntry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FaultEntryExists(int id)
        {
            return _context.FaultEntry.Any(e => e.No == id);
        }
    }
}

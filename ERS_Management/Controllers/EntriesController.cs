using ERS_Management.Data;
using ERS_Management.Models;
using ERS_Management.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ERS_Management.Controllers
{
    public class EntriesController : Controller
    {
        private readonly ERS_ManagementContext _context;

        public EntriesController(ERS_ManagementContext context)
        {
            _context = context;
        }

        // GET: Entries
        public async Task<IActionResult> Home(string searchString, string sortOrder, int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["AreaSortParm"] = sortOrder == "area" ? "area_desc" : "area";
            ViewData["LocationSortParm"] = sortOrder == "location" ? "location_desc" : "location";

            var entries = from e in _context.Entries select e;

            // ----- SEARCH -----
            if (!string.IsNullOrEmpty(searchString))
            {
                entries = entries.Where(e =>
                    e.Area.Contains(searchString) ||
                    e.Location.Contains(searchString) ||
                    e.ItemType.Contains(searchString) ||
                    (e.Brand != null && e.Brand.Contains(searchString)) ||
                    (e.Model != null && e.Model.Contains(searchString)));
            }

            // ----- SORT -----
            entries = sortOrder switch
            {
                "area" => entries.OrderBy(e => e.Area),
                "area_desc" => entries.OrderByDescending(e => e.Area),
                "location" => entries.OrderBy(e => e.Location),
                "location_desc" => entries.OrderByDescending(e => e.Location),
                _ => entries.OrderBy(e => e.SerialNumber),
            };


            return View(entries);
        }

        // GET: Entries/Create
        public IActionResult Create()
        {
            LoadDropdowns();

            return View();
        }

        // POST: Entries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    [Bind("Area,Location,Brand,Model,ItemType,UserPurpose,AssetNo,SerialNo,Quantity,Status,Remarks,Username")]
    Entries entry)
        {
            // ---------- 1. Validate ----------
            if (!ModelState.IsValid)
            {
                // Repopulate any dropdowns you may have (e.g. Status)
                ViewData["Status"] = new SelectList(Enum.GetValues(typeof(EntryStatus)));
                return View(entry);
            }

            // ---------- 2. Set audit fields ----------
            entry.CreatedAt = DateTime.UtcNow;

            // ---------- 3. Persist the entry ----------
            _context.Entries.Add(entry);
            int count = await _context.SaveChangesAsync();   // <-- SerialNumber (PK) is now filled by DB

            // ---------- 4. Create log ----------
            var log = new EntryLog
            {
                EnteredBy = User.Identity.Name,
                EntryTime = entry.CreatedAt,
                logAction = LogAction.Created,
                // Use the **generated PK**, not the user-supplied SerialNo
                FaultId = entry.SerialNumber
            };

            _context.EntryLog.Add(log);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Home));
        }
        private void LoadDropdowns()
        {
            ViewBag.Areas = new SelectList(_context.Entries.Select(c => c.Area).Distinct().ToList());
            ViewBag.Brands = new SelectList(_context.Entries.Select(c => c.Brand).Distinct().ToList());
            ViewBag.Locations = new SelectList(_context.Entries.Select(c => c.Location).Distinct().ToList());
            ViewBag.Status = new SelectList(_context.Entries.Select(c => c.Status).Distinct().ToList());
            ViewBag.ItemTypes = new SelectList(_context.Entries.Select(c => c.ItemType).Distinct().ToList());
            ViewBag.Models = new SelectList(_context.Entries.Select(c => c.Model).Distinct().ToList());
            ViewBag.Purposes = new SelectList(_context.Entries.Select(c => c.UserPurpose).Distinct().ToList());
        }
        // GET: Entries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var entry = await _context.Entries.FindAsync(id);
            if (entry == null) return NotFound();


            return View(entry);
        }




        [HttpGet]
        public async Task<IActionResult> GetLog(string sn)
        {
            if (string.IsNullOrWhiteSpace(sn))
                return Json(new object[0]);

            var logs = await _context.EntryLog
                .Where(l => l.FaultId == Convert.ToInt32(sn))
                .OrderBy(l => l.EntryTime)
                .Select(l => new
                {
                    time = l.EntryTime.ToString("yyyy-MM-dd HH:mm:ss"), // BDT
                    user = l.EnteredBy ?? "system",
                    action = l.logAction == LogAction.Created ? "Created" : l.logAction == LogAction.Updated ? "Updated" : "Deleted",

                })
                .ToListAsync();

            return Json(logs);
        }






        // POST: Entries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("SerialNumber,Area,Location,Brand,Model,ItemType,UserPurpose,AssetNo,SerialNo,Quantity,Status,Remarks,Username,CreatedAt")] Entries entry)
        {
            if (id != entry.SerialNumber) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    entry.LastUpdate = DateTime.UtcNow;
                    _context.Update(entry);
                    await _context.SaveChangesAsync();


                    var log = new EntryLog
                    {
                        EnteredBy = User.Identity.Name,
                        EntryTime = DateTime.UtcNow,
                        logAction = LogAction.Updated,
                        // Use the **generated PK**, not the user-supplied SerialNo
                        FaultId = entry.SerialNumber
                    };

                    _context.EntryLog.Add(log);
                    await _context.SaveChangesAsync();


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntryExists(entry.SerialNumber)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Home));
            }
            return View(entry);
        }

        // GET: Entries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var entry = await _context.Entries.FirstOrDefaultAsync(m => m.SerialNumber == id);
            if (entry == null) return NotFound();
            return View(entry);
        }

        // GET: Entries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var entry = await _context.Entries.FirstOrDefaultAsync(m => m.SerialNumber == id);
            if (entry == null) return NotFound();
            return View(entry);
        }


        public async Task<IActionResult> AddDropdownData(string? name, string? field)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(field))
            {
                return BadRequest("Invalid name or field.");
            }
            bool exists = field.ToLower() switch
            {
                "area" => await _context.Entries.AnyAsync(e => e.Area == name),
                "userpurpose" => await _context.Entries.AnyAsync(e => e.UserPurpose == name),
                _ => false,
            };
            if (exists)
            {
                return Json(new { success = false, message = $"'{name}' already exists." });
            }
            else
            {


            }








            // Since Areas and UserPurposes are not separate entities, we don't add them to a separate table.
            // They will be added when a new Entry is created with these values.
            return View();
        }


        // POST: Entries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entry = await _context.Entries.FindAsync(id);
            if (entry != null)
            {
                _context.Entries.Remove(entry);
                await _context.SaveChangesAsync();


                var log = new EntryLog
                {
                    EnteredBy = User.Identity.Name,
                    EntryTime = DateTime.UtcNow,
                    logAction = LogAction.Deleted,
                    // Use the **generated PK**, not the user-supplied SerialNo
                    FaultId = entry.SerialNumber
                };

                _context.EntryLog.Add(log);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Home));
        }

        private bool EntryExists(int id) => _context.Entries.Any(e => e.SerialNumber == id);
    }
}

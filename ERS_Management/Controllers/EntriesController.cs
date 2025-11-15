using ERS_Management.Data;
using ERS_Management.Models;
using ERS_Management.PaginatedList;
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
        public async Task<IActionResult> Index(string searchString, string sortOrder, int? pageNumber)
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

            // ----- PAGING -----
            int pageSize = 10;
            var paged = await PaginatedList<Entries>.CreateAsync(entries.AsNoTracking(), pageNumber ?? 1, pageSize);
            return View(paged);
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
        public async Task<IActionResult> Create([Bind(
            "Area,Location,Brand,Model,ItemType,UserPurpose,AssetNo,SerialNo,Quantity,Status,Remarks,Username")] Entries entry)
        {

            if (ModelState.IsValid)
            {
                entry.CreatedAt = DateTime.UtcNow;

                _context.Add(entry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entry);
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
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntryExists(entry.SerialNumber)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
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
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EntryExists(int id) => _context.Entries.Any(e => e.SerialNumber == id);
    }
}

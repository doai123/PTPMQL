using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.Data;
using Microsoft.AspNetCore.Authorization;
using mvc.Models.Entities; 
using mvc.Models.Process;
namespace mvc.Controllers

{
    // [Authorize(Policy = "PolicyEmployee")]
    // [Authorize(Policy = nameof(SystemPermissions.))]
    [Authorize]
    public class MemberUnitController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemberUnitController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MemberUnit'
        [Authorize(Policy = nameof(SystemPermissions.MemberUnitView))]
        public async Task<IActionResult> Index()
        {
            return View(await _context.MemberUnit.ToListAsync());
        }

        // GET: MemberUnit/Details/5
        // [Authorize(Policy = nameof(SystemPermissions.MemberUnitView))]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberUnit = await _context.MemberUnit
                .FirstOrDefaultAsync(m => m.MemberUnitId == id);
            if (memberUnit == null)
            {
                return NotFound();
            }

            return View(memberUnit);
        }

        // GET: MemberUnit/Create
        // [Authorize(Policy = "PolicyAdmin")]
        [Authorize(Policy = nameof(SystemPermissions.MemberUnitCreate))]
        public IActionResult Create()
        {
            return View();
        }

        // POST: MemberUnit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = nameof(SystemPermissions.MemberUnitCreate))]
        public async Task<IActionResult> Create(MemberUnit memberUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(memberUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(memberUnit);
        }

        // GET: MemberUnit/Edit/5
        // [Authorize(Policy = "PolicyAdmin")]
        [Authorize(Policy = nameof(SystemPermissions.MemberUnitEdit))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberUnit = await _context.MemberUnit.FindAsync(id);
            if (memberUnit == null)
            {
                return NotFound();
            }
            return View(memberUnit);
        }

        // POST: MemberUnit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Authorize(Policy = "PolicyAdmin")]
        [Authorize(Policy = nameof(SystemPermissions.MemberUnitEdit))]
        public async Task<IActionResult> Edit(int id, [Bind("MemberUnitId,Name,Description")] MemberUnit memberUnit)
        {
            if (id != memberUnit.MemberUnitId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memberUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberUnitExists(memberUnit.MemberUnitId))
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
            return View(memberUnit);
        }

        // GET: MemberUnit/Delete/5
        // [Authorize(Policy = "PolicyAdmin")]
        [Authorize(Policy = nameof(SystemPermissions.MemberUnitDelete))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberUnit = await _context.MemberUnit
                .FirstOrDefaultAsync(m => m.MemberUnitId == id);
            if (memberUnit == null)
            {
                return NotFound();
            }

            return View(memberUnit);
        }

        // POST: MemberUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // [Authorize(Policy = "PolicyAdmin")]
        [Authorize(Policy = nameof(SystemPermissions.MemberUnitDelete))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memberUnit = await _context.MemberUnit.FindAsync(id);
            if (memberUnit != null)
            {
                _context.MemberUnit.Remove(memberUnit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberUnitExists(int id)
        {   
            return _context.MemberUnit.Any(e => e.MemberUnitId == id);
        }
    }
}

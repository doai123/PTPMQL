using Microsoft.AspNetCore.Mvc;
using mvc.Data;
using mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace mvc.Controllers
{
    public class PersonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách Person
        public async Task<IActionResult> Data()
        {
            var persons = await _context.Persons.ToListAsync();
            return View(persons);
        }

        // Hiển thị form thêm Person
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý thêm Person
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction("Data");
            }
            return View(person);
        }

        // 🛠 CHỈNH SỬA Person - GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var person = await _context.Persons.FindAsync(id);
            if (person == null) return NotFound();

            return View(person);
        }

        // 🛠 CHỈNH SỬA Person - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Person person)
        {
            if (id != person.PersonId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(person);
                await _context.SaveChangesAsync();
                return RedirectToAction("Data");
            }
            return View(person);
        }

        // ❌ XOÁ Person - GET (Xác nhận trước khi xóa)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var person = await _context.Persons.FindAsync(id);
            if (person == null) return NotFound();

            return View(person);
        }

        // ❌ XOÁ Person - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person != null)
            {
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Data");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using mvc.Data;
using mvc.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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
        
    [HttpPost]
    public async Task<IActionResult> ImportFromExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["Error"] = "Vui lòng chọn file Excel!";
            return RedirectToAction("Data");
        }

        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var person = new Person
                        {
                            FullName = worksheet.Cells[row, 1].Value?.ToString()?.Trim() ?? "",
                            Address = worksheet.Cells[row, 2].Value?.ToString()?.Trim() ?? ""
                        };

                        _context.Persons.Add(person);
                    }

                    await _context.SaveChangesAsync();
                }
            }

            TempData["Success"] = "Nhập dữ liệu từ Excel thành công!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Lỗi khi nhập Excel: " + ex.Message;
        }

        return RedirectToAction("Data");
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

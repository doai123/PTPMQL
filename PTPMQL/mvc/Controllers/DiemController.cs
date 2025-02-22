using Microsoft.AspNetCore.Mvc;
using mvc.Models;

namespace mvc.Controllers
{
    public class DiemController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TinhDiem(Diem model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            ViewBag.SumDiem = model.SumDiem;
            return View("Create", model);
        }
    }
}

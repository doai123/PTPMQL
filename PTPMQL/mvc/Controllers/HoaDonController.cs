using Microsoft.AspNetCore.Mvc;
using mvc.Models;

namespace mvc.Controllers
{
    public class HoaDonController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult TinhTien(HoaDon model)
        {
            if (model.SoLuong < 1 || model.DonGia < 1000)
            {
                ViewBag.Error = "Số lượng phải lớn hơn 0 và đơn giá phải lớn hơn 1000!";
                return View("Create");
            }

            ViewBag.ThanhTien = model.ThanhTien;
            return View("Create", model);
        }
        
    }
}

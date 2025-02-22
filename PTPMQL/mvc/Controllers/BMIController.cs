using Microsoft.AspNetCore.Mvc;
using mvc.Models;
namespace mvc.Controllers;
public class BMIController : Controller
{
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Calculate(BMI model)
    {
        if (model.Weight <= 0 || model.Height <= 0)
        {
            ViewBag.Error = "Cân nặng và chiều cao phải lớn hơn 0.";
            return View();
        }

        model.CalculateBMI = model.Weight / (model.Height * model.Height);

        if (model.CalculateBMI < 18.5)
            model.Category = "Thiếu cân";
        else if (model.CalculateBMI >= 18.5 && model.CalculateBMI < 24.9)
            model.Category = "Bình thường";
        else if (model.CalculateBMI >= 25 && model.CalculateBMI < 29.9)
            model.Category = "Thừa cân";
        else
            model.Category = "Béo phì";

        ViewBag.Message = $"Chỉ số CalculateBMI: {model.CalculateBMI} kết quả: {model.Category}";
        return View("Calculate", model);
    }
}

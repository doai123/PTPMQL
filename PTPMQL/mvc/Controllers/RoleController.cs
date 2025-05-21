using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mvc.Models;
using mvc.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using mvc.Models.Process;

namespace mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                var role = new IdentityRole(roleName.Trim());
                await _roleManager.CreateAsync(role);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string newName)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(newName))
            {
                role.Name = newName.Trim();
                await _roleManager.UpdateAsync(role);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                // Tìm tất cả user có role này
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                foreach (var user in usersInRole)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }

                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    TempData["Error"] = string.Join("; ", result.Errors.Select(e => e.Description));
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AssignClaim(string id)
{
    var allPermissions = System.Enum.GetValues(typeof(SystemPermissions))
        .Cast<SystemPermissions>()
        .Select(p => p.ToString())
        .ToList();

    List<Claim> roleClaims = new List<Claim>();
    string roleName = string.Empty;
    string roleId = id;

    // Các quyền mặc định muốn tích sẵn
    var defaultPermissions = new List<string> { "EmployeeView", "RoleView", "AccountView", "MemberUnitView" };

    if (!string.IsNullOrEmpty(id))
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role != null)
        {
            roleName = role.Name;
            roleId = role.Id;
            var claims = await _roleManager.GetClaimsAsync(role);
            if (claims != null)
                roleClaims = claims.ToList();
        }
    }

    var model = new RoleClaimVM
    {
        RoleId = roleId,
        RoleName = roleName,
        Claims = allPermissions.Select(p => new RoleClaim
        {
            Type = "Permission",
            Value = p,
            Selected = roleClaims.Any(c => c.Type == "Permission" && c.Value == p)
                      // Nếu chưa có claim nào thì tích mặc định
                      || (roleClaims.Count == 0 && defaultPermissions.Contains(p))
        }).ToList()
    };

    return View(model);
}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignClaim(RoleClaimVM model)
        {
            if (!model.Claims.Any(c => c.Selected))
            {
                ModelState.AddModelError("", "Bạn phải chọn ít nhất một quyền cho role này!");
                return View(model);
            }

            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                ModelState.AddModelError("", "Role không tồn tại!");
                return View(model);
            }

            var claims = await _roleManager.GetClaimsAsync(role) ?? new List<Claim>();
            foreach (var claim in claims.Where(c => c.Type == "Permission"))
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            foreach (var claim in model.Claims.Where(c => c.Selected))
            {
                var result = await _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value));
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

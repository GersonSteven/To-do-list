using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ListaTareas.Models.Users;

namespace ListaTareas.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ManageUserController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;

        public ManageUserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var admin = (await _userManager.GetUsersInRoleAsync("Administrador")).ToArray();

            var everyone = await _userManager.Users.ToArrayAsync();

            var model = new ManageUserModel
            {
                Adminstradors = admin,
                Everyone = everyone
            };

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var currenUser = await _userManager.FindByIdAsync(id);
            if (currenUser == null) return NotFound();

            return View(currenUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletedConfirm(string id)
        {
            var currentUser = await _userManager.FindByIdAsync(id);
            if (currentUser != null)
            {
                await _userManager.DeleteAsync(currentUser);
            }

            return RedirectToAction("Index");
        }


    }
}

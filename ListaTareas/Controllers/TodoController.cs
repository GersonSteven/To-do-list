using Microsoft.AspNetCore.Mvc;
using ListaTareas.Models.Todo;
using ListaTareas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using X.PagedList.Extensions;

namespace ListaTareas.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoService _todoServices;
        private readonly UserManager<IdentityUser> _userManager;

        public TodoController(ITodoService todoService, UserManager<IdentityUser> userManager)
        {
            _todoServices = todoService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? page, string searchString)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;
            var currentUser = await _userManager.GetUserAsync(User);
            ViewData["CurrentSearch"] = searchString;

            if (currentUser == null) return Challenge();

            var todoItem = await _todoServices.GetImcompleteAsync(currentUser, searchString);
            var pageTodo = todoItem.ToPagedList(pageNumber, pageSize);

            return View(pageTodo);
        }

        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(Item todo)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return RedirectToAction("Index");
            }

            var result = await _todoServices.AddItemsAsync(todo, currentUser);

            if (!result)
            {
                return BadRequest("No se agrego la tarea");
            }

            return RedirectToAction("Index");

        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Index");
            }

            var susseful = await _todoServices.MarkDoneAsync(id, currentUser);

            if (!susseful)
            {
                return BadRequest("No se pudo finalizar la tarea");
            }

            return RedirectToAction("Index");
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return RedirectToAction("Index");

            var model = await _todoServices.FindTodoAsync(id);

            if (model == null) return RedirectToAction("Index");

            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirm(Item todo)
        {
            if (!ModelState.IsValid) return RedirectToAction("Edit");

            var susseful = await _todoServices.EditTodoAsync(todo);

            if(!susseful) return RedirectToAction("Edit");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) { return RedirectToAction("Index"); }

            var sucessful = await _todoServices.DeleteTodoAsync(id);

            if (sucessful) { return RedirectToAction("Index"); }

            return RedirectToAction("Index");
        }
    }
}

using ListaTareas.Models.Todo;
using ListaTareas.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ListaTareas.Services
{
    public class TodoService : ITodoService
    {
        private readonly ApplicationDbWebContext _context;

        public TodoService(ApplicationDbWebContext context)
        {
            _context = context;
        }

        public async Task<Todo[]> GetImcompleteAsync(IdentityUser user, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return await _context.Items.Where(
                    x => x.IsDone == false && 
                    x.UserId == user.Id && 
                    x.Title.Contains(searchString.ToUpper()))
                    .OrderBy(x => x.CreatedAt)
                    .ToArrayAsync();
            }

            return await _context.Items
                .Where(x => x.IsDone == false && x.UserId == user.Id).OrderBy(x => x.CreatedAt)
                .ToArrayAsync();
        }

        public async Task<bool> AddItemsAsync(Todo todo, IdentityUser user)
        {
            todo.Id = Guid.NewGuid();
            todo.UserId = user.Id;

            if (todo.CreatedAt < DateTime.Now.Date) { return false; }

            _context.Items.Add(todo);

            var result = await _context.SaveChangesAsync();

            return result == 1;
        }

        public async Task<bool> MarkDoneAsync(Guid id, IdentityUser user)
        {
            var item = await _context.Items
                .Where(x => x.Id == id && x.UserId == user.Id)
                .SingleOrDefaultAsync();

            if (item == null) return false;

            item.IsDone = true;

            var saveResult = await _context.SaveChangesAsync();

            return saveResult == 1;
        }

        public async Task<Todo?> FindTodoAsync(Guid id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task<bool> EditTodoAsync(Todo todo)
        {
            _context.Update(todo);

            var result = await _context.SaveChangesAsync();

            return result == 1;
        }

        public async Task<bool> DeleteTodoAsync(Guid id)
        {
            var item = await FindTodoAsync(id);
            if (item == null) return false;

            _context.Remove(item);
            var result = await _context.SaveChangesAsync();
            return result == 1;
        }
    }
}

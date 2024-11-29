using ListaTareas.Models.Todo;
using Microsoft.AspNetCore.Identity;

namespace ListaTareas.Services
{
    public interface ITodoService
    {
        Task<Todo[]> GetImcompleteAsync(IdentityUser user, string searchString);
        Task<bool> AddItemsAsync(Todo todo, IdentityUser user);
        Task<bool> MarkDoneAsync(Guid id, IdentityUser user);
        Task<bool> EditTodoAsync(Todo todo);
        Task<Todo?> FindTodoAsync(Guid id);
        Task<bool> DeleteTodoAsync(Guid id);
    }
}

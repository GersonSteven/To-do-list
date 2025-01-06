using ListaTareas.Models.Todo;
using Microsoft.AspNetCore.Identity;

namespace ListaTareas.Services
{
    public interface ITodoService
    {
        Task<Item[]> GetImcompleteAsync(IdentityUser user, string searchString);
        Task<bool> AddItemsAsync(Item todo, IdentityUser user);
        Task<bool> MarkDoneAsync(Guid id, IdentityUser user);
        Task<bool> EditTodoAsync(Item todo);
        Task<Item?> FindTodoAsync(Guid id);
        Task<bool> DeleteTodoAsync(Guid id);
    }
}

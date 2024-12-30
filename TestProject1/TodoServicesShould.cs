using ListaTareas.Data;
using ListaTareas.Models.Todo;
using ListaTareas.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TestProject1
{
    public class TodoServicesShould
    {
        private readonly IdentityUser _fakeUser;
        private readonly Todo _todo;

        public TodoServicesShould()
        {
            _fakeUser = new IdentityUser
            {
                Id = "Fake-0000",
                UserName = "fake@fake"
            };

            _todo = new Todo
            {
                Title = "Testing?",
                CreatedAt = DateTimeOffset.Now
            };
        }

        [Fact]
        public async Task AddNewItem_IncompleteTodo()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbWebContext>()
                .UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;

            using (var inmemoryContext = new ApplicationDbWebContext(options))
            {
                var service = new TodoService(inmemoryContext);

                await service.AddItemsAsync(_todo, _fakeUser);
                Todo?  task = await service.FindTodoAsync(_todo.Id);
                var diferrence = DateTimeOffset.Now - task!.CreatedAt;

                Assert.Equal(1, await inmemoryContext.Items.CountAsync());
                Assert.Equal("Fake-0000", task.UserId);
                Assert.Equal("Testing?", task.Title);
                Assert.False(task.IsDone);
                Assert.True(diferrence < TimeSpan.FromSeconds(2));
            }
        }

        [Fact]
        public async Task MarkDoneTodo_ExsitsTodo()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbWebContext>()
                .UseInMemoryDatabase(databaseName: "Test_MarkDoneTodoExists").Options;

            using (var inmemoryContext = new ApplicationDbWebContext(options))
            {
                var service = new TodoService(inmemoryContext);

                await service.AddItemsAsync(_todo, _fakeUser);
                var result = await service.MarkDoneAsync(_todo.Id, _fakeUser);
                var item = service.FindTodoAsync(_todo.Id);

                Assert.Equal(1, await inmemoryContext.Items.CountAsync());
                Assert.True(result);
                Assert.NotNull(item);
                Assert.True(_todo.IsDone);
            }
        }
    }
}
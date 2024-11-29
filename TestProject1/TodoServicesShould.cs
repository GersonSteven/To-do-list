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
        public async Task AddNewItemIncomplete()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbWebContext>()
                .UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;
            Todo? item;

            using (var inmemoryContext = new ApplicationDbWebContext(options))
            {
                var service = new TodoService(inmemoryContext);

                await service.AddItemsAsync(_todo, _fakeUser);
                item = await service.FindTodoAsync(_todo.Id);
                var diferrence = DateTimeOffset.Now - item!.CreatedAt;

                Assert.Equal(1, await inmemoryContext.Items.CountAsync());
                Assert.Equal("Fake-0000", item.UserId);
                Assert.Equal("Testing?", item.Title);
                Assert.False(item!.IsDone);
                Assert.True(diferrence < TimeSpan.FromSeconds(1));
            }
        }

        [Fact]
        public async Task MarkDoneTodoExsits()
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
                Assert.True(_todo.IsDone);
            }
        }
    }
}
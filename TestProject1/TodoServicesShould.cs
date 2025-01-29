using ListaTareas.Data;
using ListaTareas.Models.Todo;
using ListaTareas.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;

namespace TestProject1
{
    public class TodoServicesShould
    {
        private readonly IdentityUser _fakeUser;
        private readonly Item _todo;

        public TodoServicesShould()
        {
            _fakeUser = new IdentityUser
            {
                Id = "Fake-0000",
                UserName = "fake@fake"
            };

            _todo = new Item
            {
                Title = "Testing?",
                CreatedAt = DateTimeOffset.Now
            };
        }

        [Fact]
        public async Task Add_NewItem_IncompleteTodo()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbWebContext>()
                .UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;

            using (var inmemoryContext = new ApplicationDbWebContext(options))
            {
                var service = new TodoService(inmemoryContext);

                await service.AddItemsAsync(_todo, _fakeUser);
                Item?  task = await service.FindTodoAsync(_todo.Id);
                var diferrence = DateTimeOffset.Now - task!.CreatedAt;

                Assert.Equal(1, await inmemoryContext.Items.CountAsync());
                Assert.Equal("Fake-0000", task.UserId);
                Assert.Equal("Testing?", task.Title);
                Assert.False(task.IsDone);
                Assert.True(diferrence < TimeSpan.FromSeconds(2));
            }
        }

        [Fact]
        public async Task Mark_DoneTodo_ExsitsTodo()
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

        [Fact]
        public async Task Delete_Task_ExsistsTask()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbWebContext>()
                .UseInMemoryDatabase(databaseName: "Delete_taskTest").Options;

            using (var inmemoryContext = new ApplicationDbWebContext(options))
            {
                var mockService = new TodoService(inmemoryContext);

                await mockService.AddItemsAsync(_todo, _fakeUser);
                bool response = await mockService.DeleteTodoAsync(_todo.Id);

                Assert.True(response);
                Assert.Equal(0, await inmemoryContext.Items.CountAsync());
            }
        }
    }
}
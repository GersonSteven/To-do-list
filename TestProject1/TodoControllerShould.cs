using ListaTareas.Services;
using NSubstitute;
using ListaTareas.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace TestProject1
{
    public class TodoControllerShould
    {
        /// <summary>
        /// Mark done action.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RediretcToAction_ForMarkDoneWhitEmptyId()
        {
            //Arrange.
            var mockTodoService = Substitute.For<ITodoService>();
            var mockUserManager = MockUserManager.Create();
            var controller = new TodoController(mockTodoService, mockUserManager);

            //Act.
            var result = await controller.MarkDone(Guid.Empty) as RedirectToActionResult;

            //Assert.
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task ReturnUnauthorize_ForMarkDoneWhitNoUser()
        {
            //Arrange.
            var mockTodoService = Substitute.For<ITodoService>();
            var mockUserManager = MockUserManager.Create();

            //Make the mockUserManager always return null for GetUserAsync.
            mockUserManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns(Task.FromResult<IdentityUser?>(null));
            var controller = new TodoController(mockTodoService, mockUserManager);

            //Act
            var ramdonId = Guid.NewGuid();
            var result = await controller.MarkDone(ramdonId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }






    }
}

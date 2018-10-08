using FinalApp.Models;
using FinalApp.Areas.Identity.Pages.Account;
using FinalApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FinalApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Http;
using FinalApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;

namespace FinalAppTest
{
    public class UnitTest1
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly UserManager<IdentityUser> _userManager;

        [Fact]
        public async Task Category_ShouldCreateNewCategoryAsync()
        {
            // Assemble
            var context = new ApplicationContext(DbAssembly().Options);
            var categoryController = new CategoryController(context);
            var category = (new Category());

            // Act
            await categoryController.Create(category);

            // Assert
            Assert.NotEmpty(context.Categories);
        }
        
        [Fact]
        public void HomeController_IndexActionShouldReturnIndexView()
        {
            //Assemble
            var homeController = new HomeController();

            //Act
            var result = homeController.Index();

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        public DbContextOptionsBuilder<ApplicationContext> DbAssembly()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
#pragma warning disable CS0618 // Type or member is obsolete
            optionsBuilder.UseInMemoryDatabase();
#pragma warning restore CS0618 // Type or member is obsolete
            return optionsBuilder;
        }
        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductManagement.Controllers;
using ProductManagement.Models.ViewModel;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ProductTest
{
    public class AccountControllerTests
    {
        [Fact]
        public async Task Login_ValidCredentials_RedirectToUserDashboard()
        {
            var signInManagerMock = new Mock<SignInManager<IdentityUser>>(
               new Mock<UserManager<IdentityUser>>(
                   new Mock<IUserStore<IdentityUser>>().Object,
                   null, null, null, null, null, null, null, null
               ).Object,
               null, null, null, null, null
           );

            var accountController = new AccountController(signInManagerMock.Object, null);
            var loginModel = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "password",
                RememberMe = false
            };

            signInManagerMock
                .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            signInManagerMock
                .Setup(x => x.UserManager.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityUser());

            signInManagerMock
                .Setup(x => x.UserManager.GetRolesAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(new[] { "User" });

            // Act
            var result = await accountController.Login(loginModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UserDashboard", result.ActionName);
            Assert.Equal("User", result.ControllerName);
        }

        [Fact]
        public async Task Register_ValidCredentials_RedirectToLogin()
        {
            // Arrange
            var signInManagerMock = new Mock<SignInManager<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(new Mock<IUserStore<IdentityUser>>().Object, null, null, null, null, null, null, null, null);
            var accountController = new AccountController(signInManagerMock.Object, userManagerMock.Object);
            var registerModel = new RegisterViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "test@example.com",
                Password = "SecurePassword123",
                ConfirmPassword = "SecurePassword123"
            };

            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            signInManagerMock.Setup(x => x.SignInAsync(It.IsAny<IdentityUser>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(Task.FromResult(0)); 

            // Act
            var result = await accountController.Register(registerModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Login", result.ActionName);
            Assert.Equal("Account", result.ControllerName);
        }
    }
}

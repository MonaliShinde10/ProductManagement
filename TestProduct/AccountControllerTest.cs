using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagement.Controllers;
using ProductManagement.Models.ViewModel;
using ProductManagement.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TestProduct
{
    public class AccountControllerTest
    {
        [Fact]
        public async Task Register_ValidModel_ReturnsRedirectToLogin()
        {
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(mock => mock.RegisterAsync(It.IsAny<RegisterViewModel>())).ReturnsAsync(true);
            var controller = new AccountController(userServiceMock.Object);
            var model = new RegisterViewModel();

            var result = await controller.Register(model) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Login", result.ActionName);
        }
        [Fact]
        public async Task Register_InvalidModel_ReturnsViewWithModel()
        {
            var userServiceMock = new Mock<IUserService>();
            var controller = new AccountController(userServiceMock.Object);
            controller.ModelState.AddModelError("ErrorKey", "ErrorMessage");
            var model = new RegisterViewModel();

            var result = await controller.Register(model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
        }

        [Fact]
        public async Task Login_ValidCredentialsAsUser_RedirectsToCorrectDashboard()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();

            userServiceMock.Setup(service => service.LoginAsync(It.IsAny<LoginViewModel>()))
                .ReturnsAsync(true);

            userServiceMock.Setup(service => service.GetRolesAsync("validemail@example.com"))
                .ReturnsAsync(new string[] { "User" });

            userServiceMock.Setup(service => service.FindByEmailAsync("validemail@example.com"))
                .ReturnsAsync(new UserModel()); 

            var controller = new AccountController(userServiceMock.Object);
            var loginViewModel = new LoginViewModel
            {
                Email = "validemail@example.com",
                Password = "validpassword"
            };

            // Act
            var result = await controller.Login(loginViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UserDashboard", result.ActionName);
            Assert.Equal("User", result.ControllerName);
        }
        [Fact]
        public async Task Login_ValidCredentialsAsSuperAdmin_RedirectsToCorrectDashboard()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();

            userServiceMock.Setup(service => service.LoginAsync(It.IsAny<LoginViewModel>()))
                .ReturnsAsync(true);

            userServiceMock.Setup(service => service.GetRolesAsync("validemail@example.com"))
                .ReturnsAsync(new string[] { "SuperAdmin" });

            userServiceMock.Setup(service => service.FindByEmailAsync("validemail@example.com"))
                .ReturnsAsync(new UserModel());

            var controller = new AccountController(userServiceMock.Object);
            var loginViewModel = new LoginViewModel
            {
                Email = "validemail@example.com",
                Password = "validpassword"
            };

            // Act
            var result = await controller.Login(loginViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("SuperAdminDashboard", result.ActionName);
            Assert.Equal("SuperAdmin", result.ControllerName);
        }
        [Fact]
        public async Task Login_ValidCredentialsAsAdmin_RedirectsToCorrectDashboard()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();

            userServiceMock.Setup(service => service.LoginAsync(It.IsAny<LoginViewModel>()))
                .ReturnsAsync(true);

            userServiceMock.Setup(service => service.GetRolesAsync("validemail@example.com"))
                .ReturnsAsync(new string[] { "Admin" });

            userServiceMock.Setup(service => service.FindByEmailAsync("validemail@example.com"))
                .ReturnsAsync(new UserModel());

            var controller = new AccountController(userServiceMock.Object);
            var loginViewModel = new LoginViewModel
            {
                Email = "validemail@example.com",
                Password = "validpassword"
            };

            // Act
            var result = await controller.Login(loginViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("AdminDashboard", result.ActionName);
            Assert.Equal("Product", result.ControllerName);
        }
        [Fact]
        public async Task Login_InvalidCredentials_ReturnsViewWithError()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();

            // Set up the mock to return false for LoginAsync
            userServiceMock.Setup(service => service.LoginAsync(It.IsAny<LoginViewModel>()))
                .ReturnsAsync(false);

            var controller = new AccountController(userServiceMock.Object);
            var loginViewModel = new LoginViewModel
            {
                Email = "invalidemail@example.com",
                Password = "invalidpassword"
            };

            // Act
            var result = await controller.Login(loginViewModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);
            Assert.Contains("Invalid login attempt.", controller.ModelState[""].Errors[0].ErrorMessage);
        }


    }
}

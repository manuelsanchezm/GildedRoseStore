using GildedRoseStore.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace GildedRoseStore.Tests
{
    public class AccountControllerTests
    {
        [Fact]
        public async void Account_Login_ReturnsAView_Success()
        {
            // Arrange
            var controller = GetAccountControllerSignOut();

            // Act
            ViewResult result = await controller.Login() as ViewResult;

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async void Account_LoginPost_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var configuration = Mock.Of<IConfiguration>();
            var controller = new AccountController(configuration);
            controller.ModelState.AddModelError("SessionName", "Required");
            string loginName = "";
            string password = "";
            bool rememberMe = true;

            // Act
            var result = await controller.Login(loginName, password, rememberMe);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async void Account_LoginPost_ReturnsAView_WhenCredentialsAreNull()
        {
            // Arrange
            var configuration = Mock.Of<IConfiguration>();
            var controller = new AccountController(configuration);
            string loginName = default;
            string password = default;
            bool rememberMe = false;

            // Act
            var result = await controller.Login(loginName, password, rememberMe);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async void Account_LoginPost_ReturnsRedirect_WhenAuthenticationPasswordFail()
        {
            // Arrange
            string projectPath = string.Concat(AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0], @"AppSettingsFiles");
            IConfiguration _configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("WrongPassword.json")
                .Build();

            var controller = new AccountController(_configuration);
            string loginName = "userTest";
            string password = "passwordTest";
            bool rememberMe = false;

            // Act
            var result = await controller.Login(loginName, password, rememberMe);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async void Account_LoginPost_ReturnsRedirect_WhenAuthenticationPasswordSuccess()
        {
            // Arrange
            // mocking async context object
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(s => s.SignInAsync(It.IsAny<HttpContext>(),
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        Mock.Of<ClaimsPrincipal>(),
                        It.IsAny<AuthenticationProperties>())).
                        Returns(Task.FromResult(true));

            var servicesProviderMock = new Mock<IServiceProvider>();
            servicesProviderMock
                .Setup(sp => sp.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);
            servicesProviderMock
                .Setup(sp => sp.GetService(typeof(IUrlHelperFactory))).Returns(new UrlHelperFactory());

            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            servicesProviderMock
                .Setup(sp => sp.GetService(typeof(ITempDataDictionaryFactory)))
                .Returns(new TempDataDictionaryFactory(tempDataProvider));

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            context.HttpContext.RequestServices = servicesProviderMock.Object;

            // Getting configuration files
            string projectPath = string.Concat(AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0], @"AppSettingsFiles");
            IConfiguration _configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("Complete.json")
                .Build();

            var controller = new AccountController(_configuration)
            {
                ControllerContext = context
            };

            string loginName = "userTest";
            string password = "passwordTest";
            bool rememberMe = false;

            // Act
            LocalRedirectResult result = await controller.Login(loginName, password, rememberMe) as LocalRedirectResult;

            // Assert
            var viewResult = Assert.IsType<LocalRedirectResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("/Home/Index", viewResult.Url);
        }

        [Fact]
        public async void Account_LogOut_ReturnsAView_Success() {
            // Arrange
            var controller = GetAccountControllerSignOut();

            // Act
            LocalRedirectResult result = await controller.Logout() as LocalRedirectResult;

            // Assert
            var viewResult = Assert.IsType<LocalRedirectResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("/Home/Index", viewResult.Url);
        }

        private AccountController GetAccountControllerSignOut()
        {
            var _configuration = Mock.Of<IConfiguration>();

            // Mocking async context
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(s => s.SignOutAsync(It.IsAny<HttpContext>(),
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        It.IsAny<AuthenticationProperties>())).
                        Returns(Task.FromResult(true));

            var servicesProviderMock = new Mock<IServiceProvider>();
            servicesProviderMock
                .Setup(sp => sp.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);
            servicesProviderMock
                .Setup(sp => sp.GetService(typeof(IUrlHelperFactory))).Returns(new UrlHelperFactory());

            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            servicesProviderMock
                .Setup(sp => sp.GetService(typeof(ITempDataDictionaryFactory)))
                .Returns(new TempDataDictionaryFactory(tempDataProvider));
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            context.HttpContext.RequestServices = servicesProviderMock.Object;

            var controller = new AccountController(_configuration)
            {
                ControllerContext = context
            };

            return controller;
        }
    }
}

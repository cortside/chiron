using Chiron.Auth.Controllers;
using Chiron.Auth.Data;
using Chiron.Auth.Models;
using Chiron.Auth.Services;
using IdentityServer4.Configuration;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Chiron.Auth.Tests.Controllers {

    public class AccountControllerTest : BaseTestFixture {
        AccountController target;
        Mock<IAuthenticator> authenticatorMock;
        Mock<HttpContext> httpContextMock;
        Mock<AuthenticationManager> authManagerMock;
        Mock<IIdentityServerInteractionService> idsServiceMock;
        Mock<IConfigurationRoot> configMock;
        Mock<ILogger<AccountController>> logMock;
        RouteData routeData;
        ActionDescriptor actionDescriptor;

        public AccountControllerTest() {
            authenticatorMock = new Mock<IAuthenticator>();
            idsServiceMock = new Mock<IIdentityServerInteractionService>();
            httpContextMock = new Mock<HttpContext>();
            authManagerMock = new Mock<AuthenticationManager>();
            configMock = new Mock<IConfigurationRoot>();
            logMock = new Mock<ILogger<AccountController>>();
            routeData = new RouteData();
            actionDescriptor = new ControllerActionDescriptor() { /* ??? */ };

            httpContextMock.Setup(c => c.RequestServices).Returns(CreateServices());
            authManagerMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            authManagerMock.Setup(x => x.GetAuthenticationSchemes())
                .Returns(new AuthenticationDescription[] {
                    new AuthenticationDescription() {
                        AuthenticationScheme = "Cookies",
                        DisplayName = "Cookies" }
                });
            httpContextMock.Setup(x => x.Authentication).Returns(authManagerMock.Object);

            target = new AccountController(authenticatorMock.Object, idsServiceMock.Object, configMock.Object, logMock.Object) {
                ControllerContext = new ControllerContext(new ActionContext(httpContextMock.Object, routeData, actionDescriptor))
            };
        }

        [Fact(Skip = "fails without setting cookies -- see TODO")]
        public async Task ShouldBeAbleToLogin_Valid_ReturnURL() {
            //Arrange
            var model = CreateTestData<LoginModel>();
            Arrange(() => {
                model.RememberLogin = false;
                var user = CreateTestData<User>();
                authenticatorMock.Setup(x => x.AuthenticateAsync(It.IsAny<LoginInfo>()))
                    .ReturnsAsync(user);
                authManagerMock.Setup(x => x.SignInAsync(It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), null))
                    .Returns(Task.FromResult(0));
                idsServiceMock.Setup(x => x.IsValidReturnUrl(model.ReturnUrl)).Returns(true);
            });

            //Act 
            var result = await target.Login(model) as RedirectResult;

            //Assert
            authenticatorMock.Verify(x => x.AuthenticateAsync(It.Is<LoginInfo>(l => l.Username == model.Username && l.Password == model.Password)));
            Assert.Equal(model.ReturnUrl, result.Url);
        }

        [Fact(Skip = "fails without setting cookies -- see TODO")]
        public async Task ShouldBeAbleToLogin_Invalid_ReturnURL() {
            //Arrange
            var model = CreateTestData<LoginModel>();
            var defaultUrl = Guid.NewGuid().ToString();
            Arrange(() => {
                model.RememberLogin = true;
                var user = CreateTestData<User>();
                authenticatorMock.Setup(x => x.AuthenticateAsync(It.IsAny<LoginInfo>()))
                    .ReturnsAsync(user);
                authManagerMock.Setup(x => x.SignInAsync(It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                    .Returns(Task.FromResult(0));
                idsServiceMock.Setup(x => x.IsValidReturnUrl(model.ReturnUrl)).Returns(false);
                configMock.Setup(x => x["defaultUrl"]).Returns(defaultUrl);
            });

            //Act 
            var result = await target.Login(model) as RedirectResult;

            //Assert
            authenticatorMock.Verify(x => x.AuthenticateAsync(It.Is<LoginInfo>(l => l.Username == model.Username && l.Password == model.Password)));
            Assert.Equal(defaultUrl, result.Url);
        }

        [Fact]
        public async Task ShouldBeAbleToLogin_Failed() {
            //Arrange
            var model = CreateTestData<LoginModel>();
            Arrange(() => {
                model.RememberLogin = true;
                authenticatorMock.Setup(x => x.AuthenticateAsync(It.IsAny<LoginInfo>()))
                    .ReturnsAsync(null as User);
            });

            //Act
            await target.Login(model);

            //Assert
            Assert.Equal(1, target.ModelState.ErrorCount);
        }

        [Fact(Skip = "fails without setting cookies -- see TODO")]
        public async Task ShouldBeAbleToLogout() {
            //Arrange
            var defaultUrl = Guid.NewGuid().ToString();
            Arrange(() => {
                configMock.Setup(x => x["defaultUrl"]).Returns(defaultUrl);
            });

            //Act
            var result = await target.Logout() as RedirectResult;

            //Assert
            authManagerMock.Verify(x => x.SignOutAsync("Cookies"));
            Assert.Equal(defaultUrl, result.Url);
        }

        private static IServiceProvider CreateServices() {
            var idsOptions = new IdentityServerOptions();
            // TODO: this has changed in the .net core 2.0 version -- figure out how this is set now
            //idsOptions.Authentication.AuthenticationScheme = "Cookies";
            var dicMock = new Mock<ITempDataDictionaryFactory>();

            return new ServiceCollection()
                .AddSingleton(new Mock<ILoggerFactory>().Object)
                .AddSingleton(idsOptions)
                .AddSingleton(dicMock.Object)
                .BuildServiceProvider();
        }
    }
}

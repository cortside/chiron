using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chiron.Auth.Data;
using Chiron.Auth.Services;
using IdentityServer4.Models;
using Moq;
using Xunit;

namespace Chiron.Auth.Tests.Services {

    public class UserProfileServiceTest : BaseTestFixture {
        UserProfileService target;
        Mock<IUserDbContextFactory> ctxFactoryMock;
        Mock<IUserDbContext> dbCtxMock;

        public UserProfileServiceTest() {
            ctxFactoryMock = new Mock<IUserDbContextFactory>();
            dbCtxMock = new Mock<IUserDbContext>();
            ctxFactoryMock.Setup(x => x.NewUserDbContext()).Returns(dbCtxMock.Object);

            target = new UserProfileService(ctxFactoryMock.Object);
        }

        [Fact]
        public async Task ShouldBeAbleToGetProfileDataAsync() {
            //Arrange
            var user = CreateTestData<User>();
            var ctx = new ProfileDataRequestContext {
                Subject = CreatePrincipal(user),
                RequestedClaimTypes = new string[] { }
            };
            Arrange(() => {
                var usersMock = MockAsyncQueryable(user);
                dbCtxMock.Setup(x => x.Users).Returns(usersMock.Object);
                ArrangeUserLists(user);
            });

            //Act
            await target.GetProfileDataAsync(ctx);

            //Assert
            Assert.Equal(2, ctx.IssuedClaims.Count);
            Assert.Equal(user.UserId.ToString(), ctx.IssuedClaims.First(x => x.Type == "sub").Value);
            Assert.Equal(user.Username, ctx.IssuedClaims.First(x => x.Type == "name").Value);
        }

        [Fact]
        public async Task ShouldBeAbleToGetIsActiveAsync_Active() {
            //Arrange
            var user = CreateTestData<User>();
            var ctx = new IsActiveContext(CreatePrincipal(user), new Client(), "asdf");
            Arrange(() => {
                user.UserStatus = "Active";
                user.EffectiveDate = DateTime.UtcNow.AddDays(-1);
                user.ExpirationDate = DateTime.UtcNow.AddDays(1);

                var usersMock = MockAsyncQueryable(user);
                dbCtxMock.Setup(x => x.Users).Returns(usersMock.Object);
            });

            //Act
            await target.IsActiveAsync(ctx);

            //Assert
            Assert.True(ctx.IsActive);
        }

        [Fact]
        public async Task ShouldBeAbleToGetIsActiveAsync_Active_NoExpiration() {
            //Arrange
            var user = CreateTestData<User>();
            var ctx = new IsActiveContext(CreatePrincipal(user), new Client(), "asdf");
            Arrange(() => {
                user.UserStatus = "Active";
                user.EffectiveDate = DateTime.UtcNow.AddDays(-1);
                user.ExpirationDate = null;

                var usersMock = MockAsyncQueryable(user);
                dbCtxMock.Setup(x => x.Users).Returns(usersMock.Object);
            });

            //Act
            await target.IsActiveAsync(ctx);

            //Assert
            Assert.True(ctx.IsActive);
        }

        [Fact]
        public async Task ShouldBeAbleToGetIsActiveAsync_InActive() {
            //Arrange
            var user = CreateTestData<User>();
            var ctx = new IsActiveContext(CreatePrincipal(user), new Client(), "asdf");
            Arrange(() => {
                user.UserStatus = "InActive";
                user.EffectiveDate = DateTime.UtcNow.AddDays(-1);
                user.ExpirationDate = DateTime.UtcNow.AddDays(1);

                var usersMock = MockAsyncQueryable(user);
                dbCtxMock.Setup(x => x.Users).Returns(usersMock.Object);
            });

            //Act
            await target.IsActiveAsync(ctx);

            //Assert
            Assert.False(ctx.IsActive);
        }

        [Fact]
        public async Task ShouldBeAbleToGetIsActiveAsync_InActive_NotEffective() {
            //Arrange
            var user = CreateTestData<User>();
            var ctx = new IsActiveContext(CreatePrincipal(user), new Client(), "asdf");
            Arrange(() => {
                user.UserStatus = "Active";
                user.EffectiveDate = DateTime.UtcNow.AddDays(1);
                user.ExpirationDate = DateTime.UtcNow.AddDays(2);

                var usersMock = MockAsyncQueryable(user);
                dbCtxMock.Setup(x => x.Users).Returns(usersMock.Object);
            });

            //Act
            await target.IsActiveAsync(ctx);

            //Assert
            Assert.False(ctx.IsActive);
        }

        [Fact]
        public async Task ShouldBeAbleToGetIsActiveAsync_InActive_Expired() {
            //Arrange
            var user = CreateTestData<User>();
            var ctx = new IsActiveContext(CreatePrincipal(user), new Client(), "asdf");
            Arrange(() => {
                user.UserStatus = "Active";
                user.EffectiveDate = DateTime.UtcNow.AddDays(-2);
                user.ExpirationDate = DateTime.UtcNow.AddDays(-1);

                var usersMock = MockAsyncQueryable(user);
                dbCtxMock.Setup(x => x.Users).Returns(usersMock.Object);
            });

            //Act
            await target.IsActiveAsync(ctx);

            //Assert
            Assert.False(ctx.IsActive);
        }

        private ClaimsPrincipal CreatePrincipal(User user) {
            return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("sub", user.UserId.ToString()) }));
        }

        private void ArrangeUserLists(User user) {
        }
    }
}

using System;
using System.Threading.Tasks;
using Chiron.Registration.Customer.Event;
using Chiron.Registration.Customer.Customer.Command;
using Cortside.Common.DomainEvent;
using Moq;
using Xunit;

namespace Chiron.Registration.Customer.Command.Handler.Tests {
    public class CreateCustomerHandlerTest : BaseTest {
        CreateCustomerHandler target;
        Mock<IHashProvider> hashProviderMock;
        Mock<IDomainEventPublisher> serviceBusMock;

        public CreateCustomerHandlerTest() {
            hashProviderMock = new Mock<IHashProvider>();
            serviceBusMock = new Mock<IDomainEventPublisher>();
            target = new CreateCustomerHandler(serviceBusMock.Object, hashProviderMock.Object);
        }

        [Fact]
        public void ShouldBeAbleToCreateCustomer() {
            //Arrange
            var command = CreateTestData<CreateCustomerCommand>();
            command.AgreeToTerms = true;
            var pwHash = Guid.NewGuid().ToString();
            serviceBusMock.Setup(x => x.SendAsync(It.IsAny<CustomerRegisteredEvent>())).Returns(Task.FromResult(0));
            hashProviderMock.Setup(x => x.ComputeHash(It.IsAny<string>())).Returns(pwHash);

            //Act
            target.Execute(command).Wait();

            //Assert
            serviceBusMock.Verify(x => x.SendAsync(It.Is<CustomerRegisteredEvent>(e =>
            e.AgreeToTerms.Date == DateTime.UtcNow.Date
            && e.Email == command.Email
            && e.FirstName == command.FirstName
            && e.LastName == command.LastName
            && e.PhoneNumber == command.PhoneNumber
            && e.Password == pwHash
            && !string.IsNullOrWhiteSpace(e.Salt)
            )));
        }

        [Fact]
        public void ShouldRequireFirstName() {
            //Arrange
            var command = CreateTestData<CreateCustomerCommand>();
            command.FirstName = null;

            //Act
            var exception = Assert.Throws<AggregateException>(() => target.Execute(command).Wait());

            //Assert
            Assert.Contains("First Name", exception.Message);
            serviceBusMock.Verify(x => x.SendAsync(It.IsAny<CustomerRegisteredEvent>()), Times.Never());
        }

        [Fact]
        public void ShouldRequireLastName() {
            //Arrange
            var command = CreateTestData<CreateCustomerCommand>();
            command.LastName = null;

            //Act
            var exception = Assert.Throws<AggregateException>(() => target.Execute(command).Wait());

            //Assert
            Assert.Contains("Last Name", exception.Message);
            serviceBusMock.Verify(x => x.SendAsync(It.IsAny<CustomerRegisteredEvent>()), Times.Never());
        }

        [Fact]
        public void ShouldRequireEmail() {
            //Arrange
            var command = CreateTestData<CreateCustomerCommand>();
            command.Email = null;

            //Act
            var exception = Assert.Throws<AggregateException>(() => target.Execute(command).Wait());

            //Assert
            Assert.Contains("Email", exception.Message);
            serviceBusMock.Verify(x => x.SendAsync(It.IsAny<CustomerRegisteredEvent>()), Times.Never());
        }

        [Fact]
        public void ShouldRequirePhoneNumber() {
            //Arrange
            var command = CreateTestData<CreateCustomerCommand>();
            command.PhoneNumber = null;

            //Act
            var exception = Assert.Throws<AggregateException>(() => target.Execute(command).Wait());

            //Assert
            Assert.Contains("Phone Number", exception.Message);
            serviceBusMock.Verify(x => x.SendAsync(It.IsAny<CustomerRegisteredEvent>()), Times.Never());
        }

        [Fact]
        public void ShouldRequirePassword() {
            //Arrange
            var command = CreateTestData<CreateCustomerCommand>();
            command.Password = null;

            //Act
            var exception = Assert.Throws<AggregateException>(() => target.Execute(command).Wait());

            //Assert
            Assert.Contains("Password", exception.Message);
            serviceBusMock.Verify(x => x.SendAsync(It.IsAny<CustomerRegisteredEvent>()), Times.Never());
        }

        [Fact]
        public void ShouldRequireTermsAgreement() {
            //Arrange
            var command = CreateTestData<CreateCustomerCommand>();
            command.AgreeToTerms = false;

            //Act
            var exception = Assert.Throws<AggregateException>(() => target.Execute(command).Wait());

            //Assert
            Assert.Contains("Terms Agreement", exception.Message);
            serviceBusMock.Verify(x => x.SendAsync(It.IsAny<CustomerRegisteredEvent>()), Times.Never());
        }
    }
}

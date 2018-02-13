using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Chiron.Registration.Customer.Event;
using Chiron.Registration.Customer.Customer.Command;
using Cortside.Common.Command;
using Cortside.Common.DomainEvent;

namespace Chiron.Registration.Customer.Command.Handler {
    public class CreateCustomerHandler : ICommandHandler<CreateCustomerCommand> {
        public IDomainEventPublisher ServiceBus { get; }
        public IHashProvider HashProvider { get; }

        public CreateCustomerHandler(IDomainEventPublisher serviceBus, IHashProvider hashProvider) {
            ServiceBus = serviceBus;
            HashProvider = hashProvider;
        }

        public async Task Execute(CreateCustomerCommand command) {
            //TODO: What am I supposed to do with this data? Store this?

            ValidateCommand(command);

            var salt = Guid.NewGuid().ToString();
            var password = HashProvider.ComputeHash(command.Password + salt);

            var @event = new CustomerRegisteredEvent {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                PhoneNumber = command.PhoneNumber,
                Password = password,
                Salt = salt,
                AgreeToTerms = DateTime.UtcNow
            };

            await ServiceBus.SendAsync(@event);
        }

        private void ValidateCommand(CreateCustomerCommand command) {
            var missing = new List<string>();
            if (string.IsNullOrWhiteSpace(command.FirstName)) {
                missing.Add("First Name");
            }
            if (string.IsNullOrWhiteSpace(command.LastName)) {
                missing.Add("Last Name");
            }
            if (string.IsNullOrWhiteSpace(command.Email)) { //TODO: Additional checks for valid email
                missing.Add("Email");
            }
            if (string.IsNullOrWhiteSpace(command.PhoneNumber)) { //TODO: Additional checks for valid phonenumber
                missing.Add("Phone Number");
            }
            if (string.IsNullOrWhiteSpace(command.Password)) {
                missing.Add("Password");
            }
            if (!command.AgreeToTerms) {
                missing.Add("Terms Agreement");
            }

            if (missing.Any()) {
                var missingStr = string.Join(", ", missing);
                var message = $"The following fields are required: {missingStr}";
                throw new ValidationException(message);
            }
        }
    }
}

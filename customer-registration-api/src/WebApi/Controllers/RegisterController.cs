using System.Threading.Tasks;
using Chiron.Registration.Customer.Customer.Command;
using Cortside.Common.Command;
using Microsoft.AspNetCore.Mvc;

namespace Chiron.Registration.Customer.WebApi.Controllers {
    [Route("api/[controller]")]
    public class RegisterController {
        public ICommandDispatcher CommandDispatcher { get; }

        public RegisterController(ICommandDispatcher commandDispatcher) {
            CommandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task Post([FromBody]CreateCustomerCommand data) {
            await CommandDispatcher.Dispatch(data);
        }
    }
}

using System.Collections.Generic;
using Chiron.Registration.Customer.BootStrap.Installer;
using Cortside.Common.BootStrap;

namespace Chiron.Registration.Customer.BootStrap {

    public class DefaultApplicationBootStrapper : BootStrapper {

        public DefaultApplicationBootStrapper() {
            installers = new List<IInstaller> {
        new LogInstaller(),
        new SqlInstaller(),
        new QueryInstaller(),
        new CommandHandlerInstaller(),
        new ServiceInstaller()
        };
        }
    }
}

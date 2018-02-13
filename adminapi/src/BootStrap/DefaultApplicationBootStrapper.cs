using System.Collections.Generic;
using Chiron.Admin.BootStrap.Installer;
using Cortside.Common.BootStrap;

namespace Chiron.Admin.BootStrap {

    public class DefaultApplicationBootStrapper : BootStrapper {

        public DefaultApplicationBootStrapper() {
            installers = new List<IInstaller> {
        //new LogInstaller(),
        new SqlInstaller(),
        new QueryInstaller()
        };
        }
    }
}

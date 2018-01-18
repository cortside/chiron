using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chiron.Auth.Data
{
    public interface IUserDbContextFactory
    {
	IUserDbContext NewUserDbContext();
    }
}

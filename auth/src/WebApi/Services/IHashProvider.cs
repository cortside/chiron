using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chiron.Auth.Services
{
    public interface IHashProvider
    {
	string ComputeHash(string theString);
    }
}

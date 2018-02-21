using System;
using System.Security.Cryptography;
using System.Text;

namespace Chiron.Auth.WebApi.Services {
    public class HashProvider : IHashProvider {
        public string ComputeHash(string theString) {
            var x = SHA512.Create();
            var data = Encoding.ASCII.GetBytes(theString);
            data = x.ComputeHash(data);
            var hash = Convert.ToBase64String(data);

            return hash;
        }
    }
}

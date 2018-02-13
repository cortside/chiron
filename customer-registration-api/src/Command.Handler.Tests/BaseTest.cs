using System;
using System.Linq;

namespace Chiron.Registration.Customer.Command.Handler.Tests {
    public abstract class BaseTest {
        static Random r = new Random();

        public T CreateTestData<T>() //Would normally use Autofixture, but not available for .NET Core
        {
            var instance = Activator.CreateInstance<T>();
            var props = typeof(T).GetProperties()
            .ToList();

            foreach (var x in props) {
                if (x.PropertyType == typeof(string)) {
                    x.SetValue(instance, Guid.NewGuid().ToString());
                } else if (x.PropertyType == typeof(bool)) {
                    x.SetValue(instance, Convert.ToBoolean(r.Next(0, 1)));
                } else if (x.PropertyType == typeof(short)) {
                    x.SetValue(instance, Convert.ToInt16(r.Next(short.MinValue, short.MaxValue)));
                } else if (x.PropertyType == typeof(int)) {
                    x.SetValue(instance, r.Next());
                } else if (x.PropertyType == typeof(long)) {
                    x.SetValue(instance, r.NextLong());
                } else if (x.PropertyType == typeof(double)) {
                    x.SetValue(instance, r.Next() + r.NextDouble());
                } else if (x.PropertyType == typeof(decimal)) {
                    x.SetValue(instance, Convert.ToDecimal(r.Next() + r.NextDouble()));
                } else if (x.PropertyType == typeof(float)) {
                    x.SetValue(instance, Convert.ToSingle(r.Next() + r.NextDouble()));
                } else if (x.PropertyType == typeof(byte[])) {
                    byte[] bytes = new byte[10];
                    r.NextBytes(bytes);
                    x.SetValue(instance, bytes);
                } else if (x.PropertyType == typeof(DateTime)) {
                    x.SetValue(instance, DateTime.Now);
                }
            }

            return instance;
        }
    }
}

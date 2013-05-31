using System;
using System.Linq;
using Castle.MicroKernel.Registration;

namespace CableCo.Common.Windsor
{
    /// <summary>
    /// Extensions for Castle fluent registration API
    /// </summary>
    public static class RegistrationExtensions
    {
        /// <summary>
        /// Selects service types from any interfaces implemented by the type (usually one) that
        /// have the named of the type prefixed by &quot;I&quot; prefix. For example, UserService
        /// would be registered with the service type of IUserService. An exception is thrown if 
        /// the type does not implement an interface with a name that matches this convention.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public static BasedOnDescriptor InterfaceWithIPrefix(this ServiceDescriptor descriptor)
        {
            ServiceDescriptor.ServiceSelector selector = (type, baseType) =>
            {
                string interfaceName = "I" + type.Name;
                var interfaceTypes = type.GetInterfaces().Where(x => x.Name == interfaceName);
                if (!interfaceTypes.Any())
                {
                    string message =
                        string.Format(
                            "Type {0} does not implement an interface with name {1}. Review the stack trace to see which registration logic relies on this convention.", type, interfaceName);
                    throw new InvalidOperationException(message);
                }
                return interfaceTypes;
            };
            return descriptor.Select(selector);
        }

    }
}
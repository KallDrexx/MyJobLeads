using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace MyJobLeads.DomainModel
{
    public static class TypeExtensions
    {
        public static bool IsDerivedFromOpenGenericType(this Type type, Type openGenericType)
        {
            Contract.Requires(type != null);
            Contract.Requires(openGenericType != null);
            Contract.Requires(openGenericType.IsGenericTypeDefinition);
            return type.GetTypeHierarchy()
                       .Where(t => t.IsGenericType)
                       .Select(t => t.GetGenericTypeDefinition())
                       .Any(t => openGenericType.Equals(t));
        }

        public static bool HasGenericInterface(this Type type, Type openGenericInterfaceType)
        {
            Contract.Requires(type != null);
            Contract.Requires(openGenericInterfaceType != null);
            Contract.Requires(openGenericInterfaceType.IsGenericTypeDefinition);
            return type.GetInterfaces()
                       .Where(t => t.IsGenericType)
                       .Select(t => t.GetGenericTypeDefinition())
                       .Any(t => openGenericInterfaceType.Equals(t));
        }

        public static IEnumerable<Type> GetTypeHierarchy(this Type type)
        {
            Contract.Requires(type != null);
            Type currentType = type;
            while (currentType != null)
            {
                yield return currentType;

                // Return all interfaces for the current type
                foreach (Type typeInterface in currentType.GetInterfaces())
                    yield return typeInterface;

                currentType = currentType.BaseType;
            }
        }
    }
}

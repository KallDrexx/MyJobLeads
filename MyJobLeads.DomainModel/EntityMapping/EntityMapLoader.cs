using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MyJobLeads.DomainModel.EntityMapping
{
    public class EntityMapLoader
    {
        public static void LoadEntityMappings()
        {
            // Load all IEntityMapConfiguration classes via reflection
            var types = AppDomain.CurrentDomain
                                 .GetAssemblies()
                                 .SelectMany(x => x.GetTypes())
                                 .Where(x => x.GetInterfaces().Contains(typeof(IEntityMapConfiguration)))
                                 .Select(x => Activator.CreateInstance(x) as IEntityMapConfiguration)
                                 .ToList();

            foreach (var config in types)
                config.ConfigureMapping();
        }
    }
}

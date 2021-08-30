using Planner.Application.Common.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace Planner.Application.Common.Mappers
{
    public static class MapperConfigurationRunner
    {
        public static void RunMapperConfigurations(Assembly assembly)
        {
            var mapperConfigurationType = typeof(IMapperConfiguration);

            var mapperConfigurationImplementations = assembly.GetTypes()
                .Where(x => mapperConfigurationType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToList();

            mapperConfigurationImplementations.ForEach(x =>
            {
                var constructor = x.GetConstructor(Type.EmptyTypes);
                var obj = constructor?.Invoke(new object[] { });

                var method = x.GetMethod("ConfigureMappings");

                method?.Invoke(obj, null);
            });
        }
    }
}
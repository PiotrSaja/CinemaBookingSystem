using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace CinemaBookingSystem.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        #region MappingProfile()
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }
        #endregion

        #region ApplyMappingsFromAssembly()
        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes().Where(p =>
                    p.GetInterfaces().Any(i => i.IsGenericType
                                               && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Wallhaven.Discord
{
    public static class WallhavenAssembly
    {
        /// <summary>
        /// Represents the assembly's name.
        /// </summary>
        public static string Name = Assembly.GetEntryAssembly().GetName().Name;
        public static string Contact = "admin@venipa.net";
        /// <summary>
        /// Represents the assembly's version.
        /// </summary>
        public static string Version = Assembly.GetEntryAssembly().GetName().Version.ToString(4);

        /// <summary>
        /// Find all methods in the assembly with the specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute being searched for.</typeparam>
        /// <returns>A list of all methods with the specified attribute.</returns>
        public static IEnumerable<Tuple<TAttribute, MethodInfo>> GetMethodsWithAttribute<TAttribute>() where TAttribute : Attribute
        {
            return from Method in AppDomain.CurrentDomain.GetAssemblies()
                    .Where(assembly => !assembly.GlobalAssemblyCache)
                    .SelectMany(assembly => assembly.GetTypes())
                    .SelectMany(type => type.GetMethods())
                   let Attribute = Attribute.GetCustomAttribute(Method, typeof(TAttribute), false) as TAttribute
                   where Attribute != null
                   select new Tuple<TAttribute, MethodInfo>(Attribute, Method);
        }
        public static IEnumerable<Type> GetClassesBasedOn<T>() where T : class
        {
            return from Class in AppDomain.CurrentDomain.GetAssemblies()
                    .Where(assembly => !assembly.GlobalAssemblyCache)
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(type => type != typeof(T))
                    .Where(type => type.IsSubclassOf(typeof(T)))
                   select Class;
        }
        public static string GetPath(params string[] path)
        {
            return Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory }.Concat(path).ToArray());
        }
    }

}

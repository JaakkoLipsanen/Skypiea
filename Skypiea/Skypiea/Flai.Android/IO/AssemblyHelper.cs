
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Flai.DataStructures;

namespace Flai.IO
{
    public static class AssemblyHelper
    {
#if !WINDOWS_PHONE
        public static readonly Assembly EntryAssembly = Assembly.GetEntryAssembly();
#endif

        public static ReadOnlyArray<Assembly> AllAssemblies = new ReadOnlyArray<Assembly>(AppDomain.CurrentDomain.GetAssemblies());


        private static readonly HashSet<string> ignoredNames = new HashSet<string>() { "Microsoft® .NET Framework", "Microsoft® Visual Studio® 2012" };
        public static Assembly[] GetAllNonFrameworkAssemblies()
        {
            return AssemblyHelper.GetAllNonFrameworkAssemblies(Common.ToEnumerable(AppDomain.CurrentDomain));
        }

        public static Assembly[] GetAllNonFrameworkAssemblies(IEnumerable<AppDomain> appDomains)
        {
            IEnumerable<Assembly> allAssemblies = appDomains.SelectMany(appDomain => appDomain.GetAssemblies());

#if !WINDOWS_PHONE
            Dictionary<Assembly, string> assemblies = allAssemblies.ToDictionary(assembly => assembly, assembly => assembly.GetCustomAttributesData().First(attr => attr.Constructor.DeclaringType == typeof(AssemblyProductAttribute)).ConstructorArguments[0].Value as string);
            return assemblies.Where(kvp => !ignoredNames.Contains(kvp.Value)).Select(kvp => kvp.Key).ToArray();
#else
            return allAssemblies.ToArray();
#endif
        }
    }
}
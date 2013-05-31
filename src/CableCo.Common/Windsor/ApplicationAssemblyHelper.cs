using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Castle.Core.Internal;
using Castle.MicroKernel.Registration;

namespace CableCo.Common.Windsor
{
    public static class ApplicationAssemblyHelper
    {
        private const string ApplicationAssemblyStart = "CableCo";

        /// <summary>
        /// Gets the directory containing assemblies for the current AppDomain
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyDirectory() 
        {
            var domain = AppDomain.CurrentDomain;
            var directory = domain.BaseDirectory;
            var webBinDirectory = Path.Combine(directory, "bin");
            if(Directory.Exists(webBinDirectory))
            {
                directory = webBinDirectory;
            }
            return directory;
        }

        /// <summary>
        /// Get's all assemblies in Application's base directory starting with the name &quot;Motive.ItsDesk.Scc&quot;.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetApplicationAssemblies() 
        {
            try
            {
                return ((IAssemblyProvider)AssemblyFilter).GetAssemblies();
            }
            catch(Exception exception)
            {
                throw new ApplicationException(@"This method relies on some internal functionality in Castle.Windsor. An exception has been thrown, which may have resulted from changes to Windsor.", exception);
            }
            
        }

        /// <summary>
        /// Castle.Windsor AssemblyFilter that finds all assemblies in Application's base directory starting with the &quot;CableCo&quot;.
        /// </summary>
        public static AssemblyFilter AssemblyFilter
        {
            get
            {
                string assemblyDirectory = GetAssemblyDirectory();

                var filter = new AssemblyFilter(assemblyDirectory).FilterByName(n => n.Name.StartsWith(ApplicationAssemblyStart));
                return filter;
            }
        }
    }
}
using System;
using System.IO;

namespace CableCo.Common.Utility
{
    /// <summary>
    /// Contains functionality for working with file and folder paths
    /// </summary>
    public class PathUtility
    {
        /// <summary>
        /// Combines a relative file path with the path of the current application (based on 
        /// the BaseDirectory property of the currently running AppDomain) and returns
        /// the full file path. In the context of a web application, the application directory will
        /// be the home directory of the web site. In the context of a WinForms application, the
        /// application will be the directory containing the currently running exe file.
        /// </summary>
        /// <param name="path">The path relative to the application folder. &quot;.&quot; can
        /// be used to reference the application folder. &quot;..&quot; can be used to reference
        /// parent paths. If the path contains an absolute path, then it will not be combined with
        /// the application path and will simply be returned as the return value.</param>
        /// <returns></returns>
        public static string CombineWithAppDomainPath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            string appDomainPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            return Path.GetFullPath(appDomainPath);
        }

        public static string CombineWebPath(string s1, string s2)
        {
            return string.Format("{0}/{1}", s1.EndsWith("/") ? s1.TrimEnd(Convert.ToChar("/")) : s1,
                                 s2.StartsWith("/") ? s2.TrimStart(Convert.ToChar("/")) : s2);
        }
    }
}
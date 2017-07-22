#if WINDOWS

using System.IO;

namespace Flai.IO
{
    public static class DirectoryHelper
    {
        public static string OutputDirectory
        {
            get { return Path.GetDirectoryName(AssemblyHelper.EntryAssembly.Location); }
        }

        public static string ExecutablePath
        {
            get { return AssemblyHelper.EntryAssembly.Location; }
        }

        public static FileInfo ExecutableInfo
        {
            get { return new FileInfo(DirectoryHelper.ExecutablePath); }
        }

        public static string ProjectDirectoryPath
        {
            get { return DirectoryHelper.ProjectDirectoryInfo.FullName; }
        }

        public static DirectoryInfo ProjectDirectoryInfo
        {
            get
            {
                if (DirectoryHelper.ExecutableInfo.Directory != null && DirectoryHelper.ExecutableInfo.Directory.Parent != null)
                {
                    return DirectoryHelper.ExecutableInfo.Directory.Parent.Parent;
                }

                return null;
            }
        }

        public static string SolutionDirectoryPath
        {
            get { return DirectoryHelper.SolutionDirectoryInfo.FullName; }
        }

        public static DirectoryInfo SolutionDirectoryInfo
        {
            get { return DirectoryHelper.ProjectDirectoryInfo.Parent; }
        }
    }

}

#endif

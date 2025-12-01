using System.IO;

namespace Lua.Plugins.Extensions;

public static class DirectoryExtensions
{
    extension(Directory)
    {
        public static void CreateDirectoryIfNeeded(string path)
        {
            if (Directory.Exists(path)) return;
            Directory.CreateDirectory(path);
        }
    }
}

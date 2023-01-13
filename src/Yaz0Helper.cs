using System.Reflection;
using System.Runtime.InteropServices;

namespace Yaz0Library
{
    public static class Yaz0Helper
    {
        private static readonly string[] Libs = { "Cead.dll" };
        private static bool IsLoaded = false;

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr SetDllDirectory(string lpFileName);

        public static void LoadDlls()
        {
            if (IsLoaded) {
                return;
            }

            string path = Path.Combine(Path.GetTempPath(), $"Yaz0Library-{typeof(Yaz0).Assembly.GetName().Version}");
            string dll = Path.Combine(path, "Cead.dll");
            // Always copy in debug mode
#if DEBUG
            Directory.CreateDirectory(path);
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Yaz0Library.Lib.Cead.dll");
            using FileStream fs = File.Create(dll);
            stream.CopyTo(fs);
#else
            if (!File.Exists(dll)) {
                Directory.CreateDirectory(path);
                using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Yaz0Library.Lib.Cead.dll");
                using FileStream fs = File.Create(dll);
                stream.CopyTo(fs);
            }
#endif

            SetDllDirectory(path);
            IsLoaded = true;
        }
    }
}

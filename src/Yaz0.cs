using Microsoft.Win32.SafeHandles;
using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Yaz0Library
{
    public partial class Yaz0
    {

        private static bool IsLoaded = false;

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr SetDllDirectory(string lpFileName);

        [LibraryImport("Cead.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool FreeResource(IntPtr vector_ptr);

        [LibraryImport("Cead.dll")]
        internal static unsafe partial void Compress(byte* src, int src_len, out VectorSafeHandle dst_handle, out byte* dst, out int dst_len, uint data_alignment, int level);

        [LibraryImport("Cead.dll")]
        internal static unsafe partial void Decompress(byte* src, int src_len, byte* dst, int dst_len);

        [LibraryImport("Yaz0.dll", EntryPoint = "decompress")]
        internal static unsafe partial byte* YazDecompress(byte* src, uint srcLen, uint* destLen);

        [LibraryImport("Yaz0.dll", EntryPoint = "compress")]
        internal static unsafe partial byte* YazCompress(byte* src, uint srcLen, uint* destLen, byte optCompr);

        [LibraryImport("Yaz0.dll", EntryPoint = "freePtr")]
        internal static unsafe partial void FreePtr(void* ptr);

        internal static void LoadDlls()
        {
            if (IsLoaded) {
                return;
            }

            string path = Path.Combine(Path.GetTempPath(), "Yaz0Library");
            string dll = Path.Combine(path, "Cead.dll");

            // Extract unmanaged DLL (assume there will be no newer version of Yaz0.dll)
#if DEBUG
            Directory.CreateDirectory(path);
            using Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream($"Yaz0Library.Lib.Cead.dll");
            using FileStream fs = File.Create(dll);
            stream.CopyTo(fs);
#else
            if (!File.Exists(dll) || true) {
                Directory.CreateDirectory(path);
                using Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream($"Yaz0Library.Lib.Cead.dll");
                using FileStream fs = File.Create(dll);
                stream.CopyTo(fs);
            }
#endif

            SetDllDirectory(path);
            IsLoaded = true;
        }

        public static unsafe Span<byte> Decompress(ReadOnlySpan<byte> src)
        {
            LoadDlls();

            Span<byte> dst = new byte[BinaryPrimitives.ReadUInt32BigEndian(src[0x04..0x08])];

            fixed (byte* src_ptr = src) {
                fixed (byte* dst_ptr = dst) {
                    Decompress(src_ptr, src.Length, dst_ptr, dst.Length);
                }
            }

            return dst;
        }
    }
}

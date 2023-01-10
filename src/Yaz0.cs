using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace Yaz0Library
{
    public static partial class Yaz0
    {
        [LibraryImport("Cead.dll")]
        internal static unsafe partial void Compress(byte* src, int src_len, out VectorSafeHandle dst_handle, out byte* dst, out int dst_len, uint data_alignment, int level);

        [LibraryImport("Cead.dll")]
        internal static unsafe partial void Decompress(byte* src, int src_len, byte* dst, int dst_len);

        [LibraryImport("Cead.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool FreeResource(IntPtr vector_ptr);

        public static unsafe ReadOnlySpan<byte> Compress(ReadOnlySpan<byte> src, int level = 7)
        {
            Yaz0Helper.LoadDlls();

            fixed (byte* srcPtr = src) {
                Compress(srcPtr, src.Length, out VectorSafeHandle dstHandle, out byte* dst, out int dstLen, 0, level);
                return new(dst, dstLen);
            }
        }

        public static unsafe Span<byte> Decompress(ReadOnlySpan<byte> src)
        {
            Yaz0Helper.LoadDlls();

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

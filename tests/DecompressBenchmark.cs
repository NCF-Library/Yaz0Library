using BenchmarkDotNet.Attributes;
using Yaz0Library;

namespace Yaz0Tests
{
    [MemoryDiagnoser]
    public class DecompressBenchmark
    {
        private const string Path = "D:\\Bin\\Yaz0Library\\Enemy_Lynel_Dark.sbactorpack";

        [Benchmark]
        public byte[] Yaz0Impl()
        {
            return Yaz0.DecompressUnmanaged(Path);
        }

        [Benchmark]
        public byte[] IlImpl()
        {
            return Yaz0.DecompressManaged(Path);
        }

        [Benchmark]
        public Span<byte> CeadImpl()
        {
            return Yaz0.Decompress(File.ReadAllBytes(Path));
        }
    }
}

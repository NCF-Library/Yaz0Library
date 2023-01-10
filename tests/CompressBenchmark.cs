using BenchmarkDotNet.Attributes;
using Yaz0Library;

namespace Yaz0Tests
{
    [MemoryDiagnoser]
    public class CompressBenchmark
    {
        private const string Path = "D:\\Bin\\Yaz0Library\\Enemy_Lynel_Dark.bactorpack";

        [Benchmark]
        public byte[] IlImpl()
        {
            return Yaz0Managed.Compress(Path);
        }

        [Benchmark]
        public ReadOnlySpan<byte> CeadImpl()
        {
            return Yaz0.Compress(File.ReadAllBytes(Path));
        }
    }
}

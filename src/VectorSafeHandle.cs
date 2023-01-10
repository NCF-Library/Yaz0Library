using Microsoft.Win32.SafeHandles;

namespace Yaz0Library
{
    internal class VectorSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal VectorSafeHandle() : base(true) { }
        protected override bool ReleaseHandle()
        {
            return Yaz0.FreeResource(handle);
        }
    }
}

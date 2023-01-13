using Microsoft.Win32.SafeHandles;

namespace Yaz0Library
{
    public class Yaz0SafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public Yaz0SafeHandle() : base(true) { }
        protected override bool ReleaseHandle()
        {
            return Yaz0.FreeResource(handle);
        }
    }
}

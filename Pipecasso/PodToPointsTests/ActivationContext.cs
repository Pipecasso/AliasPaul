using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace PodToPointsTests
{
    class ActivationContext : IDisposable
    {
		private readonly UnsafeNativeMethods.ACTCTX _context;
		private IntPtr _hActCtx = (IntPtr)(-1);
        private IntPtr _cookie = IntPtr.Zero;

        public ActivationContext(string manifest, string assemblyDirectory)
        {
            // Set up context definition data structure
             _context = new UnsafeNativeMethods.ACTCTX();
            _context.cbSize = Marshal.SizeOf(typeof(UnsafeNativeMethods.ACTCTX));
            if (_context.cbSize != 0x20 && _context.cbSize != 0x38)
            {
                throw new Exception("ACTCTX.cbSize is wrong");
            }
            _context.lpSource = manifest;
            _context.lpAssemblyDirectory = assemblyDirectory;
            _context.dwFlags |= 4; // Assembly directory valid

            // Create context
            _hActCtx = UnsafeNativeMethods.CreateActCtx(ref _context);
            var errCode = Marshal.GetLastWin32Error();
            if (_hActCtx == (IntPtr)(-1)) { throw new Win32Exception(errCode); }
        }

        public void Activate()
        {
            if (!UnsafeNativeMethods.ActivateActCtx(_hActCtx, out _cookie))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public void Deactivate()
        {
            UnsafeNativeMethods.DeactivateActCtx(0, _cookie);
            _cookie = IntPtr.Zero;
        }

        #region Disposable/Cleanup
        private bool _disposed = false;

        ~ActivationContext()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }

                if (_cookie != IntPtr.Zero)
                {
                    UnsafeNativeMethods.DeactivateActCtx(0, _cookie);
                    _cookie = IntPtr.Zero;
                }

                if (_hActCtx != (IntPtr)(-1))
                {
                    UnsafeNativeMethods.ReleaseActCtx(_hActCtx);
                    _hActCtx = (IntPtr) (-1);
                }
            }

            _disposed = true;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
        #endregion



        static public void UsingManifestDo(string manifest, Action action)
        {
            UnsafeNativeMethods.ACTCTX context = new UnsafeNativeMethods.ACTCTX();
            context.cbSize = Marshal.SizeOf(typeof(UnsafeNativeMethods.ACTCTX));
            if (context.cbSize != 0x20)
            {
                throw new Exception("ACTCTX.cbSize is wrong");
            }
            context.lpSource = manifest;
 
            IntPtr hActCtx = UnsafeNativeMethods.CreateActCtx(ref context);
            var errCode = Marshal.GetLastWin32Error();
            if (hActCtx == (IntPtr)(-1)) { throw new Win32Exception(errCode); }

            try // with valid hActCtx
            {
                IntPtr cookie = IntPtr.Zero;
                if (!UnsafeNativeMethods.ActivateActCtx(hActCtx, out cookie))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                try // with activated context
                {
                    action();
                }
                finally
                {
                    UnsafeNativeMethods.DeactivateActCtx(0, cookie);
                }
            }
            finally
            {
                UnsafeNativeMethods.ReleaseActCtx(hActCtx);
            }
        }
 
        [SuppressUnmanagedCodeSecurity]
        internal static class UnsafeNativeMethods
        {
            // Activation Context API Functions
            [DllImport("Kernel32.dll", SetLastError = true, EntryPoint = "CreateActCtxW")]
            internal extern static IntPtr CreateActCtx(ref ACTCTX actctx);
 
            [DllImport("Kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool ActivateActCtx(IntPtr hActCtx, out IntPtr lpCookie);
 
            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DeactivateActCtx(int dwFlags, IntPtr lpCookie);
 
            [DllImport("Kernel32.dll", SetLastError = true)]
            internal static extern void ReleaseActCtx(IntPtr hActCtx);
 
            // Activation context structure
            // Pack = 4,
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal struct ACTCTX
            {
                public int cbSize;
                public UInt32 dwFlags;
                public string lpSource;
                public UInt16 wProcessorArchitecture;
                public UInt16 wLangId;
                public string lpAssemblyDirectory;
                public string lpResourceName;
                public string lpApplicationName;
                public IntPtr hModule;
            }
 
        }
    }
}

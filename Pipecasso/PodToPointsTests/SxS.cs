using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PodToPointsTests
{
    [TestClass]
    public class SxS
    {
        private static ActivationContext _actCtx;

        [AssemblyInitialize]
        public static void CreateActCtx(TestContext context)
        {
            // Assembly root is parent (if running in build folder) or deployment folder
            var execAssemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (System.AppDomain.CurrentDomain.ShadowCopyFiles)
            {
                execAssemblyDir = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            }
            string sxsAssemblyDir = execAssemblyDir;

            string sxsManifestPath = Path.Combine(execAssemblyDir, Assembly.GetExecutingAssembly().GetName().Name);
            sxsManifestPath += (IntPtr.Size == 4) ? ".SxS.manifest" : ".SxS.amd64.manifest";

            _actCtx = new ActivationContext(sxsManifestPath, sxsAssemblyDir);
            _actCtx.Activate();

            // Create AssembyLoad handler
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemblyDll = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";

            foreach (string path in Directory.EnumerateFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), assemblyDll, SearchOption.AllDirectories))
            {
                return Assembly.LoadFrom(path);
            }

            return null;
        }

        [AssemblyCleanup]
        public static void DeleteActCtx()
        {
            _actCtx.Deactivate();
            _actCtx.Dispose();

            // Create AssembyLoad handler
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        }

        internal static ActivationContext ActCtx { get { return _actCtx; } }
    }
}

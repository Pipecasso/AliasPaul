using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedPodLoader;
using Intergraph.PersonalISOGEN;
using AliasPOD;
using System.IO;
using System.Windows.Forms;

namespace ConnectComponentsRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            string pod = args[0];
            string manifest = args[1];
            string newname = Path.GetFileNameWithoutExtension(pod) + "con" + ".pod";
            string podcon = Path.Combine(Path.GetDirectoryName(pod), newname);

          
            LoadedPod lp = new LoadedPod(manifest, pod);
            MessageBox.Show("Attach Debugger Now");
            using (IsogenAssemblyLoaderCookie ial = new IsogenAssemblyLoaderCookie(lp.isogenAssemblyLoader))
            {
                POD podfile = lp.pod;
                podfile.Pipelines.Item(0).ConnectComponents();
                podfile.Save(podcon);
            }

        
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intergraph.PersonalISOGEN;
using Intergraph.PersonalISOGEN.ProjectManagerNet.ProjectManagement;
using System.IO;


namespace ProjectManagerTest2012
{
    class Program
    {
        static void Main(string[] args)
        {
            string Manifest = args[0];
            string ManifestDir = Path.GetDirectoryName(Manifest);
            string CoreComponents = Path.Combine(ManifestDir, "Core Components");
            IsogenAssemblyLoader ial = new IsogenAssemblyLoader(Manifest, ManifestDir);
            ial.AddStringPath(CoreComponents);
            using (IsogenAssemblyLoaderCookie cookieMonster = new IsogenAssemblyLoaderCookie(ial))
            {
                DoTheProjectManagerThing();

            }
        
        }

        static void DoTheProjectManagerThing()
        {
            ProjectManager pm = new ProjectManager();
        }
    }
}

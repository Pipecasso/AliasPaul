using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intergraph.PersonalISOGEN;
using Intergraph.PersonalISOGEN.ProjectManagerNet.ProjectManagement;
using System.IO;

namespace ProjectManagerTest2019
{
    class Program
    {
        static void Main(string[] args)
        {

            string Manifest = args[0];
            string ManifestDir = Path.GetDirectoryName(Manifest);
            string CoreComponents = Path.Combine(ManifestDir, "Core Components");
            IsogenAssemblyLoader ial = new IsogenAssemblyLoader(Manifest, ManifestDir,ManifestDir,true);
          //  ial.AddStringPath(CoreComponents);
            using (IsogenAssemblyLoaderCookie cookieMonster = new IsogenAssemblyLoaderCookie(ial))
            {
                try
                {
                    DoTheProjectManagerThing();
                }
                catch (System.IO.FileNotFoundException fnf)
                {
                    string error = ial.GetErrorMessage();
                }



            }
        }

        static void DoTheProjectManagerThing()
        {
            ProjectManager pm = new ProjectManager();
        }

    }
}

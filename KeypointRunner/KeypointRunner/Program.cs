using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intergraph.PersonalISOGEN;
using AliasPOD;
using PodHandshake;
using System.IO;
using AliasGeometry;

namespace KeypointRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <3)
            {
                return;

                //pod component manifest
            }

            string manifest_file = args[2];
            string manifestRootFolder = System.IO.Path.GetDirectoryName(manifest_file);

            POD mypod = null;
            IsogenAssemblyLoader ial = new Intergraph.PersonalISOGEN.IsogenAssemblyLoader(manifest_file, manifestRootFolder);
            using (IsogenAssemblyLoaderCookie monster = new IsogenAssemblyLoaderCookie(ial))
            {
                mypod = new POD();
                mypod.Handshake = HandshakeTools.GetPODHandshake(manifestRootFolder);
                mypod.Load(args[0]);
             
            }

            KeypointRunner keypointRunner = new KeypointRunner(mypod, ial);
            bool bOk = keypointRunner.Initialise(args[1], "Run_2");
            keypointRunner.keypointRunItems.Sort3D();

            using (StreamWriter sw = new StreamWriter("Seb.log"))
            {
                using (IsogenAssemblyLoaderCookie monster = new IsogenAssemblyLoaderCookie(ial))
                {
                    keypointRunner.keypointRunItems.Sort2D();

                    foreach (KeypointRunItem keypointRunItem in keypointRunner.keypointRunItems)
                    {
                        Point3d p3d = keypointRunItem.Get3d();
                        Point2d p2d = keypointRunItem.Get2d();
                        double d2d =  Point2d.Distance(keypointRunner.keypointRunItems.Anchor.Get2d(), keypointRunItem.Get2d());
                        double d3d = Point3d.Distance(keypointRunner.keypointRunItems.Anchor.Get3d(), keypointRunItem.Get3d());
                        sw.WriteLine($"{keypointRunItem.Name}, {p3d.X},{p3d.Y},{p3d.Z},,{p2d.X},{p2d.Y},{d2d},{d3d}");
                    }

                }
            }



        }
    }
}

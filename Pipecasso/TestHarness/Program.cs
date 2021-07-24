using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedPodLoader;
using PodToPoints;
using Projector;
using AliasGeometry;
using System.IO;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            string manifest = args[0];
            string podpath = args[1];


            PODTransformer podTransformer = new PODTransformer(manifest, podpath);
            Dictionary<dynamic, Shapes3d> podshapes = podTransformer.Shapes3;
            List<Point3d> myPoints = podTransformer.GetPoints();

            using (StreamWriter sw = new StreamWriter("cubepoints.txt"))
            {
                foreach (Point3d p in myPoints)
                {
                    string spoint = $"{p.X} | {p.Y} | {p.Z}";
                    sw.WriteLine(spoint);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedPodLoader;
using PodToPoints;
using Projector;

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
            Camera camera = new Camera()

        
        }
    }
}
